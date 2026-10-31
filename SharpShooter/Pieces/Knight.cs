namespace SharpShooter.Pieces
{
  public class Knight : Piece
  {
    public Knight(Colour colour)
      : base(colour)
    {
    }

    public override PieceType Type()
    {
      return Pieces.PieceType.Knight;
    }

    /// <summary>
    /// Generates a list of legal moves for a Knight. 
    /// </summary>
    /// <param name="startingSquare"></param>
    /// <returns></returns>
    private List<Move> GenerateMoves(Square startingSquare,
                                     Board board,
                                     Action<List<Move>, Move, Board> addMove)
    {
      var possibleOffsets = new (int, int)[] { (+1, +2), (+2, +1), (+2, -1), (+1, -2), (-1, -2), (-2, -1), (-2, +1), (-1, +2) };

      List<Move> legalMoves = [];

      foreach (var offset in possibleOffsets)
      {
        addMove(legalMoves,
                   new Move(startingSquare,
                            Square.Offset(startingSquare,
                                          offset.Item1,
                                          offset.Item2)),
                   board);
      }

      return legalMoves;
    }

    /// <summary>
    /// Generates a list of legal moves for a Knight. 
    /// </summary>
    /// <param name="startingSquare"></param>
    /// <returns></returns>
    public override List<Move> GenerateLegalMoves(Square startingSquare, Board board)
    {
      return GenerateMoves(startingSquare, board, AddIfValid);
    }

    /// <summary>
    /// Generates a list of attacking moves for a Knight. Ignores things like pins. 
    /// </summary>
    /// <param name="startingSquare"></param>
    /// <returns></returns>
    public override List<Move> GenerateAttackingMoves(Square startingSquare, Board board)
    {
      return GenerateMoves(startingSquare, board, AddIfOnBoard);
    }
  }
}
