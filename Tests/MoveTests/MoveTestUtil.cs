using SharpShooter;
using SharpShooter.Pieces;

namespace Tests
{
  internal static class MoveTestUtil
  {
    internal static void VerifyMoves(string fen, List<Move> expectedMoves)
    {
      // Tests that a position generates all expected legal moves.
      var engine = new ChessEngine(fen);
      var actualMoves = engine.GenerateLegalMoves();
      Assert.Equal(expectedMoves.Count, actualMoves.Count);
      Assert.All(expectedMoves, expected => Assert.Contains(expected, actualMoves));
    }

    internal static void VerifyMovesFor(string fen, Piece piece, Square square, List<Move> expectedMoves)
    {
      // Tests that a position generates all expected legal moves.
      var board = new Board(fen);
      var actualMoves = board.GenerateLegalMovesForPieceAtSqaure(piece, square);
      Assert.Equal(expectedMoves.Count, actualMoves.Count);
      Assert.All(expectedMoves, expected => Assert.Contains(expected, actualMoves));
    }
  }
}
