using System;
using System.Collections.Generic;
using System.Text;

namespace SharpShooter
{
  public readonly record struct Move(Square BeforeSquare, Square AfterSquare)
  {
    public static implicit operator Move((Square BeforeSquare, Square AfterSquare) t) => new Move(t.BeforeSquare, t.AfterSquare);
    public static implicit operator (Square BeforeSquare, Square AfterSquare)(Move m) => (m.BeforeSquare, m.AfterSquare);
  }
}
