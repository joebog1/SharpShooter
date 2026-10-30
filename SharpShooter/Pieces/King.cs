using System;
using System.Collections.Generic;
using System.Text;

namespace SharpShooter.Pieces
{
  public class King : Piece
  {
    public King(Colour colour)
    : base(colour)
    {
    }

    public override PieceType Type()
    {
      return Pieces.PieceType.King;
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
      // :TODO: Check if generated moves leave the board.
      pesudoLegalMoves.Add((startingSquare, (startingSquare.File + 1, startingSquare.Rank))); // Up
      pesudoLegalMoves.Add((startingSquare, (startingSquare.File - 1, startingSquare.Rank))); // Down
      pesudoLegalMoves.Add((startingSquare, (startingSquare.File, startingSquare.Rank - 1))); // Left
      pesudoLegalMoves.Add((startingSquare, (startingSquare.File, startingSquare.Rank + 1))); // Right
      pesudoLegalMoves.Add((startingSquare, (startingSquare.File + 1, startingSquare.Rank + 1))); // Up-Right
      pesudoLegalMoves.Add((startingSquare, (startingSquare.File + 1, startingSquare.Rank - 1))); // Up-Left
      pesudoLegalMoves.Add((startingSquare, (startingSquare.File - 1, startingSquare.Rank + 1))); // Down-Right
      pesudoLegalMoves.Add((startingSquare, (startingSquare.File - 1, startingSquare.Rank - 1))); // Down-Left
      // :TODO: Representing the idea of castling as a move for the king.
      // This requires some work for the Move object to represent either side castling.
      return pesudoLegalMoves;
    }
  }
}
