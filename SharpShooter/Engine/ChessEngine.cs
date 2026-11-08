using SharpShooter.Pieces;
using System.Diagnostics.CodeAnalysis;

namespace SharpShooter
{

  public class ChessEngine
  {
    public ChessEngine(string fen) { SetPositionFromFen(fen); }

    public Board myBoard;

    [MemberNotNull(nameof(myBoard))]
    public void SetPositionFromFen(string fen)
    {
      Fen parsedFen = FenParser.Parse(fen);
      myBoard = new Board(parsedFen);
    }

    /// <summary>
    /// Suggests the top moves to display
    /// </summary>
    /// <param name="numberOfMovesToDisplay"></param> The number of top moves to return, ordered 0th is the best move and nth is n+1th best.
    /// <param name="depth"></param> How many moves deep does it look.
    /// <returns></returns>
    /// // :TODO: Perhaps a Move on the engine level is a move + and evaluation metric?
    public List<Move> GenerateBestMoves(Board position, int numberOfMovesToDisplay = 1, int depth = 5)
    {
      // :TODO: This implementation is a stub.
      List<Move> bestMoves = [];

      var legalMoves = myBoard.GenerateLegalMoves();
      Colour currentTurn = position.myTurn.Value;

      // If any move suggested leads to a board state where the game is won,
      // it is the best move.
      foreach (var move in legalMoves)
      {
        var boardAfterMove = new Board(myBoard);
        if (boardAfterMove.ApplyMove(move))
        {
          // Mate in 1 is a top move!
          if (currentTurn == Colour.White && boardAfterMove.Result() == GameResult.WhiteWins)
          {
            bestMoves.Insert(0, move);
          }
          else if (currentTurn == Colour.Black && boardAfterMove.Result() == GameResult.BlackWins)
          {
            bestMoves.Insert(0, move);
          }
          else
          {
            // :TODO: If move isn't just winning the game instantly we need to evaulate the
            // position after the move and rank it accordingly.
            bestMoves.Add(move);
          }
        }
      }
      return bestMoves.Take(numberOfMovesToDisplay).ToList();
    }

    public float Evaluate(Board board)
    {
      // Evaulate the given board and give a score as to who is winning.
      return 12;
    }
  }
}
