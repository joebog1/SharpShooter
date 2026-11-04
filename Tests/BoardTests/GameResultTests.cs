using SharpShooter;

namespace Tests.BoardTests
{
  public class GameResultTests
  {
    [Theory]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", GameResult.Undecided)]
    [InlineData("r1bqkb1r/pppp1Qpp/2n2n2/4p3/2B1P3/8/PPPP1PPP/RNB1K1NR b KQkq - 0 4", GameResult.WhiteWins)]
    [InlineData("rnb1k1nr/pppp1ppp/8/2b1p3/4P3/2N2N2/PPPP1qPP/R1BQKB1R w KQkq - 0 5", GameResult.BlackWins)]
    [InlineData("8/8/8/8/8/6k1/5q2/7K w - - 0 5", GameResult.Draw)]
    [InlineData("8/8/8/8/8/6K1/5Q2/7k b - - 0 5", GameResult.Draw)]
    public void Result_StartingPosition_IsUndecided(string fen, GameResult expectedGameResult)
    {
      var board = new Board(fen);

      // The current game result should be what we expect.
      Assert.Equal(expectedGameResult, board.Result());
    }

  }
}
