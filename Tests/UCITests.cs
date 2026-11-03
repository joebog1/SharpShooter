using SharpShooter;
using SharpShooter.UCI;

namespace Tests
{
  public class UCITests
  {
    [Fact]
    public void Parser_AsksIsReady_returnsReadyOk()
    {
      // Send the isready command.
      Assert.Equal("readyok",UciCommandParser.ParseUciCommand("isready"));
    }

    [Fact]
    public void Parser_AsksForId_GivesId()
    {
      // Send the id command.
      Assert.Equal("id name SharpShooter\nid author Joseph Bruanto\nuciok", UciCommandParser.ParseUciCommand("id"));
    }
  }
}
