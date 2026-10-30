using SharpShooter;
using SharpShooter.Pieces;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Tests
{
  public class MoveTests
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
      VerifyMoves(fen, expectedMoves);
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

    [Fact]
    public void PawnsInEachOthersWay_GeneratesMoves_NoMoves()
    {
      // Tests that two pawns with each other in their way can't move.
      VerifyMoves(
        "8/8/8/8/4p3/4P3/8/8 b - - 0 1",
        new List<Move>{});
    }

    [Fact]
    public void PawnsCanCapture_GeneratesMoves_Captures()
    {
      // Tests that two pawns that can capture each other generate capture moves.
      VerifyMoves(
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
      VerifyMovesFor(fen, piece, square, expectedMoves);
    }

    public static IEnumerable<object[]> KnightTestData => new List<object[]>
    {
      new object[] { "8/8/3N4/8/8/8/8/8 b - - 0 1", 'N',
                     3,
                     5,
                     new List<Move> { ((3, 5), (4, 7)),
                                      ((3, 5), (2, 7)), 
                                      ((3, 5), (4, 3)), 
                                      ((3, 5), (2, 3)),
                                      ((3, 5), (1, 4)),
                                      ((3, 5), (1, 6)),
                                      ((3, 5), (5, 6)),
                                      ((3, 5), (5, 4)),
                     } },
    };
    [Theory]
    [MemberData(nameof(KnightTestData))]
    public void KnightMoves_GenerateOnlyLegalKnightMoves(string fen, char pieceChar, int squareX, int squareY, List<Move> expectedMoves)
    {
      var piece = PieceExtension.FromFenCharacter(pieceChar);
      var square = new Square(squareX, squareY);
      VerifyMovesFor(fen, piece, square, expectedMoves);
    }
    //[Fact]
    //public void KnightsWithLikePieces_GeneratesMoves_CantTakeLikePieces()
    //{
    //  // Tests that a knight can't jump onto a square of a like piece.
    //}

    private static void VerifyMoves(string fen, List<Move> expectedMoves)
    {
      // Tests that a position generates all expected legal moves.
      var engine = new ChessEngine(fen);
      var actualMoves = engine.GenerateLegalMoves();
      Assert.Equal(expectedMoves.Count, actualMoves.Count);
      Assert.All(expectedMoves, expected => Assert.Contains(expected, actualMoves));
    }
    private static void VerifyMovesFor(string fen, Piece piece, Square square, List<Move> expectedMoves)
    {
      // Tests that a position generates all expected legal moves.
      var board = new Board(fen);
      var actualMoves = board.GenerateLegalMovesForPieceAtSqaure(piece, square);
      Assert.Equal(expectedMoves.Count, actualMoves.Count);
      Assert.All(expectedMoves, expected => Assert.Contains(expected, actualMoves));
    }

  }
}
