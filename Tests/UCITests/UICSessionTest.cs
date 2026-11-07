using SharpShooter;
using SharpShooter.UCI;

namespace Tests.UCITests
{
  public class UICSessionTest
  {
    [Fact]
    public void PositionCommand_SetsPositionCorrectly()
    {
      string fen = "r1bqkbnr/pppp1ppp/2n5/4p3/3PP3/5N2/PPP2PPP/RNBQKB1R b KQkq - 0 3";
      var command = new UCICommand(UCICommandType.Position, fen);

      var session = new UciSession();

      session.Execute(command);

      // Ask the session for the board state, it should be the starting position.
      var exepctedBoard = new Board(fen);
      Assert.Equal(exepctedBoard, session.GetBoard());
    }
  }
}
