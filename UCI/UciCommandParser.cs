using System.Diagnostics;

namespace SharpShooter.UCI;

public class UciCommandParser
{
    /// :TODO:
    /// 
    /// How to break up this class:
    /// * Parser: Only reads the tokens and feeds the correct arguments to the UciSession
    /// * Session: Told by the console host what to do, manages internal state like the ChessEngine.
    /// * ConsoleHost: takes over what Program is doing now (starting the session and telling the parser to get to work.)
    /// * Program.cs, essentially main.cpp, starts the ConsoleHost.
    /// 
    
  public UCICommand ParseUciCommand(string command)
  {
    // Split tokens to easily handle commands and arguments
    string[] tokens = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    if (tokens.Length == 0) return new UCICommand(UCICommandType.Nothing);

    switch (tokens[0])
    {
      case "uci":
        return new UCICommand(UCICommandType.UCI);

      case "isready":
        return new UCICommand(UCICommandType.IsReady);

      case "quit":
        return new UCICommand(UCICommandType.Quit);

      case "position":
        // :TODO: it seems the position command can be used to send a fen AND additional moves.
        // We should probably handle that.
        string fen = string.Join(" ", tokens.Skip(1));
        return new UCICommand(UCICommandType.Position, fen);

        // :TODO: Handle, go, setoption, etc. below...
    }
    // :TODO: handle this?
    Debug.Assert(false, "Unreachable?");
    return new UCICommand(UCICommandType.Nothing); ;
  }
}
