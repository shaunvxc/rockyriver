using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 
 * (C) %SelloutSystems
 * 
 * 
 * @Author Shaun Viguerie
 */
public class MoveSequenceEnhanced : IMoveSequence
{
		private int totalMoveCount = 0;
		private int moveCounter = 0;
		private List<int>  moves;
		private int previousMove;
		private int currentMove;
		private int startPosition;
		private int baseMove;

		private int lastZigDirection;
		private int numObstaclesPerRow;
		private int leftBound;
		private int rightBound;

		public MoveSequenceEnhanced (int startPosition, int numObstacles)
		{
				moves = new List<int> ();
				numObstaclesPerRow = numObstacles;
				init (startPosition);
		}
		
		public void resetSequence (int startPosition)
		{
				moves.Clear ();
				init (startPosition);
		}
		
		public void clearAndRestartSequence ()
		{ 
				int currentExit = getExitPosition ();
				//Debug.Log ("Cleaning & Restarting Sequence: " + printSequence() + " exit position is: " + currentExit);
				moves.Clear ();
				init (currentExit);  // stitch together the new Sequence starting at the ending point of the old sequence
		}

		public int getLastZigDirection ()
		{ 
				return lastZigDirection;
		}
		
		public int peek ()
		{ 
			
				if (moveCounter + 1 < moves.Count) { 
						return moves [moveCounter + 1];
				}

				return 0;
		}

		private void init (int startPosition)
		{ 
				leftBound = 1;
				rightBound = numObstaclesPerRow - 1;
				moveCounter = 0;
				totalMoveCount = 0;
				this.startPosition = startPosition;
				baseMove = startPosition;
				previousMove = startPosition;
				currentMove = startPosition;
		}

		#region IMoveSequence implementation

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

		public int getNumTotalMovesInSequence ()
		{
				return totalMoveCount;
		}

		public int getNumMovesMade ()
		{ 
				return moveCounter;
		}

		public bool hasMoreMoves ()
		{
				return moveCounter <= moves.Count - 1;
		}

		public int getExitPosition ()
		{
				if (moves.Count == 0) {
						return startPosition;	
				}
			
				return moves [totalMoveCount - 1];
		}
		
		#endregion
		
		public string printSequence ()
		{
				string s = "";
			
				foreach (int move in moves) {
						s += (move + ", ");
				}

				return s;
		}
		
		public void Zig (int direction, int numMoves)
		{	 
				for (int i = 0; i < numMoves; i++) {
						zigInDirection (direction);
				}
		}

		private void zigInDirection (int direction)
		{
				if (direction == DirectionalCodes.Right) {
						zigRight ();
				} else if (direction == DirectionalCodes.Left) {
						zigLeft ();
				}
		}
	
		private void zigLeft ()
		{ 			
				if (baseMove > leftBound) {        // previously was baseMove > 0
						baseMove -= 1;
						moves.Add (baseMove);
						totalMoveCount++;
						lastZigDirection = DirectionalCodes.Left;
				} else {	
						Debug.Log ("Error: Cannot Zig to left! BaseMove = " + baseMove + " leftBound = " + leftBound + ", " + printSequence());
						zigRight ();
				}
		}
		
		private void zigRight ()
		{
				if (baseMove < rightBound) {  // previously was baseMove < numObstaclesPerRow
						baseMove += 1;
						moves.Add (baseMove);
						totalMoveCount++;
						lastZigDirection = DirectionalCodes.Right; 
				} else {
						Debug.Log ("Error: Cannot Zig to right! BaseMove = " + baseMove + " rightBound = " + rightBound + ", " + printSequence());
						zigLeft ();
				}
		}

		public int getNumObstacles ()
		{ 
				return numObstaclesPerRow;	
		}

		public void AlternateWDirection (int numMoves, int startDirection)
		{ 			

				if (startDirection == DirectionalCodes.Right && getExitPosition () == rightBound) { // previously was getExitPosition() == numObstaclesPerRow
						startDirection = DirectionalCodes.Left;
				} else if (startDirection == DirectionalCodes.Left && getExitPosition () == leftBound) { // previously was getExitPosition() == 0
						startDirection = DirectionalCodes.Right;
				} 
			
				for (int i = 0; i < numMoves; i++) {
						Zig (startDirection, 1);
						startDirection *= -1;
				}
		}

		

		public void Alternate (int numMoves)
		{ 
				int startDirection = lastZigDirection * -1;
				if (startDirection == 0) {
						startDirection = getRandomSafeDirection ();
				}
				for (int i = 0; i < numMoves; i++) {
						Zig (startDirection, 1);
						startDirection *= -1;
				}
		}

		private int getRandomSafeDirection ()
		{ 
			
				if (getExitPosition () <= leftBound + 1) { // previously was == 0
						return DirectionalCodes.Right;
				} else if (getExitPosition () >= rightBound - 1) {  // previously was == 7
						return DirectionalCodes.Left;
				} else {
						if (UnityEngine.Random.Range (0, 10) <= 5) {
								return DirectionalCodes.Left;
						} else {
								return DirectionalCodes.Right;
						}
				}

		}

		public void fallback ()
		{
				moves.Add (-1);
				totalMoveCount++;
		}
}
