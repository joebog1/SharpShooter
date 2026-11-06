using SharpShooter;
using SharpShooter.Pieces;

namespace Tests.BoardTests
{
  public class PositionValidityTests
  {
    [Fact]
    public void EmptyBoard_ChecksValidity_IsInvalid()
    {
      // Arrange an empty board is invalid because kings are required to exist.
      var board = new Board("8/8/8/8/8/8/8/8 w - - 0 1");

      // Act by checking if the position we are in is valid.
      bool isValid = board.IsValidPosition();

      // Assert that the position is invalid.
      Assert.False(isValid);
    }
    [Theory]
    [InlineData("8/8/8/4k3/8/2K5/8/5P2 w - - 0 1")]
    [InlineData("8/8/8/4k3/8/2K5/8/5P2 b - - 0 1")]
    [InlineData("3P4/8/8/4k3/8/2K5/8/8 w - - 0 1")]
    [InlineData("3P4/8/8/4k3/8/2K5/8/8 b - - 0 1")]
    [InlineData("3p4/8/8/4k3/8/2K5/8/8 w - - 0 1")]
    [InlineData("3p4/8/8/4k3/8/2K5/8/8 b - - 0 1")]
    [InlineData("8/8/8/4k3/8/2K5/8/p7 w - - 0 1")]
    [InlineData("8/8/8/4k3/8/2K5/8/p7 b - - 0 1")]
    public void PawnsOnFirstRank_ChecksValidity_IsInvalid(string fen)
    {
      // Arrange an empty board is invalid because kings are required to exist.
      var board = new Board(fen);

      // Act by checking if the position we are in is valid.
      bool isValid = board.IsValidPosition();

      // Assert that the position is invalid.
      Assert.False(isValid);
    }

    [Theory]
    [InlineData("4k3/8/8/8/8/8/8/4K3 w - - 0 1", true, "Two kings far apart is valid")]
    [InlineData("3Kk3/8/8/8/8/8/8/8 w - - 0 1", false, "Kings adjacent on same row is invalid")]
    [InlineData("3K3/8/8/8/3k4/8/8/3K4 w - - 0 1", false, "needs to be exactly 1 of each king")]
    [InlineData("3K3/8/8/8/8/8/8/8 w - - 0 1", false, "needs to be exactly 1 of each king")]
    [InlineData("4K3/3k4/8/8/8/8/8/8 w - - 0 1", false, "Kings touching diagonally is invalid")]
    [InlineData("8/8/8/8/8/8/3k4/3K4 w - - 0 1", false, "Kings adjacent on same column is invalid")]
    [InlineData("4k3/8/8/8/8/8/8/3PK3 w - - 0 1", false, "Pawns can't exist on the first rank")]
    [InlineData("4k3/8/8/8/8/8/8/3pK3 w - - 0 1", false, "Pawns can't exist on the first rank")]
    [InlineData("4Pk3/8/8/8/8/8/8/3K3 w - - 0 1", false, "Pawns can't exist on the first rank")]
    [InlineData("4pk3/8/8/8/8/8/8/3K3 w - - 0 1", false, "Pawns can't exist on the first rank")]
    public void KingProximity_ChecksValidity_ReturnsExpected(string fen, bool expected, string _)
    {
      var board = new Board(fen);

      bool isValid = board.IsValidPosition();

      Assert.Equal(expected, isValid);
    }

    [Theory]
    [InlineData("8/8/2k5/8/8/8/8/4K3 w Q - 0 1")]
    [InlineData("8/8/2k5/8/8/8/8/4K3 w K - 0 1")]
    public void IllegalCastlingRights_IsValidPosition_IsInvalid(string fen)
    {
      var board = new Board(fen);

      bool actual = board.IsValidPosition();

      Assert.False(actual);
    }

    [Theory]
    [InlineData("4k2r/8/8/8/8/8/8/4K2R w kK - 0 1")]
    [InlineData("r3k3/8/8/8/8/8/8/R3K3 w qQ - 0 1")]
    public void LegalCastlingRights_IsValidPosition_IsValid(string fen)
    {
      var board = new Board(fen);

      bool actual = board.IsValidPosition();

      Assert.True(actual);
    }


    [Theory]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", true, true, true, true)]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQk - 0 1", true, true, true, false)]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQq - 0 1", true, true, false, true)]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQ - 0 1", true, true, false, false)]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w Kkq - 0 1", true, false, true, true)]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w Kk - 0 1", true, false, true, false)]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w K - 0 1", true, false, false, false)]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w Qkq - 0 1", false, true, true, true)]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w Qk - 0 1", false, true, true, false)]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w Qq - 0 1", false, true, false, true)]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w Q - 0 1", false, true, false, false)]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w q - 0 1", false, false, false, true)]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w k - 0 1", false, false, true, false)]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w - - 0 1", false, false, false, false)]
    public void Result_Fen_BoardKnowsWhoCanCastle(string fen,
                                                  bool whiteKing,
                                                  bool whiteQueen,
                                                  bool blackKing,
                                                  bool blackQueen
                                                  )
    {
      var board = new Board(fen);

      // Assert that the castling rights are what the fen says.
      Assert.Equal(whiteKing, board.CanCastleKingside(Colour.White));
      Assert.Equal(whiteQueen, board.CanCastleQueenside(Colour.White));

      Assert.Equal(blackKing, board.CanCastleKingside(Colour.Black));
      Assert.Equal(blackQueen, board.CanCastleQueenside(Colour.Black));
    }


  }
}
