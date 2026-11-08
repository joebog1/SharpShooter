using SharpShooter;

namespace Tests.EngineTests
{
  public class EngineTests
  {
    [Fact]
    public void ChessEngine_HasScolarsMate_SuggestsScholarsMate()
    {
      // Given a position where white has 1 legal move that wins the game, the engine must suggest
      // that as the top move.
      var engine = new ChessEngine("rnbqk2r/pppp1ppp/5n2/2b1p2Q/2B1P3/8/PPPP1PPP/RNB1K1NR w KQkq - 4 4");

      var bestMoves = engine.GenerateBestMoves(engine.myBoard);

      var checkMate = new Move("h5", "f7");

      Assert.Single(bestMoves);
      Assert.Equal(checkMate, bestMoves[0]);
    }
  }
}
