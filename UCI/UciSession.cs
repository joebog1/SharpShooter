using System.Diagnostics;

namespace SharpShooter.UCI;

public class UciSession
{
  ChessEngine myChessEngine = new ChessEngine(UciCommandParser.StartingPosition());

  public Board GetBoard() { return myChessEngine!.myBoard; }

  public void Execute(UCICommand command)
  {
    switch (command.myCommandType)
    {
      case UCICommandType.Position:
        {
          Debug.Assert(command.myCommandArguments != null);
          myChessEngine.SetPositionFromFen(command.myCommandArguments.ToString());
          break;
        }
    }
  }
}
