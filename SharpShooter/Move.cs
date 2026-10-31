namespace SharpShooter
{
  public readonly record struct Move(Square BeforeSquare, Square AfterSquare)
  {
    public static implicit operator Move((Square BeforeSquare, Square AfterSquare) t) => new Move(t.BeforeSquare, t.AfterSquare);
    public static implicit operator (Square BeforeSquare, Square AfterSquare)(Move m) => (m.BeforeSquare, m.AfterSquare);

    public static (int deltaFile, int deltaRank) Offset(Move move)
    {
      return (move.AfterSquare.File - move.BeforeSquare.File,
              move.AfterSquare.Rank - move.BeforeSquare.Rank);
    }
  }
}
