using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SharpShooter
{
  public class Board
  {
    List<(Piece Piece, Square Square)> myPieces;

    public List<(Piece Piece, Square Square)> Pieces() { return myPieces; }

    public Board(string fen) : this(FenParser.Parse(fen)) { }

    public Board(Fen fen) 
    {
      myPieces = new();
      foreach (var (piece, position) in fen.Pieces)
      {
        myPieces!.Add((piece, position));
      }
    }

    public Square? FindKing(Colour colour)
    {
      // This method assumes the board is valid (because then a king will exist).
      // Debug.Assert(IsValidPosition()); is what I would like to assert, but that is recursive!
      var pieceToLookFor = new Piece(colour, Type.King);
      foreach (var (peice, position) in myPieces)
      {
        // :MYTODO: Ensure this isn't a check by reference!
        if (peice == pieceToLookFor) return position;
      }
      return null;
    }


    public Piece? PieceAtPosition(Square location)
    {
      // :NOTE: O(n) piece look up is not ideal... #1
      foreach (var (piece, position) in myPieces)
      {
        // :MYTODO: hope this isn't another reference check!
        if (position == location)
        {
          return piece;
        }
      }
      // Not found.
      return null;
    }

  }
}
