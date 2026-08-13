using System.Diagnostics;
using System.Globalization;

namespace SharpShooter
{
    using Square = (int File, int Rank);

  public class ChessEngine
  {
    public ChessEngine(string fen) { SetPositionByFen(fen); }

    string myFen;

    string[]mySplitFen => myFen.Split(' ')[0].Split('/');

    // I want all of the string splits after the first one.
    string[] myExtraFenInformation => myFen.Split(' ').Skip(1).ToArray();

    Piece[,] myBoard = new Piece[8,8];

    // Can be null if the position is invalid. assume it isn't otherwise.
    Colour? myTurn;

    public void SetPositionByFen(string fen)
    {
      myFen = fen;
      // Now we have an array of strings. There should be 8 of them as there are 8 rows of a chessboard.
      Debug.Assert(mySplitFen.Length == 8);

      if (myExtraFenInformation[0][0] == 'w') myTurn = Colour.White;
      else if (myExtraFenInformation[0][0] == 'b') myTurn = Colour.Black;
      else { Debug.Assert(false, "it should either by whites turn or blacks turn!"); }
      
      // :NOTE: consider mimicing 'real' chess coordinates by using 1-8 for rank and a-g for file.
      // Fen starts from the 8th rank down to 1st rank.
      int rank = 8;
      int file = 0;
      foreach (var rankFen in mySplitFen)
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
            file++;
            myBoard[file, rank] = new Piece(character);
          }
        }
        // Reset which file we are on.
        file = 0;
      }
      // rank should always be 0 after checking every row.
      Debug.Assert(rank == 0);
    }

    private Square? FindKing(Colour colour)
    {
      // This method assumes the board is valid (because then a king will exist).
      // Debug.Assert(IsValidPosition()); is what I would like to assert, but that is recursive!
      for (int rank = 0; rank < 8; rank++)
        for (int file = 0; file < 8; file++)
        {
          var piece = myBoard[file, rank];
          if (piece != null && piece.Colour() == colour && piece.Type() == Type.King)
            return (File: file, Rank: rank);
        }
      return null;
    }

    // Returns true if the current player's Colour is in check. False otherwise
    // :NOTE: it's impossible for the other player to be in check on your Colour.
    // The game ends before that.
    public bool IsInCheck()
    {
      // :TODO: This is going to explode when IsValidPosition calls IsInCheck to handle illegal
      // positions involving both kings in check. This funciton assumes a valid state as it only
      // checks the king who's turn it is.
      Debug.Assert(IsValidPosition());
      // Early exit for garbage case.
      if(!IsValidPosition()) return false;

      Square kingWhoCouldBeInCheck = FindKing(myTurn!.Value)!.Value;

      // Check if there are any rooks of the opposite colour on the same rank and file.
      Colour otherColour = myTurn!.Value == Colour.White ? Colour.Black : Colour.White;

      var rookToLookFor = new Piece(otherColour, Type.Rook);

      // Naively go through every file along the same rank to see if a rook of the opposite colour is there.
      for (int file = 0; file < 8; file++)
      {
        if(PieceAtPosition((file, kingWhoCouldBeInCheck.Rank)) == rookToLookFor)
        {
          return true;
        }
      }
      // Now check every rank along a file to find rooks
      for (int rank = 0; rank < 8; rank++)
      {
        if(PieceAtPosition((kingWhoCouldBeInCheck.File, rank)) == rookToLookFor)
        {
          return true;
        }
      }

      var bishopToLookFor = new Piece(otherColour, Type.Bishop);
      // Bishops are more akward, but it's the same rough principle, just with diagonals.
      // Find the upward slope starting square.

      // Up and to the right check. :TODO: i=0 is redudndant because that is the king, i=1 is more efficent.
      int i = 0;
      while(kingWhoCouldBeInCheck.File + i < 8 && kingWhoCouldBeInCheck.Rank + i < 8)
      {
        if (PieceAtPosition((kingWhoCouldBeInCheck.File + i, kingWhoCouldBeInCheck.Rank + i)) == bishopToLookFor)
        {
          return true;
        }
        i++;
      }
      i = 0;
      // Up and to the left check.
      while (kingWhoCouldBeInCheck.File - i >= 0 && kingWhoCouldBeInCheck.Rank + i < 8)
      {
        if (PieceAtPosition((kingWhoCouldBeInCheck.File - i, kingWhoCouldBeInCheck.Rank + i)) == bishopToLookFor)
        {
          return true;
        }
        i++;

      }
      i = 0;
      // Down and to the right check
      while (kingWhoCouldBeInCheck.File + i < 8 && kingWhoCouldBeInCheck.Rank - i >= 0)
      {
        if (PieceAtPosition((kingWhoCouldBeInCheck.File + i, kingWhoCouldBeInCheck.Rank - i)) == bishopToLookFor)
        {
          return true;
        }
        i++;

      }

      i = 0;
      // Down and to the left check
      while (kingWhoCouldBeInCheck.File - i >= 0 && kingWhoCouldBeInCheck.Rank - i >= 0)
      {
        if (PieceAtPosition((kingWhoCouldBeInCheck.File - i, kingWhoCouldBeInCheck.Rank - i)) == bishopToLookFor)
        {
          return true;
        }
        i++;
      }

      return false;
    }

    public Piece? PieceAtPosition(Square location)
    {
      // :NOTE: This returns a reference, is that ok?
      return myBoard[location.File, location.Rank];
    }

    public bool IsValidPosition()
    {
      // A board is valid if it has one white king and one black king.

      Square? whiteKing = FindKing(Colour.White);
      Square? blackKing = FindKing(Colour.Black);

      bool hasKings = whiteKing.HasValue && blackKing.HasValue;

      if (!hasKings) return false;

      bool duplicateOrMissingKings = myFen.Count('k') == 1 && myFen.Count('K') == 1;
      
      if(!duplicateOrMissingKings) return false;

      bool kingsTouch = Math.Abs(whiteKing!.Value.Rank - blackKing!.Value.Rank) <= 1 &&
                        Math.Abs(whiteKing!.Value.File - blackKing!.Value.File) <= 1;
      
      if (kingsTouch) return false;

      bool illegalPawns = mySplitFen[0].Contains('p') || mySplitFen[0].Contains('P') ||
                          mySplitFen[7].Contains('p') || mySplitFen[7].Contains('P');

      if (illegalPawns) return false;
      return true;
    }

  }
}
