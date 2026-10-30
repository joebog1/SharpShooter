using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;

namespace SharpShooter.Pieces
{
  public enum PieceType
  {
    King = 1,
    Queen = 2,
    Rook = 3,
    Bishop = 4,
    Knight = 5,
    Pawn = 6,
  }

  public static class PieceExtension
  {
    // Converts FEN chess peice to type
    public static PieceType CharacterToType(char character)
    {
      character = char.ToUpper(character);
      return character switch
      {
        'K' => PieceType.King,
        'Q' => PieceType.Queen,
        'R' => PieceType.Rook,
        'B' => PieceType.Bishop,
        'N' => PieceType.Knight,
        'P' => PieceType.Pawn,
        _ => throw new ArgumentException($"Invalid piece character: {character}"),
      };
    }

    /// <summary>
    /// A factory for generating the right subclass of Piece based on the character.
    /// </summary>
    /// <param name="character"></param>
    /// <returns></returns>
    public static Piece FromFenCharacter(char character)
    {
      var colour = CharacterToColour(character);
      var type = CharacterToType(character);

      return type switch
      {
        PieceType.King => new King(colour),
        PieceType.Queen => new Queen(colour),
        PieceType.Rook => new Rook(colour),
        PieceType.Bishop => new Bishop(colour),
        PieceType.Knight => new Knight(colour),
        PieceType.Pawn => new Pawn(colour),

        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
      };

    }

    public static Colour CharacterToColour(char character)
    {
      bool isUpper = char.ToUpper(character) == character;
      // Upper case characters are white peices, lowercase characters are black peices.
      if (isUpper) return Colour.White; else return Colour.Black;
    }

  }

  // Represents who's Colour it is.
  public enum Colour
  {
    White = 1,
    Black = 2,
  }

  public abstract class Piece
  {
    Colour myColour;

    public Piece(Colour colour)
    {
        myColour = colour;
    }

    public Colour Colour() => myColour;
    public abstract PieceType Type();

    /// <summary>
    /// Generates a list of pesudo legal moves for the peice. 
    /// Guarenteees that every suggested move is legal.
    /// </summary>
    /// <param name="startingSquare"></param>
    /// <returns></returns>
    public abstract List<Move> GenerateLegalMoves(Square startingSquare,
                                                        Board boardBounds);

    /// <summary>
    /// Only adds to moves if the candidate move is on the board.
    /// Can be overriden for cases where specific piece rules apply (such as pawn diagonal 
    /// movement requires opponents peice on the diagonal).
    /// :WARNING: Joseph Brunato 30th Oct 2026.
    /// The base function simply checks if the candidateMove is on the board and that there isn't a 
    /// piece of its own colour on the candidateMove.AfterSquare. 
    /// This means that it doesn't check rays (meaning this can add a move if the caller isn't 
    /// careful to check that a rook isn't jumping over another piece in the middle of the ray.)
    /// </summary>
    /// <param name="moves"></param>
    /// <param name="candidateMove"></param>
    /// <param name="boardBounds"></param>
    public virtual void AddIfOnBoard(List<Move> moves,Move candidateMove, Board board)
    {
      var pieceAtPosition = board.PieceAtPosition(candidateMove.AfterSquare);
      if (board.IsOnBoard(candidateMove.AfterSquare) &&
        (pieceAtPosition == null ||
         pieceAtPosition?.Colour() != Colour()))
      {
        moves.Add(candidateMove);
      }
    }

    public override bool Equals(object? obj) =>
      obj is Piece other &&
      other.Colour() == Colour() &&
      other.Type() == Type();

    public override int GetHashCode() =>
      HashCode.Combine(Colour(), Type());

    public static bool operator ==(Piece? a, Piece? b) => Equals(a, b);
    public static bool operator !=(Piece? a, Piece? b) => !Equals(a, b);
  }
}
