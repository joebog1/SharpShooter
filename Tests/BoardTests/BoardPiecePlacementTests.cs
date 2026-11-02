using SharpShooter;
using SharpShooter.Pieces;

namespace Tests.BoardTests
{
  public class BoardPiecePlacementTests
  {
    public static IEnumerable<object[]> PieceTestData => new[]
    {
      new object[] { "4k3/8/8/8/8/8/8/4K3 w - - 0 1", 4, 0, new King(Colour.White) },
      new object[] { "4k3/8/8/8/8/8/8/4K3 w - - 0 1", 4, 7, new King(Colour.Black)},
      new object[] { "3Rk3/8/8/8/8/8/8/4K3 b - - 0 1", 3, 7, new Rook(Colour.White)},
      new object[] { "k7/8/8/8/8/8/8/4K3 w - - 0 1", 0, 7, new King(Colour.Black)},
      new object[] { "7k/8/8/8/8/8/8/4K3 w - - 0 1", 7, 7, new King(Colour.Black)},
      new object[] { "7k/8/8/8/8/8/8/7K w - - 0 1", 7, 0, new King(Colour.White)},
      new object[] { "7k/8/8/8/8/8/7K/8 w - - 0 1", 7, 1, new King(Colour.White)},
      new object[] { "7k/8/8/8/8/7K/8/8 w - - 0 1", 7, 2, new King(Colour.White)},
      new object[] { "7k/8/8/8/7K/8/8/8 w - - 0 1", 7, 3, new King(Colour.White)},
      new object[] { "7k/8/8/7K/8/8/8/8 w - - 0 1", 7, 4, new King(Colour.White)},
      new object[] { "7k/8/7K/8/8/8/8/8 w - - 0 1", 7, 5, new King(Colour.White)},
      new object[] { "7k/7K/8/8/8/8/8/8 w - - 0 1", 7, 6, new King(Colour.White)},
    };

    [Theory]
    [MemberData(nameof(PieceTestData))]
    public void PieceAtPosition_ChecksPosition_IsExpectedPiece(string fen, int file, int rank, Piece expectedPiece)
    {
      // Arrange the board.
      var board = new Board(fen);
      // Get the piece at the expected position.
      var actualPiece = board.PieceAtPosition((File: file, Rank: rank));
      // Assert it is the expected piece.
      Assert.Equal(expectedPiece, actualPiece);
    }

    [Fact]
    public void SetupDefaultBoard_ChecksAllPiecePositions_AreExpectedPieces()
    {
      // This test ensures the default FEN has all pieces where I expect them to be.
      // I keep mixing up the rows and columns of the piece colours so this ensures
      // that gets caught if I ever do it again.
      var board = new Board("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
      Action<Piece, Square> assertPiece = (expected, square) =>
        Assert.Equal(expected, board.PieceAtPosition(square));

      // Black back rank.
      assertPiece(new Rook(Colour.Black), (File: 0, Rank: 7));
      assertPiece(new Knight(Colour.Black), (File: 1, Rank: 7));
      assertPiece(new Bishop(Colour.Black), (File: 2, Rank: 7));
      assertPiece(new Queen(Colour.Black), (File: 3, Rank: 7));
      assertPiece(new King(Colour.Black), (File: 4, Rank: 7));
      assertPiece(new Bishop(Colour.Black), (File: 5, Rank: 7));
      assertPiece(new Knight(Colour.Black), (File: 6, Rank: 7));
      assertPiece(new Rook(Colour.Black), (File: 7, Rank: 7));

      // Black pawns.
      for (int f = 0; f < 8; f++)
        assertPiece(new Pawn(Colour.Black), (File: f, Rank: 6));

      // White pawns.
      for (int f = 0; f < 8; f++)
        assertPiece(new Pawn(Colour.White), (File: f, Rank: 1));

      // White back rank.
      assertPiece(new Rook(Colour.White), (File: 0, Rank: 0));
      assertPiece(new Rook(Colour.White), (File: 7, Rank: 0));
      assertPiece(new Knight(Colour.White), (File: 1, Rank: 0));
      assertPiece(new Knight(Colour.White), (File: 6, Rank: 0));
      assertPiece(new Bishop(Colour.White), (File: 2, Rank: 0));
      assertPiece(new Bishop(Colour.White), (File: 5, Rank: 0));
      assertPiece(new Queen(Colour.White), (File: 3, Rank: 0));
      assertPiece(new King(Colour.White), (File: 4, Rank: 0));
    }

  }
}
