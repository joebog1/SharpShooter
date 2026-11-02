using SharpShooter;
using SharpShooter.Pieces;

namespace Tests.MoveTests
{
  public class KingMoveTests
  {
    public static IEnumerable<object[]> TwoKingsTestData => new List<object[]>
    {
      // White king.
      new object[]
      {
        "8/8/5k2/8/8/3K4/8/8 w - - 0 1",
        new List<Move>
        {
          ((3, 2), (3, 3)),  // Up
          ((3, 2), (3, 1)),  // Down
          ((3, 2), (4, 2)),  // Left
          ((3, 2), (2, 2)),  // Right
          ((3, 2), (4, 3)),  // Up-Right
          ((3, 2), (4, 1)),  // Down-Right
          ((3, 2), (2, 3)),  // Up-Left
          ((3, 2), (2, 1)),  // Down-Left
        }
      },
      // Black king.
      new object[]
      {
        "8/8/5k2/8/8/3K4/8/8 b - - 0 1",
        new List<Move>
        {
          ((5, 5), (5, 6)), // Up
          ((5, 5), (5, 4)), // Down
          ((5, 5), (4, 5)), // Left
          ((5, 5), (6, 5)), // Right
          ((5, 5), (6, 6)), // Up-Right
          ((5, 5), (6, 4)), // Down-Right
          ((5, 5), (6, 4)), // Up-Left
          ((5, 5), (4, 4)), // Down-Left
        }
      }
    };

    [Theory]
    [MemberData(nameof(TwoKingsTestData))]
    public void TwoKingsMidboard_AllKingMoves_AreGenerated(string fen, List<Move> expectedMoves)
    {
      // Kings not at boarders, should have 8 possible moves for the player whose turn it is.
      MoveTestUtil.VerifyMoves(fen, expectedMoves);
    }

    [Fact]
    public void KingOnEdge_GeneratesMoves_DoesntEscapeBoard()
    {
      // Tests that a king on the edge doesn't suggest leaving the board as a move.
      MoveTestUtil.VerifyMovesFor(
        "8/1k6/8/8/8/8/8/7K w - - 0 1",
        new King(Colour.White),
        new Square(7, 0),
        new List<Move>
        {
          ((7,0), (6,0)),
          ((7,0), (6,1)),
          ((7,0), (7,1)),
        });
    }

  }
}
