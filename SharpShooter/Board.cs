using SharpShooter.Pieces;
using System.Diagnostics;

namespace SharpShooter
{
  public class Board
  {
    List<(Piece Piece, Square Square)> myPieces;

    public List<(Piece Piece, Square Square)> Pieces() { return myPieces; }

    public Board(string fen) : this(FenParser.Parse(fen)) { }
    private CastlingRights myCastlingRights;

    public bool CanCastleKingside(Colour colour)
    {
      if (colour == Colour.White)
      {
        return myCastlingRights.HasFlag(CastlingRights.WhiteKingside);
      }
      else
      {
        Debug.Assert(colour == Colour.Black);
        return myCastlingRights.HasFlag(CastlingRights.BlackKingside);
      }
    }

    public bool CanCastleQueenside(Colour colour)
    {
      if (colour == Colour.White)
      {
        return myCastlingRights.HasFlag(CastlingRights.WhiteQueenside);
      }
      else
      {
        Debug.Assert(colour == Colour.Black);
        return myCastlingRights.HasFlag(CastlingRights.BlackQueenside);
      }
    }

    // Can be null if the position is invalid. assume it isn't otherwise.
    public Colour? myTurn { get; private set; }

    public Board(Fen fen)
    {
      myPieces = new();
      foreach (var (piece, position) in fen.Pieces)
      {
        myPieces!.Add((piece, position));
      }
      myCastlingRights = fen.Castling;
      myTurn = fen.WhosTurn;
    }

    // :TODO: Copying the entire board everytime we suggest a move is suspicious.
    // There's probably an optimisation that allows us to search without a bucnh of memory. #6
    private Board(Board original)
    {
      myPieces = new List<(Piece Piece, Square Square)>(original.myPieces);
      myCastlingRights = original.myCastlingRights;
      myTurn = original.myTurn;
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

    public bool WouldLeaveKingInCheck(Move suggestedMove, Colour movingPieceColour)
    {
      var resultingBoard = new Board(this);
      resultingBoard.ApplyMoveForCheck(suggestedMove);

      return resultingBoard.IsKingInCheck(movingPieceColour);
    }

    // Mutates the board state to simulate a move, does not change who's turn it is.
    private void ApplyMoveForCheck(Move move)
    {
      var movingPiece = PieceAtPosition(move.BeforeSquare)
          ?? throw new InvalidOperationException(
              $"No piece at {move.BeforeSquare}.");

      myPieces.RemoveAll(piece =>
          piece.Square == move.BeforeSquare ||
          piece.Square == move.AfterSquare);

      myPieces.Add((movingPiece, move.AfterSquare));
    }

    public bool IsValidPosition()
    {
      // :TODO: Make IsValidPositon ensure that the king who's turn it isn't is not in check.
      // Involves modifiying IsInCheck a bit, because it can recurse if we call it here.

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
      if (myTurn is not Colour turn)
      {
        Debug.Assert(false, "It is assumed that the board state would be valid enough for it to know whos's turn it is.");
        // :TODO: Consider throwing.
        return false;
      }
      return IsKingInCheck(turn);
    }

    public bool IsSquareAttacked(Square target, Colour byColour)
    {
      foreach (var (piece, from) in myPieces)
      {
        if (piece.Colour() != byColour)
          continue;

        foreach (var attack in piece.GenerateAttackingMoves(from, this))
        {
          if (attack.AfterSquare == target)
            return true;
        }
      }

      return false;
    }

    public bool IsKingInCheck(Colour kingColour)
    {
      var kingSquare = FindKing(kingColour);
      if (kingSquare is not Square square)
        return false;

      var attackerColour = kingColour.OtherColour();

      return IsSquareAttacked(square, attackerColour);
    }
  }

}
