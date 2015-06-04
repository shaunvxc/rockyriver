
using UnityEngine;
using System;
using System.Collections;

/**
* (C) %SelloutSystems
* 
*  most up-to-date path generation implementation
*
* @Author Shaun Viguerie
* 
*/
public class ObstaclePathGeneratorV3 : IPathGenerator
{
		private int numObstaclesPerRow;
		private int numTotalMoves;
		private int previousMove;

		/*  this implementation usies a MoveSequence enhanced*/
		private MoveSequenceEnhanced currentMoveSequence;
		/* set to true to print out log statements */
		private bool debug = false;
		

		public ObstaclePathGeneratorV3 (int numObstacles)
		{
				numObstaclesPerRow = numObstacles;		
				numTotalMoves = 0;

				initBaseSequences ();
		}

		private void initBaseSequences ()
		{		
				currentMoveSequence = GenerateInitialMoveSequence (); // generate a move with a random entry point
		}
		
		public int getNext ()
		{ 
		
				int ret;

				if (currentMoveSequence.hasMoreMoves ()) {

						if (currentMoveSequence.getCurrentMove () != -1) {
								previousMove = currentMoveSequence.getCurrentMove ();
						}
			
						ret = currentMoveSequence.getNext ();
				} else {
			
						if (currentMoveSequence.getCurrentMove () != -1) {
								previousMove = currentMoveSequence.getCurrentMove ();
						}

						clearAndUpdateSequence (currentMoveSequence);
						ret = currentMoveSequence.getNext ();
				}
			
				numTotalMoves++;
				return ret;
		}
	
		private void clearAndUpdateSequence (MoveSequenceEnhanced sequence)
		{ 

				int currentMove = sequence.getExitPosition ();
				sequence.clearAndRestartSequence ();
				if (UnityEngine.Random.Range (0, 100) <= 91) { // && numTotalMoves % 2 != 0) {  // the numTotalMoves % 2 case can be removed

						previousMove = currentMove;
						int rand = UnityEngine.Random.Range (0, 100);

						if (rand % 2 == 0) { 	

								if (rand <= 25) { 	
										SequenceBuilder.classicFallbackZigEntryZigExit (sequence);
										Log ("classicFallbackZigEntryZigExit(): " , sequence);
								} 
								else if (rand <= 50) { 
										SequenceBuilder.classicFallbackAltEntryZigExit (sequence);
										Log ("classicFallbackAltEntryZigExit(): " , sequence);
								} 
								else if (rand <= 75) { 
										SequenceBuilder.classicFallbackAltEntryAltZigExit (sequence);
										Log ("classicFallbackAltEntryAltZigExit(): " , sequence);
								} 
								else { 
										SequenceBuilder.classicFallbackZigEntryAltZigExit (sequence);
										Log ("classicFallbackZigEntryAltZigExit(): " , sequence);
								}
					
						} else {
					
								if (rand <= 25) { 
										SequenceBuilder.typeAFallbackZigEntryZigExit (sequence);
										Log ("typeAFallbackZigEntryZigExit(): " , sequence);
								} else if (rand <= 50) { 
										SequenceBuilder.typeAFallbackAltEntryZigExit (sequence);
										Log ("typeAFallbackAltEntryZigExit(): " , sequence);
								} else if (rand <= 75) { 
										SequenceBuilder.typeAFallbackZigEntryAltZigExit (sequence);	
										Log ("typeAFallbackZigEntryAltZigExit(): " , sequence);

								} else {
										SequenceBuilder.typeAFallbackAltEntryAltZigExit (sequence);
										Log ("typeAFallbackAltEntryAltZigExit(): " , sequence);
								}
						}

						
						if (UnityEngine.Random.Range (0, 100) <= 5) {
								SequenceBuilder.backToBackFallback (sequence);
								Log ("backToBackFallback(): " , sequence);
						} 
					
				} 
				else {
						SequenceBuilder.ZigTwoByTwos (sequence);
						Log ("ZigTwoByTwos(): " , sequence );
				}
		}


		private MoveSequenceEnhanced GenerateInitialMoveSequence ()
		{			
				MoveSequenceEnhanced sequence = new MoveSequenceEnhanced (4, numObstaclesPerRow);
				sequence.Alternate (4); // 3
				
			//	sequence.Zig (getRandomStart(), 2);
						
				return sequence;
		}

		public bool isNewSequence ()
		{
				if (currentMoveSequence.getNumMovesMade () == 1) {
						return true;
				}
		
				return false;
		}

		public int peek ()
		{ 
				return currentMoveSequence.peek ();
		}

		public int getPreviousMove ()
		{
				return previousMove;
		}
	
		public int getCurrentMove ()
		{
				return currentMoveSequence.getCurrentMove ();
		}
	
		public int getNumTotalMoves ()
		{
				return numTotalMoves;
		}

		public void resetMoves ()
		{
				numTotalMoves = 0;
		}
		
		private int getRandomStart() { 
			if (UnityEngine.Random.Range (0, 100) <= 50) {
						return -1;
				} else {
						return 1;
				}
		}
		

		private void Log(string logMessage, MoveSequenceEnhanced sequence) {
			
			if (debug) { 
				Debug.Log (logMessage + sequence.printSequence ());
			}
		}

}


