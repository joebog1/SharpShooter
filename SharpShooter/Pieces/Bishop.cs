using System;
using System.Collections.Generic;
using System.Text;

namespace SharpShooter.Pieces
{
  public class Bishop : SlidingPiece
  {
    public Bishop(Colour colour)
      : base(colour)
    {
    }

    public override PieceType Type()
    {
      return Pieces.PieceType.Bishop;
    }

    protected override IEnumerable<(int File, int Rank)> Directions =>
    [
        (-1, -1), (-1, +1),
            (+1, -1), (+1, +1)
    ];
  }
}
