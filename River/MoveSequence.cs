using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/**
 * (C) %SelloutSystems
 * 
 * 
 * @Author Shaun Viguerie
 */
public class MoveSequence : IMoveSequence
{
		private int totalMoveCount = 0;
		private int moveCounter = 0;
		private List<int>  moves;
		private int previousMove;
		private int currentMove;
		private int startPosition;
		private int baseMove;
		
		public MoveSequence (int startPosition)
		{
			moves = new List<int> ();	
			init (startPosition);
		}

		private void init(int startPosition) { 
			moveCounter = 0;
			totalMoveCount = 0;
			baseMove = startPosition;
			previousMove = startPosition;
		}

		/**
		 * 
		 * reset method allows us to not have to constantly be creating new
		 * objects... 
		 */
		public void resetSequence(int startPosition) { 
			moves.Clear ();
			init (startPosition);
		}

		#region IMoveSequence implementation



		public void moveLeftOrRight (int direction)
		{
				if (direction == 1) {
						right ();
				} else if (direction == -1) {
						left ();
				}
		}

		/**
		 * 	allows appending movesequences together to create
		 * more complex paths. 
		 */
		public void appendMoveSequence (MoveSequence sequence)
		{
				foreach (int move in sequence.getMoves()) { 
						moves.Add (move);
				}
		}

		public List<int> getMoves ()
		{
				return moves;    // shouldn't keep this public!!!
		}

		public string printSequence ()
		{
		
				string s = "";
			
				foreach (int move in moves) {
						s += (move + ", ");
				}
				
				return s;
		}

		public void left ()
		{ 

				baseMove -= 1;
				moves.Add (baseMove);
				totalMoveCount++;
		}

		public void right ()
		{
				baseMove += 1;
				moves.Add (baseMove);
				totalMoveCount++;
		}

		public void fallback ()
		{
				moves.Add (-1);
				totalMoveCount++;
		}

		public bool hasMoreMoves ()
		{ 
				return moveCounter <= moves.Count - 1;
		}

		public int getNext ()
		{
				
				if (currentMove != -1 && moveCounter != 0) {			
						previousMove = currentMove;
				}

				currentMove = moves [moveCounter];
				moveCounter++;

				return currentMove;

		}

		public int getLastMove ()
		{
				return previousMove;
		}

		public int getCurrentMove ()
		{
				return currentMove;
		}

		public int getNumMovesMade ()
		{ 
				return moveCounter;
		}

		public int getExitPosition ()
		{
				if (moves.Count == 0) {
						return startPosition;	
				}
				
				return moves [totalMoveCount - 1];
		}

		public int getNumTotalMovesInSequence ()
		{
				return totalMoveCount;
		}

		#endregion
}

