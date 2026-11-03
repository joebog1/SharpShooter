using SharpShooter;
using SharpShooter.UCI;

namespace Tests
{
  public class UCITests
  {
    [Fact]
    public void Engine_AsksIsReady_returnsReadyOk()
    {
      // Send the isready command.
      Assert.Equal("readyok",UciCommandParser.ParseUciCommand("isready"));
    }
  }
}
