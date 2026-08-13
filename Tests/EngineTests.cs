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
      new object[] { "4k3/8/8/8/8/8/8/4K3 w - - 0 1", 5, 0, Colour.White, SharpShooter.Type.King },
      new object[] { "4k3/8/8/8/8/8/8/4K3 w - - 0 1", 5, 7, Colour.Black, SharpShooter.Type.King },
      new object[] { "3Rk3/8/8/8/8/8/8/4K3 w - - 0 1", 4, 0, Colour.Black, SharpShooter.Type.Rook },
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
    [InlineData("4k3/8/8/8/8/8/8/4K3 w - - 0 1", false, "white is not in check")]
    [InlineData("4k3/8/8/8/8/8/8/4K3 b - - 0 1", false, "black is not in check")]
    [InlineData("R3k3/8/8/8/8/8/8/K7 w - - 0 1", true, "white is in check")]
    [InlineData("4k3/4R3/8/8/8/8/8/K7 w - - 0 1", true, "white is in check")]
    public void KingIsInCheck_ChecksInCheck_ReturnsExpected(string fen, bool expected, string _)
    {
      var engine = new ChessEngine(fen);

      bool isInCheck = engine.IsInCheck();

      Assert.Equal(expected, isInCheck);
    }
  }
}
