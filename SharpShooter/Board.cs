using SharpShooter.Pieces;

namespace SharpShooter
{
  public class Board
  {
    List<(Piece Piece, Square Square)> myPieces;

    public List<(Piece Piece, Square Square)> Pieces() { return myPieces; }

    public Board(string fen) : this(FenParser.Parse(fen)) { }
    CastlingRights myCastlingRights;

    // Can be null if the position is invalid. assume it isn't otherwise.
    public Colour? MyTurn { get; private set; }

    public Board(Fen fen)
    {
      myPieces = new();
      foreach (var (piece, position) in fen.Pieces)
      {
        myPieces!.Add((piece, position));
      }
      myCastlingRights = fen.Castling;
      MyTurn = fen.WhosTurn;
    }

    /// <summary>
    /// Lets you know if a square is on the board or not.
    /// </summary>
    /// <param name="square"></param>
    /// <returns></returns>
    public bool IsOnBoard(Square square)
    {
      return square.File >= 0 && square.File < 8 &&
             square.Rank >= 0 && square.Rank < 8;
    }

    public Square? FindKing(Colour colour)
    {
      // This method assumes the board is valid (because then a king will exist).
      // Debug.Assert(IsValidPosition()); is what I would like to assert, but that is recursive!
      var pieceToLookFor = new King(colour);
      foreach (var (peice, position) in myPieces)
      {
        // :MYTODO: Ensure this isn't a check by reference!
        if (peice == pieceToLookFor) return position;
      }
      return null;
    }


    public Piece? PieceAtPosition(Square location)
    {
      // :NOTE: O(n) piece look up is not ideal... #1
      foreach (var (piece, position) in myPieces)
      {
        if (position == location)
        {
          return piece;
        }
      }
      // Not found.
      return null;
    }

    public List<Move> GenerateLegalMovesForPieceAtSqaure(Piece piece, Square square)
    {
      return piece.GenerateLegalMoves(square, this);
    }

