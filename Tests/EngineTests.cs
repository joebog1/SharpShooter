using SharpShooter;
using System.Drawing;

namespace Tests
{
  public class EngineTests
  {
    [Fact]
    public void EmptyBoard_ChecksValidity_IsInvalid()
    {
      // Arrange an empty board is invalid because kings are required to exist.
      var engine = new ChessEngine("8/8/8/8/8/8/8/8 w - - 0 1");

      // Act by checking if the position we are in is valid.
      bool isValid = engine.IsValidPosition();

      // Assert that the position is invalid.
      Assert.False(isValid);
    }
    [Theory]
    [InlineData("8/8/8/4k3/8/2K5/8/5P2 w - - 0 1")]
    [InlineData("8/8/8/4k3/8/2K5/8/5P2 b - - 0 1")]
    [InlineData("3P4/8/8/4k3/8/2K5/8/8 w - - 0 1")]
    [InlineData("3P4/8/8/4k3/8/2K5/8/8 b - - 0 1")]
    [InlineData("3p4/8/8/4k3/8/2K5/8/8 w - - 0 1")]
    [InlineData("3p4/8/8/4k3/8/2K5/8/8 b - - 0 1")]
    [InlineData("8/8/8/4k3/8/2K5/8/p7 w - - 0 1")]
    [InlineData("8/8/8/4k3/8/2K5/8/p7 b - - 0 1")]
    public void PawnsOnFirstRank_ChecksValidity_IsInvalid(string fen)
    {
      // Arrange an empty board is invalid because kings are required to exist.
      var engine = new ChessEngine(fen);

      // Act by checking if the position we are in is valid.
      bool isValid = engine.IsValidPosition();

      // Assert that the position is invalid.
      Assert.False(isValid);
    }

    [Theory]
    [InlineData("4k3/8/8/8/8/8/8/4K3 w - - 0 1", true, "Two kings far apart is valid")]
    [InlineData("3Kk3/8/8/8/8/8/8/8 w - - 0 1", false, "Kings adjacent on same row is invalid")]
    [InlineData("3K3/8/8/8/3k4/8/8/3K4 w - - 0 1", false, "needs to be exactly 1 of each king")]
    [InlineData("3K3/8/8/8/8/8/8/8 w - - 0 1", false, "needs to be exactly 1 of each king")]
    [InlineData("4K3/3k4/8/8/8/8/8/8 w - - 0 1", false, "Kings touching diagonally is invalid")]
    [InlineData("8/8/8/8/8/8/3k4/3K4 w - - 0 1", false, "Kings adjacent on same column is invalid")]
    [InlineData("4k3/8/8/8/8/8/8/3PK3 w - - 0 1", false, "Pawns can't exist on the first rank")]
    [InlineData("4k3/8/8/8/8/8/8/3pK3 w - - 0 1", false, "Pawns can't exist on the first rank")]
    [InlineData("4Pk3/8/8/8/8/8/8/3K3 w - - 0 1", false, "Pawns can't exist on the first rank")]
    [InlineData("4pk3/8/8/8/8/8/8/3K3 w - - 0 1", false, "Pawns can't exist on the first rank")]
    public void KingProximity_ChecksValidity_ReturnsExpected(string fen, bool expected, string _)
    {
      var engine = new ChessEngine(fen);

      bool isValid = engine.IsValidPosition();

      Assert.Equal(expected, isValid);
    }

    public static IEnumerable<object[]> PieceTestData => new[]
    {
      new object[] { "4k3/8/8/8/8/8/8/4K3 w - - 0 1", 4, 0, Colour.White, SharpShooter.Type.King },
      new object[] { "4k3/8/8/8/8/8/8/4K3 w - - 0 1", 4, 7, Colour.Black, SharpShooter.Type.King },
      new object[] { "3Rk3/8/8/8/8/8/8/4K3 b - - 0 1", 3, 7, Colour.White, SharpShooter.Type.Rook },
      new object[] { "k7/8/8/8/8/8/8/4K3 w - - 0 1", 0, 7, Colour.Black, SharpShooter.Type.King },
      new object[] { "7k/8/8/8/8/8/8/4K3 w - - 0 1", 7, 7, Colour.Black, SharpShooter.Type.King },
      new object[] { "7k/8/8/8/8/8/8/7K w - - 0 1", 7, 0, Colour.White, SharpShooter.Type.King },
      new object[] { "7k/8/8/8/8/8/7K/8 w - - 0 1", 7, 1, Colour.White, SharpShooter.Type.King },
      new object[] { "7k/8/8/8/8/7K/8/8 w - - 0 1", 7, 2, Colour.White, SharpShooter.Type.King },
      new object[] { "7k/8/8/8/7K/8/8/8 w - - 0 1", 7, 3, Colour.White, SharpShooter.Type.King },
      new object[] { "7k/8/8/7K/8/8/8/8 w - - 0 1", 7, 4, Colour.White, SharpShooter.Type.King },
      new object[] { "7k/8/7K/8/8/8/8/8 w - - 0 1", 7, 5, Colour.White, SharpShooter.Type.King },
      new object[] { "7k/7K/8/8/8/8/8/8 w - - 0 1", 7, 6, Colour.White, SharpShooter.Type.King },
    };

    [Theory]
    [MemberData(nameof(PieceTestData))]
    public void PieceAtPosition_ChecksPosition_IsExpectedPiece(string fen, int file, int rank, Colour expectedColour, SharpShooter.Type expectedType)
    {
      // Arrange the board.
      var engine = new ChessEngine(fen);
      // Get the piece at the expected position.
      var actualPiece = engine.PieceAtPosition((File: file, Rank: rank));
      // Assert it is the expected piece.
      var expectedPiece = new Piece(expectedColour, expectedType);
      Assert.Equal(expectedPiece, actualPiece);
    }

    [Theory]
    [InlineData("8/8/2k5/8/8/8/8/4K3 w Q - 0 1")]
    [InlineData("8/8/2k5/8/8/8/8/4K3 w K - 0 1")]
    public void IllegalCastlingRights_IsValidPosition_IsInvalid(string fen)
    {
      var engine = new ChessEngine(fen);

      bool actual = engine.IsValidPosition();

      Assert.False(actual);
    }

    [Theory]
    [InlineData("4k2r/8/8/8/8/8/8/4K2R w kK - 0 1")]
    public void LegalCastlingRights_IsValidPosition_IsValid(string fen)
    {
      var engine = new ChessEngine(fen);

      bool actual = engine.IsValidPosition();

      Assert.True(actual);
    }

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
    [InlineData("4K3/5b3/8/8/8/8/8/k7 w - - 0 1", true, "white is in check from a bishop")]
    [InlineData("4k3/3B5/8/8/8/8/8/K7 b - - 0 1", true, "black is in check from a bishop")]
    [InlineData("5b2/4K3/8/8/8/8/8/k7 w - - 0 1", true, "white is in check from a bishop")]
    [InlineData("3B5/4k3/8/8/8/8/8/K7 b - - 0 1", true, "black is in check from a bishop")]
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
    [InlineData("4k3/3Q5/8/8/8/8/8/K7 b - - 0 1", true, "black is in check from a queen")]
    [InlineData("5q2/4K3/8/8/8/8/8/k7 w - - 0 1", true, "white is in check from a queen")]
    [InlineData("3Q5/4k3/8/8/8/8/8/K7 b - - 0 1", true, "black is in check from a queen")]
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
      var engine = new ChessEngine(fen);

      bool isInCheck = engine.IsInCheck();

      Assert.Equal(expected, isInCheck);
    }
  }
}
