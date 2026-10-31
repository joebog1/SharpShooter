using System.Diagnostics;

namespace SharpShooter.Pieces
{
  public class King : Piece
  {
    public King(Colour colour)
    : base(colour)
    {
    }

    public override PieceType Type()
    {
      return Pieces.PieceType.King;
    }


    private List<Move> GenerateMoves(Square startingSquare,
                                     Board board,
                                     Action<List<Move>, Move, Board> addMove)
    {
      List<Move> legalMoves = [];
      addMove(legalMoves, new Move(startingSquare, Square.Offset(startingSquare, 0, 1)), board);
      addMove(legalMoves, new Move(startingSquare, Square.Offset(startingSquare, 1, 0)), board);
      addMove(legalMoves, new Move(startingSquare, Square.Offset(startingSquare, 1, 1)), board);
      addMove(legalMoves, new Move(startingSquare, Square.Offset(startingSquare, 0, -1)), board);
      addMove(legalMoves, new Move(startingSquare, Square.Offset(startingSquare, -1, 0)), board);
      addMove(legalMoves, new Move(startingSquare, Square.Offset(startingSquare, 1, -1)), board);
      addMove(legalMoves, new Move(startingSquare, Square.Offset(startingSquare, -1, 1)), board);
      addMove(legalMoves, new Move(startingSquare, Square.Offset(startingSquare, -1, -1)), board);

      // Ask the board if castling is available to us. If it is, then provided nothing is in the
      // way, allow for castling
      if (board.CanCastleKingside(Colour()))
      {
        // It is the board's responsibility to recognise that this move actually moves more than one peice.
        addMove(legalMoves, new Move(startingSquare, Square.Offset(startingSquare, 2, 0)), board);
      }
      if (board.CanCastleQueenside(Colour()))
      {
        // It is the board's responsibility to recognise that this move actually moves more than one peice.
        addMove(legalMoves, new Move(startingSquare, Square.Offset(startingSquare, -2, 0)), board);
      }
      return legalMoves;
    }

    /// <summary>
    /// Generates a list of legal moves for a king.
    /// </summary>
    /// <param name="startingSquare"></param>
    /// <returns></returns>
    public override List<Move> GenerateLegalMoves(Square startingSquare, Board board)
    {
      return GenerateMoves(startingSquare, board, AddIfValid);
    }

    public override List<Move> GenerateAttackingMoves(Square startingSquare,
                                                       Board board)
    {
      return GenerateMoves(startingSquare, board, AddIfOnBoard);
    }

    public override bool PieceSpecificChecks(Move candidateMove, Board board)
    {
      // First check if the move suggested is a castle.
      var kingMoveOffset = Move.Offset(candidateMove);
      // :TODO: This can be simplifed.
      if (kingMoveOffset.deltaFile == 2)
      {
        // AddIfValid is already responsible for ensureing that the square the
        // kinglands on is occupied by another piece, we need only check the interim square for
        // a piece blocking or an opponents piece could theoretically get there (Ignoring pins!)
        Debug.Assert(board.CanCastleKingside(Colour()));

        // Firstly, if the king is in check, he can't castle.
        if (board.IsKingInCheck(Colour()))
          return false;

        // Secondly, if there is anything in the way, you can't castle.
        var interimSquare = Square.Offset(candidateMove.BeforeSquare, 1, 0);
        if (board.PieceAtPosition(interimSquare) != null)
          return false;

        // The square where the king will end up
        var finalSquare = Square.Offset(candidateMove.BeforeSquare, 2, 0);
        if (board.PieceAtPosition(finalSquare) != null)
          return false;

        // :NOTE: We chould check the edge, but we shouldn't need to, the board has allowed castling
        // rights so the edge should always be a rook.
        Debug.Assert(board.PieceAtPosition(Square.Offset(candidateMove.BeforeSquare, 3, 0))?.Type() == PieceType.Rook);

        // Finally, we check if either the interim position or the final position has at least one attacker of
        // opposing colour in it.
        if (board.IsSquareAttacked(interimSquare,
                               Colour().OtherColour()))
          return false;

        if (board.IsSquareAttacked(finalSquare,
                               Colour().OtherColour())) ;
        return false;

      }
      else if (kingMoveOffset.deltaFile == -2)
      {
        // AddIfValid is already responsible for ensureing that the square the
        // kinglands on is occupied by another piece, we need only check the interim square for
        // a piece blocking or an opponents piece could theoretically get there (Ignoring pins!)
        Debug.Assert(board.CanCastleQueenside(Colour()));

        // Firstly, if the king is in check, he can't castle.
        if (board.IsKingInCheck(Colour()))
          return false;

        // :NOTE: We chould check the edge, but we shouldn't need to, the board has allowed castling
        // rights so the edge should always be a rook.
        Debug.Assert(board.PieceAtPosition(Square.Offset(candidateMove.BeforeSquare, -4, 0))?.Type() == PieceType.Rook);


        // Secondly, if there is anything in the way, you can't castle.
        var interimSquare = Square.Offset(candidateMove.BeforeSquare, -1, 0);
        if (board.PieceAtPosition(interimSquare) != null)
          return false;

        var finalSquare = Square.Offset(candidateMove.BeforeSquare, -2, 0);

        if (board.PieceAtPosition(finalSquare) != null)
          return false;

        // Finally, we check if either the interim position or the final position has at least one attacker of
        // opposing colour in it.
        if (board.IsSquareAttacked(interimSquare,
                               Colour().OtherColour()))
          return false;

        if (board.IsSquareAttacked(finalSquare,
                               Colour().OtherColour())) ;
        return false;
      }
      // It's not castles, no additional peice specific checks are needed.
      return true;
    }

  }
}
