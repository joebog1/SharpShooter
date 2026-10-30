using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SharpShooter.Pieces
{
  public class Pawn : Piece
  {
    public Pawn(Colour colour)
      : base(colour)
    {
    }
    public override PieceType Type()
    {
      return Pieces.PieceType.Pawn;
    }
    /// <summary>
    /// Generates a list of pesudo legal moves for a pawn. 
    /// Guarenteees that every suggested move is on the board, nothing else.
    /// </summary>
    /// <param name="startingSquare"></param>
    /// <returns></returns>
    public override List<Move> GenerateLegalMoves(Square startingSquare, Board boardBounds)
    {
      List<Move> pesudoLegalMoves = [];
      // :TODO: Promotions count as several valid possible moves. Include them.
      if (Colour() == Pieces.Colour.Black)
      {
        Move down = new Move(startingSquare, (startingSquare.File, startingSquare.Rank - 1));
        AddIfOnBoard(pesudoLegalMoves, down, boardBounds);

        Move downLeft = new Move(startingSquare, (startingSquare.File - 1, startingSquare.Rank - 1));
        AddIfOnBoard(pesudoLegalMoves, downLeft, boardBounds);

        Move downRight = new Move(startingSquare, (startingSquare.File + 1, startingSquare.Rank - 1));
        AddIfOnBoard(pesudoLegalMoves, downRight, boardBounds);
      }
      else
      {
        Debug.Assert(Colour() == Pieces.Colour.White);
        Move up = new Move(startingSquare, (startingSquare.File, startingSquare.Rank + 1));
        AddIfOnBoard(pesudoLegalMoves, up, boardBounds);

        Move upLeft = new Move(startingSquare, (startingSquare.File - 1, startingSquare.Rank + 1));
        AddIfOnBoard(pesudoLegalMoves, upLeft, boardBounds);

        Move upRight = new Move(startingSquare, (startingSquare.File + 1, startingSquare.Rank + 1));
        AddIfOnBoard(pesudoLegalMoves, upRight, boardBounds);
      }
      return pesudoLegalMoves;
    }

    /// <summary>
    /// Checks if candidate move is a legal pawn move given the board state.
    /// </summary>
    /// <param name="moves"></param>
    /// <param name="candidateMove"></param>
    /// <param name="board"></param>
    public override void AddIfOnBoard(List<Move> moves, Move candidateMove, Board board)
    {
      // A pawn needs to know if it can take diagonally because it can't move diagonally otherwise.
      // :TODO: EN PASSANT!
      // :TODO: check if moveleaves the board in an invalid state.
      var pieceAtAfterPosition = board.PieceAtPosition(candidateMove.AfterSquare);
      // First check if the move is strictly vertical.
      if (candidateMove.BeforeSquare.File == candidateMove.AfterSquare.File)
      {
        // Then we need only to check if anything is in the way. Pawns can't capture forward
        if(board.IsOnBoard(candidateMove.AfterSquare) &&
           pieceAtAfterPosition == null)
        {
          moves.Add(candidateMove);
        }
        return;
      }
      // Must be a diagonal move, check if a piece of opposite colour occupies the candidateMove
      // after square. 
      if(pieceAtAfterPosition is not null && 
         pieceAtAfterPosition!.Colour() != Colour())
      {
        // Then you can take it.
        moves.Add(candidateMove);
      }
      return;
    }
  } 
}
