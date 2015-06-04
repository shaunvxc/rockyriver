/**
* (C) %SelloutSystems
*
* expose interface methods for path generation..
*
* @Author Shaun Viguerie
* 
*/
using System;

public interface IPathGenerator
{

	int getNext();
	int getCurrentMove();
	int getPreviousMove();
	int getNumTotalMoves();

	bool isNewSequence();

	int peek();

	void resetMoves(); // ?? do we still need this?
}


