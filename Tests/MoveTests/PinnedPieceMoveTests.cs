using SharpShooter;
using SharpShooter.Pieces;

namespace Tests.MoveTests
{
  public class PinnedPieceMoveTests
  {
    [Fact]
    public void RookPinsKnight_KnightCantMove()
    {
      MoveTestUtil.VerifyMovesFor(
        "8/3K4/3N4/8/8/8/3r4/4k3 w - - 0 1",
        new Knight(Colour.White),
        new Square(3, 5),
        new List<Move> { });
    }

    [Fact]
    public void RookPinsPawn_PawnCanMove()
    {
      // In this test, the pawn can still move as it doesn't allow check.
      MoveTestUtil.VerifyMovesFor(
        "8/3K4/8/3P4/8/8/3r4/4k3 w - - 0 1",
        new Pawn(Colour.White),
        new Square(3, 4),
        new List<Move>
        {
          ((3, 4), (3, 5)), // d5 -> d6
        });
    }

    [Fact]
    public void RookPinsPawn_PawnCantMoveBecauseKingBlocks()
    {
      // In this test, the pawn can still move as it doesn't allow check.
      MoveTestUtil.VerifyMovesFor(
        "8/3K4/3P4/8/8/8/3r4/4k3 w - - 0 1",
        new Pawn(Colour.White),
        new Square(3, 5),
        new List<Move> { });
    }

    [Fact]
    public void RookPinsPawn_PawnCantCapture()
    {
      // In this test, the pawn can still move as it doesn't allow check, however it can't capture any
      // pieces because of the pin.
      MoveTestUtil.VerifyMovesFor(
        "8/3K4/2n1n3/3P4/8/8/3r4/4k3 w - - 0 1",
        new Pawn(Colour.White),
        new Square(3, 4),
        new List<Move>
        {
          ((3, 4), (3, 5)), // d5 -> d6
        });
    }

    [Fact]
    public void RookPinsRook_RookCanMoveVertically()
    {
      // In this test, white's rook is pinned, but it can still move vertically to take black's rook.
      MoveTestUtil.VerifyMovesFor(
        "8/8/8/8/3K4/3R4/3r4/4k3 w - - 0 1",
        new Rook(Colour.White),
        new Square(3, 2),
        new List<Move>
        {
          ((3, 2), (3, 1)), // Rxd2 only move for the rook.
        });
    }

    [Fact]
    public void BishopPinsRook_RookCantMove()
    {
      // In this test, white's rook is pinned by a bishop and therefore can't move.
      MoveTestUtil.VerifyMovesFor(
        "8/8/6K1/5R2/8/8/2b5/4k3 w - - 0 1",
        new Rook(Colour.White),
        new Square(5, 4),
        new List<Move>
        {
        });
    }

    [Fact]
    public void BishopPinsQueen_QueenCanMove()
    {
      // In this test, white's queen is pinned by a bishop and therefore can only
      // move long the axis of the bishop pin.
      MoveTestUtil.VerifyMovesFor(
        "8/8/6K1/8/4Q3/8/8/1b4k1 w - - 0 1",
        new Queen(Colour.White),
        new Square(4, 3),
        new List<Move>
        {
          ((4, 3),(1,0)),
          ((4, 3),(2,1)),
          ((4, 3),(3,2)),
          ((4, 3),(5,4)), // 4,3 is skipped because you can't move where you are.
        });
    }

  }
}
