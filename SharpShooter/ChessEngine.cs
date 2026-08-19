using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;

namespace SharpShooter
{

  public class ChessEngine
  {
    public ChessEngine(string fen) { SetPositionFromFen(fen); }

    CastlingRights myCastlingRights;

    Board Board() { return myBoard; }
    private Board myBoard;

    // Can be null if the position is invalid. assume it isn't otherwise.
    public Colour? MyTurn { get; private set; }

    public void SetPositionFromFen(string fen)
    {
      Fen parsedFen = FenParser.Parse(fen);
      myBoard = new Board(parsedFen);
      myCastlingRights = parsedFen.Castling;
      MyTurn = parsedFen.WhosTurn;
    }

    public bool IsValidPosition()
    {
      // A board is valid if it has one white king and one black king.

      Square? whiteKingPosition = myBoard.FindKing(Colour.White);
      Square? blackKingPosition = myBoard.FindKing(Colour.Black);

      bool hasKings = whiteKingPosition.HasValue && blackKingPosition.HasValue;

      if (!hasKings) return false;

      // Search all pieces to see if there is an additional king that isn't either on the
      // white king's square or the black king's square.
      var whiteKingPiece = new Piece(Colour.White, Type.King);
      var blackKingPiece = new Piece(Colour.Black, Type.King);
      foreach (var (piece, positon) in myBoard.Pieces())
      {
        if (piece == whiteKingPiece && positon != whiteKingPosition) return false;
        if (piece == blackKingPiece && positon != blackKingPosition) return false;
        // If a pawn is found on the 1st or 8th ranks, it is illegal!
        if(piece.Type() == Type.Pawn)
        {
          if (positon.Rank == 0 || positon.Rank == 7) return false;
        }
      }

      bool kingsTouch = Math.Abs(whiteKingPosition!.Value.Rank - blackKingPosition!.Value.Rank) <= 1 &&
                        Math.Abs(whiteKingPosition!.Value.File - blackKingPosition!.Value.File) <= 1;

      if (kingsTouch) return false; 

      // If the board state claims that castling is available, then the kings must be on the
      // right squares as well as a rook must be present on the correct square for the respected side of castling.
      if (myCastlingRights.HasFlag(CastlingRights.BlackKingside))
      {
        if (myBoard.PieceAtPosition((4, 7)) != new Piece(Colour.Black, Type.King) ||
            myBoard.PieceAtPosition((7, 7)) != new Piece(Colour.Black, Type.Rook)) return false;
      }
      if (myCastlingRights.HasFlag(CastlingRights.WhiteKingside))
      {
        if (myBoard.PieceAtPosition((4, 0)) != new Piece(Colour.White, Type.King) ||
            myBoard.PieceAtPosition((7, 0)) != new Piece(Colour.White, Type.Rook)) return false;
      }
      if (myCastlingRights.HasFlag(CastlingRights.BlackQueenside))
      {
        if (myBoard.PieceAtPosition((4, 7)) != new Piece(Colour.Black, Type.King) ||
            myBoard.PieceAtPosition((0, 7)) != new Piece(Colour.Black, Type.Rook)) return false;
      }
      if (myCastlingRights.HasFlag(CastlingRights.WhiteQueenside))
      {
        if (myBoard.PieceAtPosition((4, 0)) != new Piece(Colour.White, Type.King) ||
            myBoard.PieceAtPosition((0, 0)) != new Piece(Colour.White, Type.Rook)) return false;
      }

      return true;
    }

