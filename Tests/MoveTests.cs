using SharpShooter;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
  public class MoveTests
  {

    //[Fact]
    //public void PawnAtE4_GeneratesMoveToE5()
    //{
    //  // Tests that a pawn at e4 can move to e5, provided there is no obstructions.
    //  var engine = new ChessEngine("4k3/8/8/8/4P3/8/8/4K3 w - - 0 1");
    //  var moves = engine.GenerateLegalMoves();
    //  Assert.Contains(new Move(new Square(4, 3), new Square(4, 4)), moves); // e4 -> e5
    //}
    //
    //[Fact]
    //public void PawnAtE4_MoveToE5_IsAtE5AndBlacksTurn()
    //{
    //  // Tests that a pawn at e4 can move to e5, provided there is no obstructions.
    //  var engine = new ChessEngine("4k3/8/8/8/4P3/8/8/4K3 w - - 0 1");
    //  var move = new Move(new Square(4, 3), new Square(4, 4));
    //
    //  engine.MakeMove(move);
    //
    //  Assert.Equal(Colour.Black, engine.MyTurn);
    //  Assert.Equal(new Piece(Colour.White, SharpShooter.Type.Pawn), engine.PieceAtPosition((4, 4)));
    //}
  }
}
