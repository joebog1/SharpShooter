using SharpShooter;
using SharpShooter.Pieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.MoveTests
{
  public class PromotionTests
  {
    [Fact]
    public void PawnAlmostPromoting_GeneratesAllPromotionMoves()
    {
      // Tests that a pawn with only 1 square to go (no captures) generates all promotion moves.
      var board = new Board("1k6/4P3/8/8/8/8/8/2K5 w - - 0 1");

      // Generate the moves for the pawn and it should generate all promotions (Knight,Bishop,Rook,Queen).
      var moves = board.GenerateLegalMovesFor("e7");

      // Assert that there are 4 moves, one for each promotion type.
      Assert.Equal(4, moves.Count);
      Assert.Contains(new Move("e7", "e8", PieceType.Queen), moves);
      Assert.Contains(new Move("e7", "e8", PieceType.Rook), moves);
      Assert.Contains(new Move("e7", "e8", PieceType.Bishop), moves);
      Assert.Contains(new Move("e7", "e8", PieceType.Knight), moves);
      // Promoting to a pawn or king is illegal!
      Assert.DoesNotContain(new Move("e7", "e8", PieceType.King), moves);
      Assert.DoesNotContain(new Move("e7", "e8", PieceType.Pawn), moves);
      // Not promoting is illegal!
      Assert.DoesNotContain(new Move("e7", "e8", null), moves);
    }

    [Fact]
    public void PawnAlmostPromoting_IncludesCapturesPromotions()
    {
      // Tests that a pawn can capture and promote.
      var board = new Board("1k1r1r2/4P3/8/8/8/8/8/2K5 w - - 0 1");

      // Generate the moves for the pawn and it should generate all promotions (Knight,Bishop,Rook,Queen).
      var moves = board.GenerateLegalMovesFor("e7");

      // Because the pawn can take either side or go forward and promote, there should be 12 moves.
      Assert.Equal(12, moves.Count);
      Assert.Contains(new Move("e7", "e8", PieceType.Queen), moves);
      Assert.Contains(new Move("e7", "e8", PieceType.Rook), moves);
      Assert.Contains(new Move("e7", "e8", PieceType.Bishop), moves);
      Assert.Contains(new Move("e7", "e8", PieceType.Knight), moves);
      // Promoting to a pawn or king is illegal!
      Assert.DoesNotContain(new Move("e7", "e8", PieceType.King), moves);
      Assert.DoesNotContain(new Move("e7", "e8", PieceType.Pawn), moves);
      // Not promoting is illegal!
      Assert.DoesNotContain(new Move("e7", "e8", null), moves);

      Assert.Contains(new Move("e7", "d8", PieceType.Queen), moves);
      Assert.Contains(new Move("e7", "d8", PieceType.Rook), moves);
      Assert.Contains(new Move("e7", "d8", PieceType.Bishop), moves);
      Assert.Contains(new Move("e7", "d8", PieceType.Knight), moves);
      // Promoting to a pawn or king is illegal!
      Assert.DoesNotContain(new Move("e7", "d8", PieceType.King), moves);
      Assert.DoesNotContain(new Move("e7", "d8", PieceType.Pawn), moves);
      // Not promoting is illegal!
      Assert.DoesNotContain(new Move("e7", "d8", null), moves);

      Assert.Contains(new Move("e7", "f8", PieceType.Queen), moves);
      Assert.Contains(new Move("e7", "f8", PieceType.Rook), moves);
      Assert.Contains(new Move("e7", "f8", PieceType.Bishop), moves);
      Assert.Contains(new Move("e7", "f8", PieceType.Knight), moves);
      // Promoting to a pawn or king is illegal!
      Assert.DoesNotContain(new Move("e7", "f8", PieceType.King), moves);
      Assert.DoesNotContain(new Move("e7", "f8", PieceType.Pawn), moves);
      // Not promoting is illegal!
      Assert.DoesNotContain(new Move("e7", "f8", null), moves);
    }

    [Fact]
    public void ApplyingPawnPromotion_PromotesToRightPieceAndColour()
    {
      // Tests that applying all pawn promotion
      var board = new Board("1k6/4P3/8/8/8/8/8/2K5 w - - 0 1");

      // Generate the moves for the pawn and it should generate all promotions (Knight,Bishop,Rook,Queen).
      var moves = board.GenerateLegalMovesFor("e7");

      foreach(var move in moves)
      {
        // Generate board with promotion applied for it.
        var boardCopy = new Board("1k6/4P3/8/8/8/8/8/2K5 w - - 0 1");
        var expectedPromotionPiece = move.Promotion;

        boardCopy.ApplyMove(move);

        // e8 should be the expected promotionPiece and it should be white.
        var actualPiece = boardCopy.PieceAtPosition("e8");

        Assert.NotNull(actualPiece);
        Assert.Equal(expectedPromotionPiece, actualPiece.Type());
        Assert.Equal(Colour.White, actualPiece.Colour());
      }
    }
  }
}
