using System.Diagnostics;

namespace SharpShooter.Pieces
{
  public class Pawn : Piece
  {
    public Pawn(Colour colour)
      : base(colour)
    {
    }
    public override PieceType Type()
    {
      return Pieces.PieceType.Pawn;
    }
    /// <summary>
    /// Generates a list of legal moves for a pawn.
    /// </summary>
    /// <param name="startingSquare"></param>
    /// <returns></returns>
    public override List<Move> GenerateLegalMoves(Square startingSquare, Board board)
    {
      List<Move> moves = [];

      int promotionRank = 0;
      if (Colour() == Pieces.Colour.White)
      {
        promotionRank = 7;
      }

      Action<Move> AddAndIncludePromotions = (move) =>
      {
        if (move.AfterSquare.Rank != promotionRank)
        {
          AddIfValid(moves, move, board);
        }
        else
        {
          // Promotions!
          Move Knight = new Move(startingSquare,
                                   move.AfterSquare,
                                   PieceType.Knight);
          AddIfValid(moves, Knight, board);
          Move Bishop = new Move(startingSquare,
                                   move.AfterSquare,
                                   PieceType.Bishop);
          AddIfValid(moves, Bishop, board);
          Move Rook = new Move(startingSquare,
                                   move.AfterSquare,
                                   PieceType.Rook);
          AddIfValid(moves, Rook, board);
          Move Queen = new Move(startingSquare,
                                   move.AfterSquare,
                                   PieceType.Queen);
          AddIfValid(moves, Queen, board);
        }
      };

      if (Colour() == Pieces.Colour.Black)
      {
        Move down = new Move(startingSquare, Square.Offset(startingSquare, 0, -1));
        AddAndIncludePromotions(down);

        Move downLeft = new Move(startingSquare, Square.Offset(startingSquare, -1, -1));
        AddAndIncludePromotions(downLeft);

        Move downRight = new Move(startingSquare, Square.Offset(startingSquare, +1, -1));
        AddAndIncludePromotions(downRight);

        if (board.IsSquareOnPawnStartingFile(startingSquare, Colour()))
        {
          Move downJump = new Move(startingSquare, Square.Offset(startingSquare, 0, -2));
          AddIfValid(moves, downJump, board);
        }
      }
      else
      {
        Debug.Assert(Colour() == Pieces.Colour.White);
        Move up = new Move(startingSquare, Square.Offset(startingSquare, 0, 1));
        AddAndIncludePromotions(up);

        Move upLeft = new Move(startingSquare, Square.Offset(startingSquare, -1, +1));
        AddAndIncludePromotions(upLeft);

        Move upRight = new Move(startingSquare, Square.Offset(startingSquare, +1, +1));
        AddAndIncludePromotions(upRight);

        if (board.IsSquareOnPawnStartingFile(startingSquare, Colour()))
        {
          Move upJump = new Move(startingSquare, Square.Offset(startingSquare, 0, 2));
          AddIfValid(moves, upJump, board);
        }
      }
      return moves;
    }

    /// <summary>
    /// Generates a list of attacked squares for a pawn at a position.
    /// for a pawn, 'attacking' is only on the diagonal.
    /// </summary>
    /// <param name="startingSquare"></param>
    /// <returns></returns>
    public override List<Move> GenerateAttackingMoves(Square startingSquare, Board board)
    {
      List<Move> attackingMoves = [];
      // :TODO: Promotions count as several valid possible moves. Include them.
      if (Colour() == Pieces.Colour.Black)
      {
        Move downLeft = new Move(startingSquare, Square.Offset(startingSquare, -1, -1));
        AddIfOnBoard(attackingMoves, downLeft, board);

        Move downRight = new Move(startingSquare, Square.Offset(startingSquare, +1, -1));
        AddIfOnBoard(attackingMoves, downRight, board);
      }
      else
      {
        Move upLeft = new Move(startingSquare, Square.Offset(startingSquare, -1, +1));
        AddIfOnBoard(attackingMoves, upLeft, board);

        Move upRight = new Move(startingSquare, Square.Offset(startingSquare, +1, +1));
        AddIfOnBoard(attackingMoves, upRight, board);
      }
      return attackingMoves;
    }

    /// <summary>
    /// Checks if candidate move is a legal pawn move given the board state.
    /// It cares about the specific rules regarding pawn captures as well as en 
    /// passent.
    /// </summary>
    /// <param name="moves"></param>
    /// <param name="candidateMove"></param>
    /// <param name="board"></param>
    public override bool PieceSpecificChecks(Move candidateMove, Board board)
    {
      // A pawn needs to know if it can take diagonally because it can't move diagonally otherwise.
      var pieceAtAfterPosition = board.PieceAtPosition(candidateMove.AfterSquare);
      // First check if the move is strictly vertical.
      if (candidateMove.BeforeSquare.File == candidateMove.AfterSquare.File)
      {
        // Then we need only to check if anything is in the way. Pawns can't capture forward
        if (board.IsOnBoard(candidateMove.AfterSquare) &&
           pieceAtAfterPosition == null)
        {
          // This is valid.
          return true;
        }
        // Something blocks the pawn, it can't move!.
        return false;
      }

      // Could be a diagonal move, check if a piece of opposite colour occupies the candidateMove
      // after square. :NOTE: If the candidateMove after square is the en passant square,
      // then there doesn't need to be a null piece after position.
      if (candidateMove.AfterSquare == board.myEnPassantSquare)
      {
        // Because the previous move must have been a pawn jump, the en passant square must be empty.
        Debug.Assert(pieceAtAfterPosition == null);
        return true;
      }
      if (pieceAtAfterPosition is not null &&
         pieceAtAfterPosition!.Colour() != Colour())
      {
        // Then you can take it.
        return true;
      }

      // If the move lands on promotion rank of the pawn for that colour, then the move must contain a non-null,
      // non king and non-pawn promotion as part of the move.
      // :TODO: This violates proper responsibility assignment, the board knows if the pawn is promoting,
      // not the pawn!
      int promotionRank = 0;
      if (Colour() == Pieces.Colour.White)
      {
        promotionRank = 7;
      }
      if (candidateMove.AfterSquare.Rank == promotionRank &&
        (candidateMove.Promotion == null ||
        candidateMove.Promotion == PieceType.King ||
        candidateMove.Promotion == PieceType.Pawn
        ))
      {
        // :TODO: This lacks test coverage because we assume the board is trusted enough not to suggest
        // nonsense moves.
        Debug.Assert(false);
        return false;
      }

      // You can't take it, illegal move
      return false;
    }
  }
}
