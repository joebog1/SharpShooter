using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SharpShooter
{
  public enum Type
  {
    King = 1,
    Queen = 2,
    Rook = 3,
    Bishop = 4,
    Knight = 5,
    Pawn = 6,
  }

  public static class PieceHelper
  {
    // Converts FEN chess peice to type
    public static Type CharacterToType(char character)
    {
      character = char.ToUpper(character);
      return character switch
      {
        'K' => Type.King,
        'Q' => Type.Queen,
        'R' => Type.Rook,
        'B' => Type.Bishop,
        'N' => Type.Knight,
        'P' => Type.Pawn,
        _ => throw new ArgumentException($"Invalid piece character: {character}"),
      };
    }
    public static Colour CharacterToColour(char character)
    {
      bool isUpper = char.ToUpper(character) == character;
      if (isUpper) return Colour.Black; else return Colour.White;
    }

  }

  // Represents who's Colour it is.
  public enum Colour
  {
    White = 1,
    Black = 2,
  }

  public class Piece
  {
    Colour myColour;
    Type myType;

    public Piece(Colour colour, Type type)
    {
        myColour = colour;
        myType = type;
    }

    public Piece(char peiceCharacter)
    {
      myType = PieceHelper.CharacterToType(peiceCharacter);
      myColour = PieceHelper.CharacterToColour(peiceCharacter);

    }

    public Colour Colour() => myColour;
    public Type Type() => myType;

    public override bool Equals(object? obj) =>
      obj is Piece other && other.myColour == myColour && other.myType == myType;

    public override int GetHashCode() => HashCode.Combine(myColour, myType);
  }
}
