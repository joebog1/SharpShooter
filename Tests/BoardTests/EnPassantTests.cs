using SharpShooter;

namespace Tests.BoardTests
{
  public class EnPassantTests
  {
    [Fact]
    public void PawnCanTakeEnPassant_OtherMoveIsMade_EnPassantPrivlagesAreGone()
    {
      // Tests that we can construct a board state that allows En Passant and apply some other move,
      // then the board wipes the en passant list.
      var board = new Board("1k6/8/8/4Pp2/8/8/8/2K5 w - f6 0 1");

      // Play Kc2
      board.ApplyMove(("c1", "c2"));

      // Ensure that myEnPassantSquare is empty.
      Assert.Null(board.myEnPassantSquare);
    }

    [Fact]
    public void PositionAllowsForEnPassant_BoardAcknolwedgesIt()
    {
      // Tests that given a fen string that includes en passant information,
      // the board tells us which squares can be treated as en-passant squares.
      var board = new Board("1k6/8/8/4Pp2/8/8/8/2K5 w - f6 0 1");

      // Assert that f6 is recognised as a special enpassant square.
      Assert.Equal("f6", board.myEnPassantSquare!.Value);
    }

    [Fact]
    public void PawnJumpsTwoSpacesWithoutEnPassantPossibility_BoardDoesNotAddItToEnPassantList()
    {
      // Tests that if a player plays a pawn move like e2-e4 that the board allows for the possibility to
      // take en-passant next move.
      // :NOTE: Our implementation on FEN follows section 16.1.3.4 of the 2026-04-18 version of the
      // "Portable Game Notation Specification and Implementation Guide" standard.
      var board = new Board("1k6/8/8/5p2/8/8/4P3/2K5 w - - 0 1");

      // E4 is played.
      board.ApplyMove(new Move("e2", "e4"));

      // Check that the board does not list e3 as an en passant square (in fact, it should be empty!)
      Assert.Null(board.myEnPassantSquare);
    }

    [Fact]
    public void PawnJumpsOneSpace_BoardEnPassantListIsEmpty()
    {
      // Tests that if a player plays a pawn move like e2-e3 the board does not allow for en passant next move.
      var board = new Board("1k6/8/8/5p2/8/8/4P3/2K5 w - - 0 1");

      // E3 is played.
      board.ApplyMove(new Move("e2", "e3"));

      // No valid en passant squares should exist.
      Assert.Null(board.myEnPassantSquare);
    }

  }
}
