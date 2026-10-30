using System;
using System.Collections.Generic;
using System.Text;

namespace SharpShooter.Pieces
{
  public class Knight : Piece
  {
    public Knight(Colour colour)
      : base(colour)
    {
    }

    public override PieceType Type()
    {
      return Pieces.PieceType.Knight;
    }

    /// <summary>
    /// Generates a list of pesudo legal moves for a king. 
    /// Guarenteees that every suggested move is on the board, nothing else.
    /// </summary>
    /// <param name="startingSquare"></param>
    /// <returns></returns>
    public override List<Move> GenerateLegalMoves(Square startingSquare, Board boardBounds)
    {
      List<Move> pesudoLegalMoves = [];

      return pesudoLegalMoves;
    }
  }
}
