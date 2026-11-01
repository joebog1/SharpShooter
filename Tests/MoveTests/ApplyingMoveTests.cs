using SharpShooter;
using SharpShooter.Pieces;

namespace Tests.MoveTests
{
  /// <summary>
  /// Tests that involve actually mutating the board state.
  /// </summary>
  public class ApplyingMoveTests
  {

    [Fact]
    public void StartingPosition_Plays_e4_StateUpdates()
    {
      var board = new Board("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
      Assert.Equal(Colour.White, board.myTurn);

      // Apply e2-e4.
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move((4, 1), (4, 3)));

      // Pawn should be at e4 and it should be black's turn.
      var expectedPieceAtE4 = new Pawn(Colour.White);
      Assert.Equal(expectedPieceAtE4, board.PieceAtPosition((4, 3)));
      Assert.Equal(Colour.Black, board.myTurn);
    }
  }
}
