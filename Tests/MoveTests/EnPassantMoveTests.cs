using SharpShooter;

namespace Tests.MoveTests
{
  public class EnPassantMoveTests
  {
    [Fact]
    public void Pawn_CanTakeEnPassant()
    {
      // Tests that we can construct a board state that allows En Passant and apply it.
      var board = new Board("1k6/8/8/4Pp2/8/8/8/2K5 w - f6 0 1");

      // Generate moves for the e5 pawn.
      var moves = board.GenerateLegalMovesFor("e5");

      // Two moves should be found, e5-e6 and exf6.
      Assert.Equal(2, moves.Count);
      Assert.Contains(new Move("e5", "e6"), moves);
      Assert.Contains(new Move("e5", "f6"), moves);
    }

    [Fact]
    public void Pawn_CannotTakeEnPassantDueToFen()
    {
      // Tests that if the Fen says en passant isn't allowed, then it is not listed as a legal move.
      var board = new Board("1k6/8/8/4Pp2/8/8/8/2K5 w - - 0 1");

      // Get the legal moves for th e5 pawn.
      var moves = board.GenerateLegalMovesFor("e5");

      // e5-e6 is the only legal move.
      Assert.Single(moves);
      var expectedMove = new Move("e5", "e6");
      Assert.Equal(expectedMove, moves[0]);

    }

    [Fact]
    public void MultiplePawns_MultipleCanTakeEnPassant()
    {
      // Tests that if the fen allows multiple pawns to take en passant that not all pawns can.
      var board = new Board("1k6/8/8/4PpP1/8/8/8/2K5 w - f6 0 1");

      // If we generate moves for the e5 or g5 pawns,
      // taking on f6 should be an option for both of them.
      var E5Moves = board.GenerateLegalMovesFor("e5");
      var G5Moves = board.GenerateLegalMovesFor("g5");

      // Assert that both can take on f6.
      Assert.Contains(("e5", "f6"), E5Moves);
      Assert.Contains(("g5", "f6"), G5Moves);
    }

    [Fact]
    public void EnPassant_TakesThePawnOffTheBoard()
    {
      // Tests that applying a en passant capture move must remove the pawn on the 3/5th rank!
      var board = new Board("1k6/8/8/4PpP1/8/8/8/2K5 w - f6 0 1");

      // Apply exf6
      board.ApplyMove(("e5", "f6"));

      // Ensure the f5 pawn is gone!
      Assert.Null(board.PieceAtPosition("f5"));
      // myEnPassantSquare should be null now.
      Assert.Null(board.myEnPassantSquare);
    }

    [Fact]
    public void EnPassantFen_MovingPawn_NullifiesEnPassant()
    {
      var board = new Board("1k6/8/8/4PpP1/8/8/8/2K5 w - f6 0 1");

      // Apply e5-e6
      board.ApplyMove(("e5", "e6"));

      // Ensure that en passant is null!
      Assert.Null(board.myEnPassantSquare);
    }

  }
}
