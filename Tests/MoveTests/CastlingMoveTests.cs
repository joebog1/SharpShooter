using SharpShooter;
using SharpShooter.Pieces;

namespace Tests.MoveTests
{
  public class CastlingMoveTests
  {
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
          ((4, 0),(5,1)),
          ((4, 0),(2,0)), // O-O-O is allowed.
        });
    }

    [Fact]
    public void KingIsAllowedToQueensideCastle_EvenIfBishopAimsAtGap()
    {
      // Tests the special case of a piece covering the b file doesn't prevent queenside castling.
      MoveTestUtil.VerifyMovesFor(
        "8/1k6/8/8/4b3/8/8/R3K2R w KQ - 0 1",
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
          ((4, 0),(5,1)),
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
          ((4, 0),(4,1)),
          ((4, 0),(2,0)), // O-O-O is allowed.
        });
    }

    [Fact]
    public void KingIsInCheck_KingCantCastle()
    {
      // Tests that a king in check can't castle.
      MoveTestUtil.VerifyMovesFor(
        "8/1k6/8/8/1b6/8/8/R3K2R w KQ - 0 1",
        new King(Colour.White),
        new Square(4, 0),
        new List<Move>
        {
          ((4, 0),(3,0)),
          ((4, 0),(5,0)),
          ((4, 0),(5,1)),
          ((4, 0),(4,1)),
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
    [Fact]
    public void KingWithPieceOnB1_QueensideCastlingIsPrevented()
    {
      // Tests that a white king can't castle queenside if a piece is on the b1 square.
      MoveTestUtil.VerifyMovesFor(
        "8/2k5/8/8/8/8/8/RB2K3 w Q - 0 1",
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
    public void KingMoves_CastlingRightsAreLost()
    {
      // Tests that moving the king loses all castling rights
      var board = new Board("rnbqkbnr/pppp1ppp/8/4p3/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 2");

      // Apply Ke2
      board.ApplyMove(("e1", "e2"));

      // Assert that White's castling rights are gone and black still has castling rights.
      Assert.False(board.CanCastleKingside(Colour.White));
      Assert.False(board.CanCastleQueenside(Colour.White));

      Assert.True(board.CanCastleQueenside(Colour.Black));
      Assert.True(board.CanCastleQueenside(Colour.Black));
    }

    [Theory]
    [InlineData(
        "r3k2r/ppp2ppp/8/8/8/8/PPPP1PPP/R3K2R w KQkq - 0 4",
        "h1",
        "g1",
        CastlingRights.WhiteQueenside | CastlingRights.BlackKingside | CastlingRights.BlackQueenside)]
    [InlineData(
        "r3k2r/ppp2ppp/8/8/8/8/PPPP1PPP/R3K2R w KQkq - 0 4",
        "a1",
        "b1",
        CastlingRights.WhiteKingside | CastlingRights.BlackKingside | CastlingRights.BlackQueenside)]
    [InlineData(
        "r3k2r/ppp2ppp/8/8/8/8/PPPP1PPP/R3K2R b KQkq - 0 4",
        "a8",
        "b8",
        CastlingRights.WhiteKingside | CastlingRights.WhiteQueenside | CastlingRights.BlackKingside)]
    [InlineData(
        "r3k2r/ppp2ppp/8/8/8/8/PPPP1PPP/R3K2R b KQkq - 0 4",
        "h8",
        "f8",
        CastlingRights.WhiteKingside | CastlingRights.WhiteQueenside | CastlingRights.BlackQueenside)]
    public void RookMoves_CastlingRightsOnThatSideAreLost(
        string fen,
        string from,
        string to,
        CastlingRights expectedCastlingRights)
    {
      // Tests that moving the Rook loses just the castling right on that side.
      var board = new Board(fen);

      // Apply Rg1
      board.ApplyMove((from, to));

      // Assert that White lost his kingside castling right.
      if (expectedCastlingRights.HasFlag(CastlingRights.WhiteKingside))
      {
        Assert.True(board.CanCastleKingside(Colour.White));
      }
      if (expectedCastlingRights.HasFlag(CastlingRights.WhiteQueenside))
      {
        Assert.True(board.CanCastleQueenside(Colour.White));
      }
      if (expectedCastlingRights.HasFlag(CastlingRights.BlackKingside))
      {
        Assert.True(board.CanCastleKingside(Colour.Black));
      }
      if (expectedCastlingRights.HasFlag(CastlingRights.BlackQueenside))
      {
        Assert.True(board.CanCastleQueenside(Colour.Black));
      }
    }

  }
}