    // Returns true if the current player's Colour is in check. False otherwise
    // :NOTE: it's impossible for the other player to be in check on your Colour.
    // The game ends before that.
    public bool IsInCheck()
    {
      // :TODO: This is going to explode when IsValidPosition calls IsInCheck to handle illegal
      // positions involving both kings in check. This funciton assumes a valid state as it only
      // checks the king who's turn it is.
      Debug.Assert(IsValidPosition());
      // Early exit for garbage case.
      if (!IsValidPosition()) return false;

      Square kingWhoCouldBeInCheck = myBoard.FindKing(MyTurn!.Value)!.Value;

      // Check if there are any rooks of the opposite colour on the same rank and file.
      Colour otherColour = MyTurn!.Value == Colour.White ? Colour.Black : Colour.White;

      // Queen checks
      var queenToLookFor = new Piece(otherColour, Type.Queen);
      if (CheckInADirection(kingWhoCouldBeInCheck, 1, 0, queenToLookFor)) return true;
      if (CheckInADirection(kingWhoCouldBeInCheck, 0, 1, queenToLookFor)) return true;
      if (CheckInADirection(kingWhoCouldBeInCheck, -1, 0, queenToLookFor)) return true;
      if (CheckInADirection(kingWhoCouldBeInCheck, 0, -1, queenToLookFor)) return true;
      if (CheckInADirection(kingWhoCouldBeInCheck, 1, 1, queenToLookFor)) return true;
      if (CheckInADirection(kingWhoCouldBeInCheck, 1, -1, queenToLookFor)) return true;
      if (CheckInADirection(kingWhoCouldBeInCheck, -1, -1, queenToLookFor)) return true;
      if (CheckInADirection(kingWhoCouldBeInCheck, -1, 1, queenToLookFor)) return true;
      // Rook checks.
      var rookToLookFor = new Piece(otherColour, Type.Rook);

      // Naively go through every file along the same rank to see if a rook of the opposite colour is there.
      if (CheckInADirection(kingWhoCouldBeInCheck, 1, 0, rookToLookFor)) return true;
      if (CheckInADirection(kingWhoCouldBeInCheck, 0, 1, rookToLookFor)) return true;
      if (CheckInADirection(kingWhoCouldBeInCheck, -1, 0, rookToLookFor)) return true;
      if (CheckInADirection(kingWhoCouldBeInCheck, 0, -1, rookToLookFor)) return true;

      // Bishop checks.
      var bishopToLookFor = new Piece(otherColour, Type.Bishop);
      if (CheckInADirection(kingWhoCouldBeInCheck, 1, 1, bishopToLookFor)) return true;
      if (CheckInADirection(kingWhoCouldBeInCheck, 1, -1, bishopToLookFor)) return true;
      if (CheckInADirection(kingWhoCouldBeInCheck, -1, -1, bishopToLookFor)) return true;
      if (CheckInADirection(kingWhoCouldBeInCheck, -1, 1, bishopToLookFor)) return true;

      // Knight checks
      // There are 8 possible cases ((+1,+2),(+2,+1),(+2,-1),(+1,-2),(-1,-2),(-2,-1),(-2,+1) and (-1,+2)).
      // We simply check if this square exists, if it does is there a knight of the opposite colour on it.
      var knightToLookFor = new Piece(otherColour, Type.Knight);
      var possibleOffsets = new (int, int)[] { (+1, +2), (+2, +1), (+2, -1), (+1, -2), (-1, -2), (-2, -1), (-2, +1), (-1, +2) };

      foreach (var possibleOffset in possibleOffsets)
      {
        if (CheckDiscrete(kingWhoCouldBeInCheck, possibleOffset.Item1, possibleOffset.Item2, knightToLookFor)) return true;
      }

      // Pawn checks
      // There are only two places to check, but it depends on the colour of the king.
      var pawnToLookFor = new Piece(otherColour, Type.Pawn);
      if (MyTurn!.Value == Colour.White)
      {
        // Check (+1,+1) and (-1,+1);
        if (CheckDiscrete(kingWhoCouldBeInCheck, 1, 1, pawnToLookFor)) return true;
        if (CheckDiscrete(kingWhoCouldBeInCheck, -1, 1, pawnToLookFor)) return true;
      }
      else
      {
        // Black king, check (+1,-1) and (-1,-1);
        if (CheckDiscrete(kingWhoCouldBeInCheck, 1, -1, pawnToLookFor)) return true;
        if (CheckDiscrete(kingWhoCouldBeInCheck, -1, -1, pawnToLookFor)) return true;
      }

      return false;
    }

    /// Looks in a specific direction for a peice. Expects either a Rook,Bishop or Queen peice to be provided.
    /// It is the callers responsibility to provide reasonable deltas (don't give a diagional delta for a rook check etc.)
    private bool CheckInADirection(Square KingsSqaure, int deltaFile, int deltaRank, Piece PeiceToLookFor)
    {
      // i starts at one because i = 0 would reveal the king.
      for (int i = 1; i < 8; i++)
      {
        var positionToCheck = new Square(KingsSqaure.File + (i * deltaFile), KingsSqaure.Rank + (i * deltaRank));
        if (positionToCheck.Rank < 0 || positionToCheck.Rank >= 8 ||
            positionToCheck.File < 0 || positionToCheck.File >= 8)
        {
          break;
        }
        Piece? pieceAtPosition = myBoard.PieceAtPosition(positionToCheck);
        if (pieceAtPosition != null)
        {
          // There is some other kind of peice in the way
          if (pieceAtPosition == PeiceToLookFor) return true;
          else
          {
            return false; // There is another peice in the way.
          }
        }
      }
      return false;
    }
    /// Looks in a specific location for a peice. Expects either a pawn or a knight peice to be provided.
    private bool CheckDiscrete(Square KingsSqaure, int deltaFile, int deltaRank, Piece PeiceToLookFor)
    {
      var positionToCheck = new Square(KingsSqaure.File + deltaFile, KingsSqaure.Rank + deltaRank);
      if (positionToCheck.Rank < 0 || positionToCheck.Rank >= 8 ||
          positionToCheck.File < 0 || positionToCheck.File >= 8)
      {
        return false;
      }
      Piece? pieceAtPosition = myBoard.PieceAtPosition(positionToCheck);
      return (pieceAtPosition == PeiceToLookFor);
    }

  }
}
