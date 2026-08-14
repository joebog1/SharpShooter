using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;

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
            myBoard[file, rank] = new Piece(character);
            file++;
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

    private enum StopReasonCheckInADirection
    {
      Obstruction = 1, // Only stop if a peice other than what we are looking for is in the way
      CheckOnce = 2,   // Stop after one, regardless (useful for pawns and knights).
    }

    /// Looks in a specific direction for a peice. Expects either a Rook,Bishop or Queen peice to be provided.
    /// It is the callers responsibility to provide reasonable deltas (don't give a diagional delta for a rook check etc.)
    private bool CheckInADirection(Square KingsSqaure, int deltaFile, int deltaRank, Piece PeiceToLookFor, StopReasonCheckInADirection StopReason)
    {
      // i starts at one because i = 0 would reveal the king.
      for (int i = 1; i < 8; i++)
      {
        var positionToCheck = new Square(KingsSqaure.File + (i * deltaFile), KingsSqaure.Rank + (i * deltaRank));
        if (positionToCheck.Rank < 0 || positionToCheck.Rank >= 8 ||
            positionToCheck.File < 0 || positionToCheck.File >= 8)
        {
          break;
        }
        Piece? pieceAtPosition = PieceAtPosition(positionToCheck);
        if(pieceAtPosition != null)
        {
          // There is some other kind of peice in the way
          if (pieceAtPosition == PeiceToLookFor) return true;
          else if(StopReason == StopReasonCheckInADirection.Obstruction)
          {
            return false; // There is another peice in the way.
          }
        }
        if (StopReason == StopReasonCheckInADirection.CheckOnce) return false;
      }
      return false;
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

      StopReasonCheckInADirection obstruction = StopReasonCheckInADirection.Obstruction;

      // Queen checks
      var queenToLookFor = new Piece(otherColour, Type.Queen);
      if (CheckInADirection(kingWhoCouldBeInCheck, 1, 0, queenToLookFor, obstruction)) return true;
      if (CheckInADirection(kingWhoCouldBeInCheck, 0, 1, queenToLookFor, obstruction)) return true;
      if (CheckInADirection(kingWhoCouldBeInCheck, -1, 0, queenToLookFor, obstruction)) return true;
      if (CheckInADirection(kingWhoCouldBeInCheck, 0, -1, queenToLookFor, obstruction)) return true;
      if (CheckInADirection(kingWhoCouldBeInCheck, 1, 1, queenToLookFor, obstruction)) return true;
      if (CheckInADirection(kingWhoCouldBeInCheck, 1, -1, queenToLookFor, obstruction)) return true;
      if (CheckInADirection(kingWhoCouldBeInCheck, -1, -1, queenToLookFor, obstruction)) return true;
      if (CheckInADirection(kingWhoCouldBeInCheck, -1, 1, queenToLookFor, obstruction)) return true;
      // Rook checks.
      var rookToLookFor = new Piece(otherColour, Type.Rook);

      // Naively go through every file along the same rank to see if a rook of the opposite colour is there.
      if (CheckInADirection(kingWhoCouldBeInCheck, 1, 0, rookToLookFor, obstruction)) return true;
      if (CheckInADirection(kingWhoCouldBeInCheck, 0, 1, rookToLookFor, obstruction)) return true;
      if (CheckInADirection(kingWhoCouldBeInCheck, -1, 0, rookToLookFor, obstruction)) return true;
      if (CheckInADirection(kingWhoCouldBeInCheck, 0, -1, rookToLookFor, obstruction)) return true;

      // Bishop checks.
      var bishopToLookFor = new Piece(otherColour, Type.Bishop);
      if (CheckInADirection(kingWhoCouldBeInCheck, 1, 1, bishopToLookFor, obstruction)) return true;
      if (CheckInADirection(kingWhoCouldBeInCheck, 1, -1, bishopToLookFor, obstruction)) return true;
      if (CheckInADirection(kingWhoCouldBeInCheck, -1, -1, bishopToLookFor, obstruction)) return true;
      if (CheckInADirection(kingWhoCouldBeInCheck, -1, 1, bishopToLookFor, obstruction)) return true;

      // Knight checks
      // There are 8 possible cases ((+1,+2),(+2,+1),(+2,-1),(+1,-2),(-1,-2),(-2,-1),(-2,+1) and (-1,+2)).
      // We simply check if this square exists, if it does is there a knight of the opposite colour on it.
      var knightToLookFor = new Piece(otherColour, Type.Knight);
      var possibleOffsets = new (int, int)[] { (+1, +2), (+2, +1), (+2, -1), (+1, -2), (-1, -2), (-2, -1), (-2, +1), (-1, +2) };

      foreach ( var possibleOffset in possibleOffsets )
      {
        if (CheckInADirection(kingWhoCouldBeInCheck, possibleOffset.Item1, possibleOffset.Item2, knightToLookFor, StopReasonCheckInADirection.CheckOnce)) return true;
      }

      // Pawn checks
      // There are only two places to check, but it depends on the colour of the king.
      var pawnToLookFor = new Piece(otherColour, Type.Pawn);
      (int, int)[] offsetsToCheck;
      if (myTurn!.Value == Colour.White)
      {
        // Check (+1,+1) and (-1,+1);
        offsetsToCheck = new (int, int)[] { (+1, +1), (-1, +1) };
      }
      else
      {
        // Black king, check (+1,-1) and (-1,-1);
        offsetsToCheck = new (int, int)[] { (+1, -1), (-1, -1) };
      }
      foreach (var offsetToCheck in offsetsToCheck)
      {
        var positionToCheck = new Square(kingWhoCouldBeInCheck.File + offsetToCheck.Item1,
                                          kingWhoCouldBeInCheck.Rank + offsetToCheck.Item2);
        if (positionToCheck.Rank < 0 || positionToCheck.Rank >= 8 ||
            positionToCheck.File < 0 || positionToCheck.File >= 8)
        {
          continue;
        }
        var pieceAtPosition = PieceAtPosition(positionToCheck);
        if (pieceAtPosition == pawnToLookFor)
        {
          return true;
        }
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
