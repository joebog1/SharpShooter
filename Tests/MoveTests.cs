using SharpShooter;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
  public class MoveTests
  {
    [Fact]
    public void TwoKingsMidboard_AllKingMoves_AreGenerated()
    {
      // Two kings not at boarders, should have 16 possible moves
      VerifyMoves(
        "8/8/5k2/8/8/3K4/8/8 w - - 0 1",
        new List<Move>
        {
          // White king
          ((3, 2), (3, 3)),  // Up
          ((3, 2), (3, 1)),  // Down
          ((3, 2), (4, 2)),  // Left
          ((3, 2), (2, 2)),  // Right
          ((3, 2), (4, 3)),  // Up-Right
          ((3, 2), (4, 1)),  // Down-Right
          ((3, 2), (2, 3)),  // Up-Left
          ((3, 2), (2, 1)),  // Down-Left
          // Black king
          ((5, 5), (5, 6)), // Up
          ((5, 5), (5, 4)), // Down
          ((5, 5), (4, 5)), // Left
          ((5, 5), (6, 5)), // Right
          ((5, 5), (6, 6)), // Up-Right
          ((5, 5), (6, 4)), // Down-Right
          ((5, 5), (6, 4)), // Up-Left
          ((5, 5), (4, 4)), // Down-Left
        });
    }

    [Fact]
    public void WhitePawnAtE4_PushToE5_IsGenerated()
    {
      // Tests that a white pawn at e4 can move to e5, provided there is no obstructions.
      VerifyMoves(
        "8/8/8/8/4P3/8/8/8 w - - 0 1",
        new List<Move>
        {
          ((4, 3), (4, 4)), // e4 -> e5
        });
    }

    [Fact]
    public void BlackPawnAtE4_PushToE3_IsGenerated()
    {
      // Tests that a black pawn at e4 can move to e3, provided there is no obstructions.
      VerifyMoves(
        "8/8/8/8/4p3/8/8/8 b - - 0 1",
        new List<Move>
        {
          ((4, 3), (4, 2)), // e4 -> e3
        });
    }

    private static void VerifyMoves(string fen, List<Move> expectedMoves)
    {
      // Tests that a position generates all expected legal moves.
      var engine = new ChessEngine(fen);
      var actualMoves = engine.GenerateLegalMoves();
      Assert.Equal(expectedMoves.Count, actualMoves.Count);
      Assert.All(expectedMoves, expected => Assert.Contains(expected, actualMoves));
    }

  }
}
