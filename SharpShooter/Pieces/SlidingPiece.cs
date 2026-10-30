using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SharpShooter.Pieces
{
  /// <summary>
  /// Represents a piece which slides across the board (Rook,Bishop and Queen)
  /// </summary>
  public abstract class SlidingPiece : Piece
  {
    protected abstract IEnumerable<(int File, int Rank)> Directions { get; }

    protected SlidingPiece(Colour colour) : base(colour)
    {
    }

    public override List<Move> GenerateLegalMoves(Square startingSquare,
                                                  Board board)
    {
      var moves = new List<Move>();

      foreach (var (fileDelta, rankDelta) in Directions)
      {
        Square candidateSquare = startingSquare;
        while (true)
        {
          candidateSquare = Square.Offset(candidateSquare, fileDelta, rankDelta);

          if (!board.IsOnBoard(candidateSquare))
            break;

          var occupant = board.PieceAtPosition(candidateSquare);

          if (occupant is null)
          {
            moves.Add((startingSquare, candidateSquare));
            continue;
          }

          if (occupant.Colour() != Colour())
            moves.Add((startingSquare, candidateSquare)); // Capture the first opposing piece.

          break; // Rest of the slide is blocked
        }
      }

      return moves;
    }

  }
}
