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

    public static implicit operator Square(string algebraic)
    {
      if (algebraic is null)
        throw new ArgumentNullException(nameof(algebraic));

      if (algebraic.Length != 2)
        throw new FormatException("A square must use algebraic notation such as \"e4\".");

      char file = char.ToLowerInvariant(algebraic[0]);
      char rank = algebraic[1];

      if (file < 'a' || file > 'h' || rank < '1' || rank > '8')
        throw new FormatException($"Invalid square notation: \"{algebraic}\".");

      return new Square(file - 'a', rank - '1');
    }
    public static implicit operator (int File, int Rank)(Square s) => (s.File, s.Rank);
  }
}
