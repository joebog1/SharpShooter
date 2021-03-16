using System;

class Test
{
    //test for compiling c# with multiple files
    static public void Main(string[] args)
    {
        Console.WriteLine("lookie here");
        Chessboard.position testStartingPos=new Chessboard.position();
        //we make starting position and run assert on converting to correct FEN
        testStartingPos.WhiteKing    = 0b00001000_00000000_00000000_00000000_00000000_00000000_00000000_00000000;
        testStartingPos.WhiteBishops = 0b00100100_00000000_00000000_00000000_00000000_00000000_00000000_00000000;
        testStartingPos.WhiteKnights = 0b01000010_00000000_00000000_00000000_00000000_00000000_00000000_00000000;
        testStartingPos.WhitePawns   = 0b00000000_11111111_00000000_00000000_00000000_00000000_00000000_00000000;
        testStartingPos.WhiteQueens  = 0b00010000_00000000_00000000_00000000_00000000_00000000_00000000_00000000;

    }
}