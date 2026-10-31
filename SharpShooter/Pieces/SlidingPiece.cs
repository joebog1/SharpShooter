namespace SharpShooter.Pieces
{
  /// <summary>
  /// Represents a piece which slides across the board (Rook,Bishop and Queen)
  /// </summary>
  public abstract class SlidingPiece : Piece
  {
    protected abstract IEnumerable<(int File, int Rank)> Directions { get; }

    protected SlidingPiece(Colour colour) : base(colour)
    {
    }

    /// <summary>
    /// Generates the moves for a sliding peice on a starting square.
    /// addFunctor allows the caller to decide if it'll add a candidate move or not.
    /// </summary>
    /// <param name="startingSquare"></param>
    /// <param name="board"></param>
    /// <param name="addFunctor"></param>
    /// <returns></returns>
    private List<Move> GenerateMoves(Square startingSquare,
                                     Board board,
                                     Action<List<Move>, Move, Board> addMove)
    {
      var moves = new List<Move>();

      foreach (var (fileDelta, rankDelta) in Directions)
      {
        Square candidateSquare = startingSquare;
        while (true)
        {
          candidateSquare = Square.Offset(candidateSquare, fileDelta, rankDelta);

          if (!board.IsOnBoard(candidateSquare))
            // Stop searching in this direction.
            break;

          var occupant = board.PieceAtPosition(candidateSquare);

          if (occupant is null)
          {
            addMove(moves, (startingSquare, candidateSquare), board);
            continue;
          }

          if (occupant.Colour() != Colour())
            // Capture the first opposing piece.
            addMove(moves, (startingSquare, candidateSquare), board);

          break; // Rest of the slide is blocked
        }
      }

      return moves;
    }

    public override List<Move> GenerateLegalMoves(Square startingSquare,
                                                  Board board)
    {
      return GenerateMoves(startingSquare, board, AddIfValid);
    }

    public override List<Move> GenerateAttackingMoves(Square startingSquare,
                                                       Board board)
    {
      return GenerateMoves(startingSquare, board, AddIfOnBoard);
    }

  }
}
