using System.Diagnostics;
using System.Globalization;

namespace SharpShooter
{
  using Square = (int Rank, int File);

  public class ChessEngine
  {
    public ChessEngine() { }

    string myBoardState;

    Square? myWhiteKingSquare;
    Square? myBlackKingSquare;

    public void SetPositionByFen(string fen)
    {
      myBoardState = fen;
      var splitFen = fen.Split(' ')[0].Split('/');
      // Now we have an array of strings. There should be 8 of them as there are 8 rows of a chessboard.
      Debug.Assert(splitFen.Length == 8);
      // :NOTE: consider mimicing 'real' chess coordinates by using 1-8 for rank and a-g for file.
      int rank = 0;
      int file = 0;
      foreach (var rankFen in splitFen)
      {
        foreach (char character in rankFen)
        {
          if (char.IsDigit(character))
          {
            int emptySpaces = character - '0';
            file += emptySpaces;
          }
          else 
          {
            if (character == 'K')
            {
              myWhiteKingSquare = (Rank: rank, File: file);
            }
            else if (character == 'k')
            {
              myBlackKingSquare = (Rank: rank, File: file);
            }
            file++;
          }
        }
        rank++;
        // Reset which file we are on.
        file = 0;
      }
    }

    public bool IsValidPosition()
    {
      // A board is valid if it has one white king and one black king.
      bool hasKings = myWhiteKingSquare.HasValue && myBlackKingSquare.HasValue;

      if (!hasKings) return false;

      bool kingsTouch = Math.Abs(myWhiteKingSquare!.Value.Rank - myBlackKingSquare!.Value.Rank) <= 1 &&
                        Math.Abs(myWhiteKingSquare!.Value.File - myBlackKingSquare!.Value.File) <= 1;

      return !kingsTouch;
    }
  }
}
