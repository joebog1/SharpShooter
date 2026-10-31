using SharpShooter;
using SharpShooter.Pieces;

namespace Tests
{
  public class SpecialRuleMoveTests
  {
    [Fact]
    public void PawnsInEachOthersWay_GeneratesMoves_NoMoves()
    {
      // Tests that two pawns with each other in their way can't move.
      MoveTestUtil.VerifyMoves(
        "8/8/8/8/4p3/4P3/8/8 b - - 0 1",
        new List<Move> { });
    }

    [Fact]
    public void PawnsCanCapture_GeneratesMoves_Captures()
    {
      // Tests that two pawns that can capture each other generate capture moves.
      MoveTestUtil.VerifyMoves(
        "8/8/8/3p4/4P3/8/8/8 w - - 0 1",
        new List<Move>{((4,3),(3,4)), // capture
                       ((4,3),(4,4))  // move forward
                       });
    }

    public static IEnumerable<object[]> BishopTestData => new List<object[]>
    {
      new object[] { "8/8/8/8/8/5p2/8/7B w - - 0 1", 'B', 7, 0, new List<Move> { ((7, 0), (6, 1)), ((7, 0), (5, 2)) } }, // Capture the pawn
      new object[] { "8/8/8/8/8/5P2/8/7B w - - 0 1", 'B', 7, 0, new List<Move> { ((7, 0), (6, 1)) } },
      new object[] { "8/8/8/8/8/5p2/8/7b w - - 0 1", 'b', 7, 0, new List<Move> { ((7, 0), (6, 1)) } },
      new object[] { "8/6p1/8/8/8/8/8/B7 w - - 0 1", 'B', 0, 0, new List<Move> { ((0, 0), (1, 1)),
                                                                                 ((0, 0), (2, 2)),
                                                                                 ((0, 0), (3, 3)),
                                                                                 ((0, 0), (4, 4)),
                                                                                 ((0, 0), (5, 5)),
                                                                                 ((0, 0), (6, 6)),} }
    };

    [Theory]
    [MemberData(nameof(BishopTestData))]
    public void BishopWithPawnInTheWay_GenerateMovesForPieceAtPosition(string fen, char pieceChar, int squareX, int squareY, List<Move> expectedMoves)
    {
      var piece = PieceExtension.FromFenCharacter(pieceChar);
      var square = new Square(squareX, squareY);
      MoveTestUtil.VerifyMovesFor(fen, piece, square, expectedMoves);
    }

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

    [Fact]
    public void KingIsAllowedToCastle_CanCastle()
    {
      // Tests that a white king can castle either side.
      MoveTestUtil.VerifyMovesFor(
        "1k6/8/8/8/8/8/8/R3K2R w KQ - 0 1",
        new King(Colour.White),
        new Square(4, 0),
        new List<Move>
        {
          ((4, 0),(3,0)),
          ((4, 0),(3,1)),
          ((4, 0),(5,0)),
          ((4, 0),(5,1)),
          ((4, 0),(4,1)),
          ((4, 0),(6,0)), // O-O
          ((4, 0),(2,0)), // O-O-O
        });
    }

    [Fact]
    public void StateRemembersNoCastling_CantCastle()
    {
      // Tests that a white king can't castle if the board knows
      // castling isn't allowed.
      MoveTestUtil.VerifyMovesFor(
        "1k6/8/8/8/8/8/8/R3K2R w - - 0 1",
        new King(Colour.White),
        new Square(4, 0),
        new List<Move>
        {
          ((4, 0),(3,0)),
          ((4, 0),(3,1)),
          ((4, 0),(5,0)),
          ((4, 0),(5,1)),
          ((4, 0),(4,1)),
        });
    }
    [Fact]
    public void KingIsAllowedToCastle_ObstructionPreventsCastling()
    {
      // Tests that a white king can't castle because a piece is in the way..
      MoveTestUtil.VerifyMovesFor(
        "1k6/8/8/8/8/8/8/R1N1KN1R w K - 0 1",
        new King(Colour.White),
        new Square(4, 0),
        new List<Move>
        {
          ((4, 0),(3,0)),
          ((4, 0),(3,1)),
          ((4, 0),(5,0)),
          ((4, 0),(5,1)),
          ((4, 0),(4,1)),
          // Either side castle is illegal becase a piece is in the way.
        });
    }

