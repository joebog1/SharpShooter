using System;
using System.Collections.Generic;
using System.Text;

namespace SharpShooter.Pieces
{
  public class Rook : Piece
  {
    public Rook(Colour colour)
      : base(colour)
    {
    }

    public override PieceType Type()
    {
      return Pieces.PieceType.Rook;
    }

    /// <summary>
    /// Generates a list of pesudo legal moves for a king. 
    /// Guarenteees that every suggested move is on the board, nothing else.
    /// </summary>
    /// <param name="startingSquare"></param>
    /// <returns></returns>
    public override List<Move> GeneratePesudoLegalMoves(Square startingSquare)
    {
      List<Move> pesudoLegalMoves = [];

      return pesudoLegalMoves;
    }
  }
}
