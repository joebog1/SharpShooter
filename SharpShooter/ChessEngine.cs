using System.Diagnostics.CodeAnalysis;

namespace SharpShooter
{

  public class ChessEngine
  {
    public ChessEngine(string fen) { SetPositionFromFen(fen); }

    private Board myBoard;

    [MemberNotNull(nameof(myBoard))]
    public void SetPositionFromFen(string fen)
    {
      Fen parsedFen = FenParser.Parse(fen);
      myBoard = new Board(parsedFen);
    }
  }
}
