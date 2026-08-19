using SharpShooter;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
  public class UCITests
  {
    [Fact]
    public void Engine_AsksIsReady_returnsReadyOk()
    {
      var engine = new ChessEngine("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");


    }
  }
}
