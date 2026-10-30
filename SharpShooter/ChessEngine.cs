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

    public List<Move> GenerateLegalMoves()
    {
      var moves = new List<Move> { };
      foreach (var (piece, sqaure) in myBoard.Pieces())
      {
        if (piece.Colour() == myBoard.MyTurn)
        {
          moves.AddRange(myBoard.GenerateLegalMovesForPieceAtSqaure(piece, sqaure));
        }
      }
      return moves;
    }
  }
}
