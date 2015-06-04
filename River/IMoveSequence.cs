
using System;

public interface IMoveSequence {
		
	int getNext();
	int getLastMove();

	int getNumTotalMovesInSequence();
	int getNumMovesMade();
	bool hasMoreMoves();

}


