using SharpShooter;

namespace Tests
{
  public class EngineTests
  {
    [Fact]
    public void EmptyBoard_ChecksValidity_IsInvalid()
    {
      // Arrange an empty board is invalid because kings are required to exist.
      var engine = new ChessEngine();
      string emptyBoardFen = "8/8/8/8/8/8/8/8 w - - 0 1";
      engine.SetPositionByFen(emptyBoardFen);

      // Act by checking if the position we are in is valid.
      bool isValid = engine.IsValidPosition();

      // Assert that the position is invalid.
      Assert.False(isValid);
    }
    [Theory]
    [InlineData("4k3/8/8/8/8/8/8/4K3 w - - 0 1", true, "Two kings far apart is valid")]
    [InlineData("3Kk3/8/8/8/8/8/8/8 w - - 0 1", false, "Kings adjacent on same row is invalid")]
    [InlineData("4K3/3k4/8/8/8/8/8/8 w - - 0 1", false, "Kings touching diagonally is invalid")]
    [InlineData("8/8/8/8/8/8/3k4/3K4 w - - 0 1", false, "Kings adjacent on same column is invalid")]
    [InlineData("4k3/8/8/8/8/8/8/3PK3 w - - 0 1", false, "Pawns can't exist on the first rank")]
    public void KingProximity_ChecksValidity_ReturnsExpected(string fen, bool expected, string _)
    {
      var engine = new ChessEngine();
      engine.SetPositionByFen(fen);

      bool isValid = engine.IsValidPosition();

      Assert.Equal(expected, isValid);
    }
  }
}
