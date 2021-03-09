/*
this file should handle the board representation

the board will be represented using bitboards,
ie UInt64's that represent the postions of each square.
we will have a bitBoard for each of the following:

WhiteKing
WhiteQueens
WhiteKnights  lol
WhiteBishops
WhiteRooks
WhitePawns

BlackKing
BlackQueens
BlackKnights
BlackBishops
BlackRooks
BlackPawns

Arbitrarily i define the a1 square as the 1st digit from the left and h8 as the 64th digit, 
going row wise this would be 1-8 on the 1st row, and 9 would be a2 and thus 
9-16 would be a2-h2 etc.

Thus a position is just the culmination of these bitboards.
*/
using System;

public class position
{
    /*
    unsure as to how best to describe bitboard/bitsets
    can consider using an enum of color to half the ammount of bit boards
    or even go the whole way and have an enum for each piece and have 
    2 bitboards one for white and one for black. idk
    */
    UInt64 WhiteKing;
    UInt64 WhiteQueens;
    UInt64 WhiteKnights;
    UInt64 WhiteBishops;
    UInt64 WhiteRooks;
    UInt64 WhitePawns;
    

    UInt64 BlackKing;
    UInt64 BlackQueens;
    UInt64 BlackKnights;
    UInt64 BlackBishops;
    UInt64 BlackRooks;
    UInt64 BlackPawns;
    
    String bitsToFEN()
    {
        //convert bitboards to FEN notation. going to have to figure out what FEN is though
        if(WhiteKing==0 || BlackKing==0)
        {
            //no kings in the postion, illegal FEN
            return "";
        }
    }
    
}

class Test
{

    static public void Main(String[] args)
    {
        Console.WriteLine("lookie here");

    }
}