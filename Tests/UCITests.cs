using SharpShooter;
using SharpShooter.UCI;

namespace Tests
{
  public class UCITests
  {
    [Fact]
    public void Parser_AsksIsReady_returnsReadyOk()
    {
      UciCommandParser parser = new UciCommandParser();

      // Send the isready command.
      Assert.Equal("readyok", parser.ParseUciCommand("isready"));
    }

    [Fact]
    public void Parser_AsksForUci_GivesId()
    {
      UciCommandParser parser = new UciCommandParser();

      // Send the uci command.
      Assert.Equal("id name SharpShooter\nid author Joseph Bruanto\nuciok", parser.ParseUciCommand("uci"));
    }

    [Fact]
    public void Parser_SetsPosition_PositionIsSet()
    {
      UciCommandParser parser = new UciCommandParser();

      // Set the position to the starting position with the fen for it.
      parser.ParseUciCommand("position rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
    }
  }
}