    public bool IsValidPosition()
    {
      // A board is valid if it has one white king and one black king.

      Square? whiteKingPosition = FindKing(Colour.White);
      Square? blackKingPosition = FindKing(Colour.Black);

      bool hasKings = whiteKingPosition.HasValue && blackKingPosition.HasValue;

      if (!hasKings) return false;

      // Search all pieces to see if there is an additional king that isn't either on the
      // white king's square or the black king's square.
      var whiteKingPiece = new King(Colour.White);
      var blackKingPiece = new King(Colour.Black);
      var pieces = Pieces();
      foreach (var (piece, positon) in pieces)
      {
        if (piece == whiteKingPiece && positon != whiteKingPosition) return false;
        if (piece == blackKingPiece && positon != blackKingPosition) return false;
        // If a pawn is found on the 1st or 8th ranks, it is illegal!
        if (piece.Type() == PieceType.Pawn)
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
        if (PieceAtPosition((4, 7)) != new King(Colour.Black) ||
            PieceAtPosition((7, 7)) != new Rook(Colour.Black)) return false;
      }
      if (myCastlingRights.HasFlag(CastlingRights.WhiteKingside))
      {
        if (PieceAtPosition((4, 0)) != new King(Colour.White) ||
            PieceAtPosition((7, 0)) != new Rook(Colour.White)) return false;
      }
      if (myCastlingRights.HasFlag(CastlingRights.BlackQueenside))
      {
        if (PieceAtPosition((4, 7)) != new King(Colour.Black) ||
            PieceAtPosition((0, 7)) != new Rook(Colour.Black)) return false;
      }
      if (myCastlingRights.HasFlag(CastlingRights.WhiteQueenside))
      {
        if (PieceAtPosition((4, 0)) != new King(Colour.White) ||
            PieceAtPosition((0, 0)) != new Rook(Colour.White)) return false;
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
      //Debug.Assert(IsValidPosition());
      // Early exit for garbage case.
      if (!IsValidPosition()) return false;

      Square kingWhoCouldBeInCheck = FindKing(MyTurn!.Value)!.Value;

      // Check if there are any rooks of the opposite colour on the same rank and file.
      Colour otherColour = MyTurn!.Value == Colour.White ? Colour.Black : Colour.White;

      // Queen checks
      var queenToLookFor = new Queen(otherColour);
      if (CheckForPieceInADirection(kingWhoCouldBeInCheck, 1, 0, queenToLookFor)) return true;
      if (CheckForPieceInADirection(kingWhoCouldBeInCheck, 0, 1, queenToLookFor)) return true;
      if (CheckForPieceInADirection(kingWhoCouldBeInCheck, -1, 0, queenToLookFor)) return true;
      if (CheckForPieceInADirection(kingWhoCouldBeInCheck, 0, -1, queenToLookFor)) return true;
      if (CheckForPieceInADirection(kingWhoCouldBeInCheck, 1, 1, queenToLookFor)) return true;
      if (CheckForPieceInADirection(kingWhoCouldBeInCheck, 1, -1, queenToLookFor)) return true;
      if (CheckForPieceInADirection(kingWhoCouldBeInCheck, -1, -1, queenToLookFor)) return true;
      if (CheckForPieceInADirection(kingWhoCouldBeInCheck, -1, 1, queenToLookFor)) return true;
      // Rook checks.
      var rookToLookFor = new Rook(otherColour);

      // Naively go through every file along the same rank to see if a rook of the opposite colour is there.
      if (CheckForPieceInADirection(kingWhoCouldBeInCheck, 1, 0, rookToLookFor)) return true;
      if (CheckForPieceInADirection(kingWhoCouldBeInCheck, 0, 1, rookToLookFor)) return true;
      if (CheckForPieceInADirection(kingWhoCouldBeInCheck, -1, 0, rookToLookFor)) return true;
      if (CheckForPieceInADirection(kingWhoCouldBeInCheck, 0, -1, rookToLookFor)) return true;

      // Bishop checks.
      var bishopToLookFor = new Bishop(otherColour);
      if (CheckForPieceInADirection(kingWhoCouldBeInCheck, 1, 1, bishopToLookFor)) return true;
      if (CheckForPieceInADirection(kingWhoCouldBeInCheck, 1, -1, bishopToLookFor)) return true;
      if (CheckForPieceInADirection(kingWhoCouldBeInCheck, -1, -1, bishopToLookFor)) return true;
      if (CheckForPieceInADirection(kingWhoCouldBeInCheck, -1, 1, bishopToLookFor)) return true;

      // Knight checks
      // There are 8 possible cases ((+1,+2),(+2,+1),(+2,-1),(+1,-2),(-1,-2),(-2,-1),(-2,+1) and (-1,+2)).
      // We simply check if this square exists, if it does is there a knight of the opposite colour on it.
      var knightToLookFor = new Knight(otherColour);
      var possibleOffsets = new (int, int)[] { (+1, +2), (+2, +1), (+2, -1), (+1, -2), (-1, -2), (-2, -1), (-2, +1), (-1, +2) };

      foreach (var possibleOffset in possibleOffsets)
      {
        if (CheckForPieceDiscrete(kingWhoCouldBeInCheck, possibleOffset.Item1, possibleOffset.Item2, knightToLookFor)) return true;
      }

      // Pawn checks
      // There are only two places to check, but it depends on the colour of the king.
      var pawnToLookFor = new Pawn(otherColour);
      if (MyTurn!.Value == Colour.White)
      {
        // Check (+1,+1) and (-1,+1);
        if (CheckForPieceDiscrete(kingWhoCouldBeInCheck, 1, 1, pawnToLookFor)) return true;
        if (CheckForPieceDiscrete(kingWhoCouldBeInCheck, -1, 1, pawnToLookFor)) return true;
      }
      else
      {
        // Black king, check (+1,-1) and (-1,-1);
        if (CheckForPieceDiscrete(kingWhoCouldBeInCheck, 1, -1, pawnToLookFor)) return true;
        if (CheckForPieceDiscrete(kingWhoCouldBeInCheck, -1, -1, pawnToLookFor)) return true;
      }

      return false;
    }

    /// It is the callers responsibility to provide reasonable deltas (don't give a diagional delta for a rook check etc.)
    private List<Move> MovesAlongRay(Square InitalSqaure, int deltaFile, int deltaRank, Colour pieceColour)
    {
      List<Move> candidateMoves = new List<Move>();
      // i starts at one because i = 0 would reveal the piece we are starting at.
      for (int i = 1; i < 8; i++)
      {
        var candidateSquare = new Square(InitalSqaure.File + (i * deltaFile), InitalSqaure.Rank + (i * deltaRank));
        if (candidateSquare.Rank < 0 || candidateSquare.Rank >= 8 ||
            candidateSquare.File < 0 || candidateSquare.File >= 8)
        {
          break;
        }
        Piece? pieceAtPosition = PieceAtPosition(candidateSquare);
        if (pieceAtPosition != null)
        {
          // There is some other kind of peice in the way, can we capture it?
          if (pieceAtPosition.Colour() == pieceColour)
          {
            // We cannot capture it and we can't keep looking past this piece.
            return candidateMoves;
          }
          else
          {
            // We can capture this piece but go no further.
            candidateMoves.Add((InitalSqaure, candidateSquare));
            return candidateMoves;
          }
        }
        else
        {
          candidateMoves.Add((InitalSqaure, candidateSquare));
        }
      }
      return candidateMoves;
    }

    /// Looks in a specific direction until a piece is in the way or the edge of the board is found.
    /// returns true if the expected Piece is found along that direction, false otherwise.
    /// Expects either a Rook,Bishop or Queen peice to be provided.
    /// It is the callers responsibility to provide reasonable deltas (don't give a diagional delta for a rook check etc.)
    private bool CheckForPieceInADirection(Square kingsSqaure, int deltaFile, int deltaRank, Piece peiceToLookFor)
    {
      // Colour of the king is actually the opposite colour of peiceToLookFor.
      Colour kingsColour = peiceToLookFor.Colour() == Colour.White ? Colour.Black : Colour.White;
      var candidateMoves = MovesAlongRay(kingsSqaure, deltaFile, deltaRank, kingsColour);
      if (candidateMoves.Count == 0) return false;
      // The last square in that direction is either the edge of the board or a piece.
      var lastSquare = candidateMoves.Last().AfterSquare;
      var pieceAtEndOfRay = PieceAtPosition(lastSquare);
      if (pieceAtEndOfRay == null)
      {
        return false; // The peice we were looking found could not be found because the edge of the board was found.
      }
      else
      {
        return pieceAtEndOfRay == peiceToLookFor;
      }
    }

    /// Looks in a specific location for a peice. Expects either a pawn or a knight peice to be provided.
    private bool CheckForPieceDiscrete(Square KingsSqaure, int deltaFile, int deltaRank, Piece PeiceToLookFor)
    {
      var positionToCheck = new Square(KingsSqaure.File + deltaFile, KingsSqaure.Rank + deltaRank);
      if (positionToCheck.Rank < 0 || positionToCheck.Rank >= 8 ||
          positionToCheck.File < 0 || positionToCheck.File >= 8)
      {
        return false;
      }
      Piece? pieceAtPosition = PieceAtPosition(positionToCheck);
      return (pieceAtPosition == PeiceToLookFor);
    }
  }
}
