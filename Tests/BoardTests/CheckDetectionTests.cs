using SharpShooter;

namespace Tests.BoardTests
{
  public class CheckDetectionTests
  {
    [Theory]
    [InlineData("4k3/8/8/8/8/8/8/4K3 w - - 0 1", false, "white is not in check")]
    [InlineData("4k3/8/8/8/8/8/8/4K3 b - - 0 1", false, "black is not in check")]
    // Rook check tests
    [InlineData("r3K3/8/8/8/8/8/8/k7 w - - 0 1", true, "white is in check")]
    [InlineData("4k3/4R3/8/8/8/8/8/K7 b - - 0 1", true, "black is in check from a rook")]
    [InlineData("4k3/8/4R3/8/8/8/8/K7 b - - 0 1", true, "black is in check from a rook")]
    [InlineData("4k3/8/8/8/8/8/K7/4R3 b - - 0 1", true, "black is in check from a rook")]
    [InlineData("4k3/5R3/8/8/8/8/8/K7 w - - 0 1", false, "white is not in check")]
    [InlineData("4K3/4b3/8/8/8/8/8/k7 w - - 0 1", false, "white is not in check")]
    // Rook blocked due to obstruction tests.
    [InlineData("8/8/8/8/8/8/8/3RK2k b - - 20 11", false, "black is not in check")]
    [InlineData("8/8/8/8/3k4/8/3K4/3R4 b - - 20 11", false, "black is not in check")]
    [InlineData("8/8/8/8/8/8/8/k1KR4 b - - 20 11", false, "black is not in check")]
    [InlineData("3R4/3K4/8/8/8/8/8/3k4 b - - 20 11", false, "black is not in check")]

    // Bishop diagnonal close tests.
    [InlineData("4K3/5b2/8/8/8/8/8/k7 w - - 0 1", true, "white is in check from a bishop")]
    [InlineData("4k3/3B4/8/8/8/8/8/K7 b - - 0 1", true, "black is in check from a bishop")]
    [InlineData("5b2/4K3/8/8/8/8/8/k7 w - - 0 1", true, "white is in check from a bishop")]
    [InlineData("3B4/4k3/8/8/8/8/8/K7 b - - 0 1", true, "black is in check from a bishop")]
    // Bishop diagonal checks from afar.
    [InlineData("b7/8/8/8/2k5/8/8/7K w - - 0 1", true, "white is in check from a bishop")]
    [InlineData("7B/1K6/8/8/8/8/8/k7 b - - 0 1", true, "black is in check from a bishop")]
    [InlineData("k7/8/8/8/2K5/8/8/7B b - - 0 1", true, "black is in check from a bishop")]
    [InlineData("7k/8/8/8/2K5/8/8/B7 b - - 0 1", true, "black is in check from a bishop")]
    // Bishop blocked due to obstruction tests
    [InlineData("8/8/8/8/3k4/8/5K2/6B1 b - - 20 11", false, "black is not in check")]
    [InlineData("8/8/8/8/3k4/8/1R3K2/B7 b - - 20 11", false, "black is not in check")]
    [InlineData("8/B7/1N6/8/3k4/8/1R3K2/8 b - - 20 11", false, "black is not in check")]
    [InlineData("8/6B1/1N3P2/8/3k4/8/1R3K2/8 b - - 20 11", false, "black is not in check")]

