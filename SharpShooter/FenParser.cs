using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace SharpShooter
{
  [Flags]
  public enum CastlingRights
  {
    None = 0,
    WhiteKingside = 1,
    WhiteQueenside = 2,
    BlackKingside = 4,
    BlackQueenside = 8,
  }
  public sealed record Fen(
    IReadOnlyList<(Piece Piece, Square Square)> Pieces,
    Colour WhosTurn,
    CastlingRights Castling
);

  public static class FenParser
  {
    // Prases fen into pieces, who's turn it is and castling rights.
    public static Fen Parse(string fen)
    {
      var splitFen = fen.Split(' ')[0].Split('/');

      var extraFenInformation = fen.Split(' ').Skip(1).ToArray();

      // Now we have an array of strings. There should be 8 of them as there are 8 rows of a chessboard.
      Debug.Assert(splitFen.Length == 8);


      Colour whosTurn = Colour.White;

      if (extraFenInformation[0][0] == 'w') whosTurn = Colour.White;
      else if (extraFenInformation[0][0] == 'b') whosTurn = Colour.Black;
      else 
      { 
        Debug.Assert(false, "it should either by whites turn or blacks turn!");
      }

      // :NOTE: consider mimicing 'real' chess coordinates by using 1-8 for rank and a-g for file.
      // Fen starts from the 8th rank down to 1st rank.
      int rank = 8;
      int file = 0;
      List<(Piece Piece, Square Square)> pieces = new List<(Piece Piece, Square Square)>();
      foreach (var rankFen in splitFen)
      {
        rank--;
        foreach (char character in rankFen)
        {
          if (char.IsDigit(character))
          {
            // It is empty spaces.
            int emptySpaces = character - '0';
            file += emptySpaces;
          }
          else
          {
            // A peice is here!
            pieces.Add((new Piece(character), (file, rank)));
            file++;
          }
        }
        // Reset which file we are on.
        file = 0;
      }
      // Rank should always be 0 after checking every row.
      Debug.Assert(rank == 0);

      return new Fen(pieces, whosTurn, CastlingRightsFromExtraFenSnippet(extraFenInformation[1]));
    }

    // Returns who has castling rights by reading the CastlingString
    private static CastlingRights CastlingRightsFromExtraFenSnippet(string CastlingString)
    {
      CastlingRights castlingRights = CastlingRights.None;
      if (CastlingString.Contains('k')) castlingRights |= CastlingRights.BlackKingside;
      if (CastlingString.Contains('K')) castlingRights |= CastlingRights.WhiteKingside;
      if (CastlingString.Contains('q')) castlingRights |= CastlingRights.BlackQueenside;
      if (CastlingString.Contains('Q')) castlingRights |= CastlingRights.WhiteQueenside;
      return castlingRights;
    }

  }
}

