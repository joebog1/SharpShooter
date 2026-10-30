using System;
using System.Collections.Generic;
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
        var current = startingSquare;

        while (true)
        {
          current = new Square(
              current.File + fileDelta,
              current.Rank + rankDelta);

          if (!board.IsOnBoard(current))
            break;

          var occupant = board.PieceAtPosition(current);

          if (occupant is null)
          {
            moves.Add((startingSquare, current));
            continue;
          }

          if (occupant.Colour() != Colour())
            moves.Add((startingSquare, current)); // Capture the first opposing piece.

          break; // Rest of the slide is blocked
        }
      }

      return moves;
    }

  }
}
