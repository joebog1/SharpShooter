using System;

class Test
{
    //test for compiling c# with multiple files
    /*
     
        FEN postiions for testing here:
        Starting Position= rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1
        Tactics 1 = 2kr3r/p2b2p1/3b1p2/6p1/1nPP4/N5P1/PP3P1P/R1B1R1K1 b - - 2 19
     */
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
        testStartingPos.WhiteRooks   = 0b10000001_00000000_00000000_00000000_00000000_00000000_00000000_00000000;

        testStartingPos.BlackKing    = 0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_00001000;
        testStartingPos.BlackBishops = 0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_00100100;
        testStartingPos.BlackKnights = 0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_01000010;
        testStartingPos.BlackPawns   = 0b00000000_00000000_00000000_00000000_00000000_00000000_11111111_00000000;
        testStartingPos.BlackQueens  = 0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_00010000;
        testStartingPos.BlackRooks   = 0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_10000001;

    }
}