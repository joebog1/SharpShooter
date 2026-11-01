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

    [Fact]
    public void StartingPosition_ScandinavianOpening_CapturesCorrectly()
    {
      var board = new Board("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
      Assert.Equal(Colour.White, board.myTurn);

      // Apply 1.e4.
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move((4, 1), (4, 3)));

      // Apply 1..d5
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move((3, 6), (3, 4)));

      // Apply 2.exd5
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move((4, 3), (3, 4)));

      // Verify that the pawn on d5 is white's pawn and black now has 7 pawns.
      var piece = board.PieceAtPosition((3, 4));
      Assert.Equal(PieceType.Pawn, piece?.Type());
      Assert.Equal(Colour.White, piece?.Colour());

      int numberOfBlackPawns = 0;

      foreach (var (potentialBlackPawn, square) in board.Pieces())
      {
        if(potentialBlackPawn.Type() == PieceType.Pawn && 
            potentialBlackPawn.Colour() == Colour.Black)
        {
          numberOfBlackPawns++;
          // Sanity check, ensure no black pawn says they are on d5.
          Assert.NotEqual(square, new Square(4, 3));
        }
      }
      Assert.Equal(7, numberOfBlackPawns);
    }

    [Fact]
    public void PlayTheOperaGame_EveryMoveIsValid()
    {
      // Play through the entierty of the opera game. Every move should be valid.
      var board = new Board("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
      Assert.Equal(Colour.White, board.myTurn);

      // 1.e4.
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("e2", "e4"));
      // 1..e5
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("e7", "e5"));
      // 2.Nf3
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("g1", "f3"));
      // 2..d6?!
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("d7", "d6"));
      // 3.d4!
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("d2", "d4"));
      // 3..Bg4?
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("c8", "g4"));
      // 4.dxe5!
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("d4", "e5"));
      // 4..Bxf3
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("g4", "f3"));
      // 5. Qxf3
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("d1", "f3"));
      // 5.. dxe5
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("d6", "e5"));
      // 6. Bc4
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("f1", "c4"));
      // 6.. Nf6
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("g8", "f6"));
      // 7. Qb3
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("f3", "b3"));
      // 7.. Qe7
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("d8", "e7"));
      // 8. Nc3!?
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("b1", "c3"));
      // 8.. c6
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("c7", "c6"));
      // 9. Bg5
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("b1", "g5"));
      // 9.. b5?
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("b7", "b5"));
      // 10. Nxb5!
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("c3", "b5"));
      // 10.. cxb5
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("c6", "b5"));
      // 11. Bxb5+
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("c4", "b5"));
      // Sanity check, ensure that black is indeed in check at this point.
      Assert.True(board.IsInCheck());
      // 11.. Nd7
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("b8", "d7"));
      // Sanity check, Black is no longer in check.
      Assert.True(board.IsKingInCheck(Colour.Black));
      // 12. O-O-O
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("e1", "c1"));
      // Sanity check, a Rook must be on d1 now.
      // :TODO: this currently fails, Apply move has no understanding that castling moves the rook.
      Assert.Equal(new Rook(Colour.White), board.PieceAtPosition("d1"));
      // 12.. Rd8
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("a8", "d8"));
      // 13. Rxd7!
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("d1", "d7"));
      // 13.. Rxd7
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("d8", "d7"));
      // 14. Rd1
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("g1", "d1"));
      // 14.. Qe6
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("e7", "e6"));
      // 15. Bxd7
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("b5", "d7"));
      // 15.. Nxd7
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("f6", "d7"));
      // 16. Qb8!!
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("b3", "b8"));
      // 16.. Nxb8
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("d7", "b8"));
      // 17. Rd8#
      MoveTestUtil.ApplyMoveAndVerifySuccess(board, new Move("d1", "d8"));
      // Ensure the board understands that it is implicitly mate.
      Assert.True(board.IsGameOver() == GameResult.WhiteWins);

    }

  }
}
