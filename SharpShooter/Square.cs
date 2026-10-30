namespace SharpShooter
{
  public readonly record struct Square(int File, int Rank)
  {
    // Returns a new Square object that is offset by the rank and file provided.
    public static Square Offset(Square existingSquare, int file, int rank)
    {
      return new Square(existingSquare.File + file, existingSquare.Rank + rank);
    }
    public static implicit operator Square((int File, int Rank) t) => new Square(t.File, t.Rank);
    public static implicit operator (int File, int Rank)(Square s) => (s.File, s.Rank);
  }
}
