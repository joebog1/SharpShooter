using SharpShooter;

namespace Tests.EngineTests
{
  public class EngineTests
  {

    // :NOTE: These tests are specific to testing the engine when an objective result is found.
    // i.e tactics puzzles that result in massive material gain that is obviously advantageous or
    // cases where a forced mate is found. Postions that are more vague and require position
    // evaluation are handled in the PositionalKnwoledgeBenchmark project.
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

    // :TODO: Cover tactical and positional tests.

    // :TODO: A simple test where we cover every response to 1.e4 that does not prevent 2.d4.
    // The best move is always to play d4. I consider this to be objectively best so it can be a test.
  }
}
