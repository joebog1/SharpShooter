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

  ChessEngine? myChessEngine;
    
  public string ParseUciCommand(string command)
  {
    // Split tokens to easily handle commands and arguments
    string[] tokens = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    if (tokens.Length == 0) return "";

    switch (tokens[0])
    {
      case "uci":
        return "id name SharpShooter\nid author Joseph Bruanto\nuciok";

      case "isready":
        return "readyok";

      case "quit":
        // :TODO: Should we delegate this back to the Progam? bit of a nuke atm,
        // not really easily testable either...
        Environment.Exit(0);
        break;

      case "position":
        // :TODO: it seems the position command can be used to send a fen AND additional moves.
        // We should probably handle that.
        string fen = string.Join(" ", tokens.Skip(1));
        if(myChessEngine == null)
        {
          myChessEngine = new ChessEngine(fen);
        }
        else
        {
          myChessEngine.SetPositionFromFen(fen);
        }
        break;
        // 
        // :TODO: Handle, go, setoption, etc. below...
    }
    // :TODO: handle this?
    return "";
  }
}
