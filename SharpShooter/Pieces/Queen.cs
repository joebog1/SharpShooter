using System;
using System.Collections.Generic;
using System.Text;

namespace SharpShooter.Pieces
{
  public class Queen : SlidingPiece
  {
    public Queen(Colour colour)
      : base(colour)
    {
    }

    public override PieceType Type()
    {
      return Pieces.PieceType.Queen;
    }

    // :TODO: missing test coverage.
    protected override IEnumerable<(int File, int Rank)> Directions =>
    [
        (-1, -1), (-1, +1),
        (+1, -1), (+1, +1),
        (0, -1), (0, +1),
        (+1, 0), (-1, 0)
    ];

  }
}
