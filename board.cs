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

arbitrarily i define the a1 square as the 1st digit and h8 as the 64th digit, 
going row wise this would be 1-8 on the 1st row, and 9 would be a2 and thus 
9-16 would be a2-h2 etc.

Thus a position is just the culmination of these bitboards.
*/
