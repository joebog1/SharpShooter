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

    /// <summary>
    /// Generates a list of pesudo legal moves for a king. 
    /// Guarenteees that every suggested move is on the board, nothing else.
    /// </summary>
    /// <param name="startingSquare"></param>
    /// <returns></returns>
    public override List<Move> GenerateLegalMoves(Square startingSquare, Board board)
    {
      List<Move> legalMoves = [];
      AddIfValid(legalMoves, new Move(startingSquare, Square.Offset(startingSquare, 0, 1)), board);
      AddIfValid(legalMoves, new Move(startingSquare, Square.Offset(startingSquare, 1, 0)), board);
      AddIfValid(legalMoves, new Move(startingSquare, Square.Offset(startingSquare, 1, 1)), board);
      AddIfValid(legalMoves, new Move(startingSquare, Square.Offset(startingSquare, 0, -1)), board);
      AddIfValid(legalMoves, new Move(startingSquare, Square.Offset(startingSquare, -1, 0)), board);
      AddIfValid(legalMoves, new Move(startingSquare, Square.Offset(startingSquare, 1, -1)), board);
      AddIfValid(legalMoves, new Move(startingSquare, Square.Offset(startingSquare, -1, 1)), board);
      AddIfValid(legalMoves, new Move(startingSquare, Square.Offset(startingSquare, -1, -1)), board);

      // Ask the board if castling is available to us. If it is, then provided nothing is in the
      // way, allow for castling
      if(board.CanCastleKingside(Colour()))
      {
        // It is the board's responsibility to recognise that this move actually moves more than one peice.
        AddIfValid(legalMoves, new Move(startingSquare, Square.Offset(startingSquare, 2, 0)), board);
      }
      if(board.CanCastleQueenside(Colour()))
      {
        // It is the board's responsibility to recognise that this move actually moves more than one peice.
        AddIfValid(legalMoves, new Move(startingSquare, Square.Offset(startingSquare, -2, 0)), board);
      }
      return legalMoves;
    }
  }
}
