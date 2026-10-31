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
    public override List<Move> GenerateLegalMoves(Square startingSquare, Board boardBounds)
    {
      List<Move> pesudoLegalMoves = [];
      // :TODO: Promotions count as several valid possible moves. Include them.
      if (Colour() == Pieces.Colour.Black)
      {
        Move down = new Move(startingSquare, Square.Offset(startingSquare, 0, -1));
        AddIfValid(pesudoLegalMoves, down, boardBounds);

        Move downLeft = new Move(startingSquare, Square.Offset(startingSquare, -1, -1));
        AddIfValid(pesudoLegalMoves, downLeft, boardBounds);

        Move downRight = new Move(startingSquare, Square.Offset(startingSquare, +1, -1));
        AddIfValid(pesudoLegalMoves, downRight, boardBounds);
      }
      else
      {
        Debug.Assert(Colour() == Pieces.Colour.White);
        Move up = new Move(startingSquare, Square.Offset(startingSquare, 0, 1));
        AddIfValid(pesudoLegalMoves, up, boardBounds);

        Move upLeft = new Move(startingSquare, Square.Offset(startingSquare, -1, +1));
        AddIfValid(pesudoLegalMoves, upLeft, boardBounds);

        Move upRight = new Move(startingSquare, Square.Offset(startingSquare, +1, +1));
        AddIfValid(pesudoLegalMoves, upRight, boardBounds);
      }
      return pesudoLegalMoves;
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
      // :TODO: EN PASSANT!
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
      // Must be a diagonal move, check if a piece of opposite colour occupies the candidateMove
      // after square. 
      if (pieceAtAfterPosition is not null &&
         pieceAtAfterPosition!.Colour() != Colour())
      {
        // Then you can take it.
        return true;
      }
      // You can't take it, illegal move
      return false;
    }
  }
}
