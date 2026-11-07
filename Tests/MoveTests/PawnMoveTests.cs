using SharpShooter;

namespace Tests.MoveTests
{
  public class PawnMoveTests
  {
    public static List<Move> BlackKingMoves()
    {
      return new List<Move>
      {
        ("a8","a7"),
        ("a8","b7"),
        ("a8","b8"),
      };
    }
    public static List<Move> WhiteKingMoves()
    {
      return new List<Move>
      {
        ("a1","a2"),
        ("a1","b2"),
        ("a1","b1"),
      };
    }

    [Fact]
    public void PawnsInEachOthersWay_GeneratesMoves_NoMoves()
    {
      // Tests that two pawns with each other in their way can't move.
      MoveTestUtil.VerifyMoves(
        "k7/8/8/8/4p3/4P3/8/K7 b - - 0 1",
        new List<Move>(BlackKingMoves())
        {
        });
    }

    [Fact]
    public void PawnsCanCapture_GeneratesMoves_Captures()
    {
      // Tests that two pawns that can capture each other generate capture moves.
      MoveTestUtil.VerifyMoves(
        "k7/8/8/3p4/4P3/8/8/K7 w - - 0 1",
        new List<Move>(WhiteKingMoves())
        {
          ((4,3),(3,4)), // capture
          ((4,3),(4,4))  // move forward
        });
    }

    [Fact]
    public void WhitePawnAtE4_PushToE5_IsGenerated()
    {
      // Tests that a white pawn at e4 can move to e5, provided there is no obstructions.
      MoveTestUtil.VerifyMoves(
        "k7/8/8/8/4P3/8/8/K7 w - - 0 1",
        new List<Move>(WhiteKingMoves())
        {
          ((4, 3), (4, 4)), // e4 -> e5
        });
    }

    [Fact]
    public void BlackPawnAtE4_PushToE3_IsGenerated()
    {
      // Tests that a black pawn at e4 can move to e3, provided there is no obstructions.
      MoveTestUtil.VerifyMoves(
        "k7/8/8/8/4p3/8/8/K7 b - - 0 1",
        new List<Move>(BlackKingMoves())
        {
          ((4, 3), (4, 2)), // e4 -> e3
        });
    }

    [Fact]
    public void PawnAtStartingSquare_CanJumpTwoSpaces()
    {
      // Tests that white pawns on the 2nd rank and black pawns on the 7th rank can
      // move one or two spaces.
      // :NOTE: Other tests such as BlackPawnAtE4_PushToE3_IsGenerated
      // Cover ensuring there's only one legal move if not on the right rank.
      // White pawn moves
      MoveTestUtil.VerifyMoves(
        "k7/pppppppp/8/8/8/8/PPPPPPPP/K7 w - - 0 1",
        new List<Move>()
        {
          // The king
          ("a1","b1"),
          // A pawn
          ((0,1),(0,2)),
          ((0,1),(0,3)),

          // B pawn
          ((1,1),(1,2)),
          ((1,1),(1,3)),

          // C pawn
          ((2,1),(2,2)),
          ((2,1),(2,3)),

          // D pawn
          ((3,1),(3,2)),
          ((3,1),(3,3)),

          // E pawn
          ((4,1),(4,2)),
          ((4,1),(4,3)),

          // F pawn
          ((5,1),(5,2)),
          ((5,1),(5,3)),

          // G pawn
          ((6,1),(6,2)),
          ((6,1),(6,3)),

          // H pawn
          ((7,1),(7,2)),
          ((7,1),(7,3)),
        }
        );
      // Black pawn moves
      MoveTestUtil.VerifyMoves(
        "k7/pppppppp/8/8/8/8/PPPPPPPP/K7 b - - 0 1",
        new List<Move>()
        {
          // The king
          ("a8","b8"),
          // A pawn
          ((0,6),(0,5)),
          ((0,6),(0,4)),

          // B pawn
          ((1,6),(1,5)),
          ((1,6),(1,4)),

          // C pawn
          ((2,6),(2,5)),
          ((2,6),(2,4)),

          // D pawn
          ((3,6),(3,5)),
          ((3,6),(3,4)),

          // E pawn
          ((4,6),(4,5)),
          ((4,6),(4,4)),

          // F pawn
          ((5,6),(5,5)),
          ((5,6),(5,4)),

          // G pawn
          ((6,6),(6,5)),
          ((6,6),(6,4)),

          // H pawn
          ((7,6),(7,5)),
          ((7,6),(7,4)),
        }
        );
    }


  }
}
