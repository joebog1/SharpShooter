using SharpShooter;
using SharpShooter.UCI;

namespace Tests
{
  public class UCITests
  {
    public static TheoryData<string, UCICommand> ParserCases => new()
    {
      { "isready", new UCICommand(UCICommandType.IsReady) },
      { "uci",     new UCICommand(UCICommandType.UCI) },
      { "quit",    new UCICommand(UCICommandType.Quit) }
    };

    [Theory]
    [MemberData(nameof(ParserCases))]
    public void Parser_GivenInput_ReturnsExpectedCommand(
      string input,
      UCICommand expectedCommand)
    {
      var parser = new UciCommandParser();

      Assert.Equal(expectedCommand, parser.ParseUciCommand(input));
    }

    [Fact]
    public void Parser_SetsPosition_PositionIsSet()
    {
      UciCommandParser parser = new UciCommandParser();

      string inputFen = "8/8/8/8/8/6k1/5q2/7K w - - 0 5";

      // Set the position to the starting position with the fen for it.
      var command = parser.ParseUciCommand($"position {inputFen}");

      // Ask the engine if it's board has the pieces I expect in the places I expect.
      var expectedCommand = new UCICommand(UCICommandType.Position, inputFen);
      Assert.Equal(expectedCommand, command);
      
    }
  }
}
