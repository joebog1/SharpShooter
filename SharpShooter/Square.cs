namespace SharpShooter
{
  public readonly record struct Square(int File, int Rank)
  {
    public static implicit operator Square((int File, int Rank) t) => new Square(t.File, t.Rank);
    public static implicit operator (int File, int Rank)(Square s) => (s.File, s.Rank);
  }
}
