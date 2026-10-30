namespace SharpShooter.Pieces
{
  public class Rook : SlidingPiece
  {
    public Rook(Colour colour)
      : base(colour)
    {
    }

    public override PieceType Type()
    {
      return Pieces.PieceType.Rook;
    }

    // :TODO: Missing test coverage.
    protected override IEnumerable<(int File, int Rank)> Directions =>
    [
        (0, -1), (0, +1),
        (+1, 0), (-1, 0)
    ];
  }
}
