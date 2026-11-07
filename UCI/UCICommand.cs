using System;
using System.Collections.Generic;
using System.Text;

namespace SharpShooter.UCI
{
  public enum UCICommandType
  {
    Nothing, // Nothing, generally means we are unable to find a command so we ignore it.
    UCI,     // Client is asking for us to give the id of engine, author and "uciok".
    IsReady, // Client is asking if we are ready, reply "readyok".
    Quit,    // Client is asking us to quit.
    Position,// Client is asking us to set the position of the board to the fen provided.
  }
  // :TODO: Make UCICommand a record struct?
  public class UCICommand
  {
    public UCICommandType myCommandType;

    // The command arguments differ based on what command type is being asked.
    // Several types, such as UCI and IsReady have no arguments.
    // Position command types include fen of the position to set the board to.
    public object? myCommandArguments;

    public UCICommand(UCICommandType commandType, object? commandArguments = null)
    {
      myCommandType = commandType;
      myCommandArguments = commandArguments;
    }

    public override bool Equals(object? obj) =>
      obj is UCICommand other &&
      myCommandType == other.myCommandType &&
      object.Equals(myCommandArguments, other.myCommandArguments);

    public override int GetHashCode() =>
      HashCode.Combine(myCommandType, myCommandArguments);
  }
}
