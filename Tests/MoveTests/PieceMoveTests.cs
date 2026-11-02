using SharpShooter;
using SharpShooter.Pieces;

namespace Tests.MoveTests
{
  public class PieceMoveTests
  {
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
      MoveTestUtil.VerifyMovesFor(fen, piece, square, expectedMoves);
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


  }
}