    // Queen gets given all bishop and rook checks (expect checks about not being in check.
    [InlineData("4K3/5q3/8/8/8/8/8/k7 w - - 0 1", true, "white is in check from a queen")]
    [InlineData("4k3/3Q4/8/8/8/8/8/K7 b - - 0 1", true, "black is in check from a queen")]
    [InlineData("5q2/4K3/8/8/8/8/8/k7 w - - 0 1", true, "white is in check from a queen")]
    [InlineData("3Q4/4k3/8/8/8/8/8/K7 b - - 0 1", true, "black is in check from a queen")]
    // Queen diagonal checks from afar.
    [InlineData("q7/8/8/8/2k5/8/8/7K w - - 0 1", true, "white is in check from a queen")]
    [InlineData("7Q/1K6/8/8/8/8/8/k7 b - - 0 1", true, "black is in check from a queen")]
    [InlineData("k7/8/8/8/2K5/8/8/7Q b - - 0 1", true, "black is in check from a queen")]
    [InlineData("7k/8/8/8/2K5/8/8/Q7 b - - 0 1", true, "black is in check from a queen")]
    [InlineData("q3K3/8/8/8/8/8/8/k7 w - - 0 1", true, "white is in check")]
    [InlineData("4k3/4Q3/8/8/8/8/8/K7 b - - 0 1", true, "black is in check from a queen")]
    [InlineData("4k3/8/4Q3/8/8/8/8/K7 b - - 0 1", true, "black is in check from a queen")]
    [InlineData("4k3/8/8/8/8/8/K7/4Q3 b - - 0 1", true, "black is in check from a queen")]
    // Queen blocked due to obstruction tests
    [InlineData("8/8/8/8/3k4/8/5N2/1K4Q1 b - - 20 11", false, "black is not in check")]
    [InlineData("8/8/8/8/3k4/8/3N4/1K1Q4 b - - 20 11", false, "black is not in check")]
    [InlineData("8/8/8/8/3k4/8/1N6/QK6 b - - 20 11", false, "black is not in check")]
    [InlineData("8/8/8/8/QN1k4/8/8/1K6 b - - 20 11", false, "black is not in check")]
    [InlineData("Q7/1N6/8/8/3k4/8/8/1K6 b - - 20 11", false, "black is not in check")]
    [InlineData("3Q4/3N4/8/8/3k4/8/8/1K6 b - - 20 11", false, "black is not in check")]
    [InlineData("8/8/8/8/3k1NQ1/8/8/1K6 b - - 20 11", false, "black is not in check")]

    // Knight checks the king in the L shape.
    [InlineData("8/1k6/8/4n3/8/3K4/8/8 w - - 0 1", true, "white is in check from a knight")]
    [InlineData("8/1k6/8/4n3/2K5/8/8/8 w - - 0 1", true, "white is in check from a knight")]
    [InlineData("8/8/2K5/4n3/8/8/8/k7 w - - 0 1", true, "white is in check from a knight")]
    [InlineData("8/3K4/8/4n3/8/8/8/k7 w - - 0 1", true, "white is in check from a knight")]
    [InlineData("8/5K2/8/4n3/8/8/8/k7 w - - 0 1", true, "white is in check from a knight")]
    [InlineData("8/8/6K1/4n3/8/8/8/k7 w - - 0 1", true, "white is in check from a knight")]
    [InlineData("8/8/8/4n3/6K1/8/8/k7 w - - 0 1", true, "white is in check from a knight")]
    [InlineData("8/8/8/4n3/8/5K2/8/k7 w - - 0 1", true, "white is in check from a knight")]

    // Black Pawn check tests.
    [InlineData("8/8/5p2/4K3/8/8/8/k7 w - - 0 1", true, "white is in check from a pawn")]
    [InlineData("8/8/3p4/4K3/8/8/8/k7 w - - 0 1", true, "white is in check from a pawn")]
    [InlineData("8/8/8/4K3/3p4/8/8/k7 w - - 0 1", false, "white is not in check")]
    [InlineData("8/8/8/4K3/5p2/8/8/k7 w - - 0 1", false, "white is not in check")]
    // White pawn check tests.
    [InlineData("8/8/8/4k3/5P2/8/4K3/8 b - - 0 1", true, "black is in check from a pawn")]
    [InlineData("8/8/8/4k3/3P4/8/4K3/8 b - - 0 1", true, "black is in check from a pawn")]
    [InlineData("8/8/3P4/4k3/8/8/4K3/8 b - - 0 1", false, "black is not in check")]
    [InlineData("8/8/5P2/4k3/8/8/4K3/8 b - - 0 1", false, "black is not in check")]
    // Ensure checks don't go out of bounds tests.
    [InlineData("8/8/5P2/4K3/8/8/8/7k w - - 7 5", false, "white is not in check")]
    [InlineData("K7/8/5P2/8/8/8/8/7k b - - 20 11", false, "black is not in check")]
    public void KingIsInCheck_ChecksInCheck_ReturnsExpected(string fen, bool expected, string _)
    {
      var board = new Board(fen);

      bool isInCheck = board.IsInCheck();

      Assert.Equal(expected, isInCheck);
    }

  }
}