    [Fact]
    public void KingIsAllowedToCastle_KnightPreventsCastling()
    {
      // Tests that a white king can't castle kingside because an opponents peice controls a
      // square that is in the path of the king's castle.
      MoveTestUtil.VerifyMovesFor(
        "8/1k6/8/8/8/6n1/8/R3K2R w KQ - 0 1",
        new King(Colour.White),
        new Square(4, 0),
        new List<Move>
        {
          ((4, 0),(3,0)),
          ((4, 0),(3,1)),
          ((4, 0),(5,0)),
          ((4, 0),(5,1)),
          ((4, 0),(4,1)),
          ((4, 0),(2,0)), // O-O-O is allowed.
        });
    }

    [Fact]
    public void KingIsAllowedToCastle_PinnedKnightPreventsCastling()
    {
      // Tests that a white king can't castle kingside even if an opponent's piece that
      // controlls the square is pinned!
      MoveTestUtil.VerifyMovesFor(
        "8/8/8/8/6k1/6n1/6Q1/R3K2R w KQ - 0 1",
        new King(Colour.White),
        new Square(4, 0),
        new List<Move>
        {
          ((4, 0),(3,0)),
          ((4, 0),(3,1)),
          ((4, 0),(5,0)),
          ((4, 0),(5,1)),
          ((4, 0),(4,1)),
          ((4, 0),(2,0)), // O-O-O is allowed.
        });
    }

    [Fact]
    public void KingIsAllowedToCastle_PinnedRookPreventsCastling()
    {
      // Tests that a white king can't castle kingside even if an opponent's piece that
      // controlls the square is pinned!
      MoveTestUtil.VerifyMovesFor(
        "8/8/2k5/8/8/5r2/6Q1/R3K2R w KQ - 0 1",
        new King(Colour.White),
        new Square(4, 0),
        new List<Move>
        {
          ((4, 0),(3,0)),
          ((4, 0),(3,1)),
          ((4, 0),(5,0)),
          ((4, 0),(5,1)),
          ((4, 0),(4,1)),
          ((4, 0),(2,0)), // O-O-O is allowed.
        });
    }

    [Fact]
    public void KingIsAllowedToCastle_PinnedBishopPreventsCastling()
    {
      // Tests that a white king can't castle kingside even if an opponent's piece that
      // controlls the square is pinned!
      MoveTestUtil.VerifyMovesFor(
        "8/8/8/8/8/1k2b1Q1/8/R3K2R w KQ - 0 1",
        new King(Colour.White),
        new Square(4, 0),
        new List<Move>
        {
          ((4, 0),(3,0)),
          ((4, 0),(5,0)),
          ((4, 0),(4,1)),
          // Either side castling isn't allowed.
        });
    }

    [Fact]
    public void KingIsAllowedToCastle_PawnPreventsCastling()
    {
      // Tests that a white king can't castle either side if a pawn implicitly prevents it!
      MoveTestUtil.VerifyMovesFor(
        "8/8/8/8/8/1k6/4p3/R3K2R w KQ - 0 1",
        new King(Colour.White),
        new Square(4, 0),
        new List<Move>
        {
          ((4, 0),(3,1)),
          ((4, 0),(5,1)),
          ((4, 0),(4,1)),
        });
    }

    [Fact]
    public void KingIsAllowedToCastle_PinnedPawnPreventsCastling()
    {
      // Tests that a white king can't castle kingside even if the pawn is pinned!
      MoveTestUtil.VerifyMovesFor(
        "8/8/8/8/8/8/2k1p1Q1/R3K2R w KQ - 0 1",
        new King(Colour.White),
        new Square(4, 0),
        new List<Move>
        {
          ((4, 0),(5,1)),
          ((4, 0),(4,1)),
        });
    }

    //[Fact]
    //public void KingIsAllowedToCastle_CastleMovesTheRook()
    //{
    //  // Tests that a white king castling kingside moves both the King and the correct Rook to the right
    //  // squares.
    //  MoveTestUtil.VerifyMovesFor(
    //    "1k6/8/8/8/8/8/8/4K2R w K - 0 1",
    //    new Queen(Colour.White),
    //    new Square(4, 0),
    //    new List<Move>
    //    {
    //      ((4, 0),(3,0)),
    //      ((4, 0),(3,1)),
    //      ((4, 0),(5,0)),
    //      ((4, 0),(5,1)),
    //      ((4, 0),(4,1)),
    //      ((4, 0),(6,0)), // O-O
    //    });
    //}
  }
}
