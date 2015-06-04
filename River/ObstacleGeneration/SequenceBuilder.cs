using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/**
* (C) %SelloutSystems
*
* This class contains static methods that generate numeric string representations of all possible
* obstacles in the game (Per Marc's work).  Sequeunces of obstacles can be "stitched" together simply
* by calling several of the below methods successively on a single MoveSequenceEnhanced object.
* 
* @Author Shaun Viguerie & Marc Bertucci
*/
public class SequenceBuilder
{

		// @ --- use an EnumType for directional bearings instead of just an integer. 
		
		private static int Left = DirectionalCodes.Left;
		private static int Right = DirectionalCodes.Right;



		/**
		 * 
		 * 
		 */
		public static void ZigBackAndForth (MoveSequenceEnhanced sequence, int startPosition)
		{
				int bound = sequence.getNumObstacles () / 2;
		
				if (startPosition < bound) {
						sequence.Zig (Right, UnityEngine.Random.Range (2, sequence.getNumObstacles () - startPosition)); 		
				} else {
						sequence.Zig (Left, UnityEngine.Random.Range (2, startPosition)); 	
				}
		}

		public static void ZigTwoByTwos (MoveSequenceEnhanced sequence)
		{
		
				ZigToPoint (sequence, sequence.getExitPosition (), 4);		
					
				for (int i = 0; i < 2; i++) {
			
						int numMoves = 2;
						if (UnityEngine.Random.Range (0, 10) <= 3) {
								numMoves++;
						} 
			
						if (i % 2 == 0) { 
								sequence.Zig (Right, numMoves);
						} else {
								sequence.Zig (Left, numMoves);
						}
				}
		}
	
		

		


		/**
		 * this has been approved
		 */ 
		public static void classicFallbackZigEntryZigExit (MoveSequenceEnhanced sequence)
		{ 
			
				int direction; // = getDirectionalBearing (sequence.getExitPosition (), 6);

				if (sequence.getExitPosition () <= 4) { 
						direction = Right;
						ZigToPoint (sequence, sequence.getExitPosition (), 1);			
				} else {

						direction = Left;
						ZigToPoint (sequence, sequence.getExitPosition (), 7); // was 7	
				}
				/*		
				if (direction == Right ) {
					ZigToPoint (sequence, sequence.getExitPosition(), 1); // was 0
				} 
				else if (direction == Left ) {
					ZigToPoint (sequence, sequence.getExitPosition(), 7); // was 7	
				}
		*/
				sequence.Zig (direction, 2);
				sequence.fallback ();
				sequence.Zig (direction, 3);
		}
		

		/**
		 * pretty sure this is also confirmed
		 */
		public static void classicFallbackZigEntryAltZigExit (MoveSequenceEnhanced sequence)
		{ 
			
				int direction = getDirectionalBearing (sequence.getExitPosition (), 4);
				if (sequence.getExitPosition () >= 4) {	
						ZigToPoint (sequence, sequence.getExitPosition (), 5);
						direction = Left;
					
				} else {
						ZigToPoint (sequence, sequence.getExitPosition (), 3);
						direction = Right;
				}
				
				
//				direction = DirectionalCodes.Left; // TODO what is this?!?!?!

				//	ZigToPoint (sequence, sequence.getExitPosition(), 5);  // wiith zig to 5 does not work, with zig to 6 it does??
				
				sequence.Zig (direction, 2);
				sequence.fallback ();
				sequence.Zig (direction, 1); // the alternate here is actually handled by two calls to Zigh different directions. 
				sequence.Zig (direction * -1, 2); 
		}
	
		/**
		 * This has been confirmed
		 */
		public static void classicFallbackAltEntryZigExit (MoveSequenceEnhanced sequence)
		{ 

				int direction = getDirectionalBearing (sequence.getExitPosition (), 5);
				
				if (direction == DirectionalCodes.Right && sequence.getExitPosition () >= 5) {
						direction = DirectionalCodes.Left;
				} else if (direction == DirectionalCodes.Left && sequence.getExitPosition () <= 2) {
						direction = DirectionalCodes.Right;
				}
				
				sequence.AlternateWDirection (getNumAlts (), direction * -1);
				sequence.fallback ();
				sequence.Zig (direction, 3);	
		}
		

		/**
		 * Confirmed & working
		 */ 
		public static void classicFallbackAltEntryAltZigExit (MoveSequenceEnhanced sequence)
		{ // int startMove) { 
		
			
				int direction  = getDirectionalBearing (sequence.getExitPosition (), 4);
				//sequence.AlternateWDirection (getNumAlts(), direction);
					/*
				if (sequence.getExitPosition () >= 4) {	
					ZigToPoint (sequence, sequence.getExitPosition (), 5);
					direction = Left;
			
				} else {
					ZigToPoint (sequence, sequence.getExitPosition (), 3);
					direction = Right;
				} */


				sequence.AlternateWDirection (3, direction);
				sequence.fallback ();
//				sequence.AlternateWDirection (1, direction);
				sequence.Zig (direction, 1);
				sequence.Zig (direction * -1, 2); 
		}

		/**
		 * This has been confirmed and is working
		 */
		public static void typeAFallbackZigEntryZigExit (MoveSequenceEnhanced sequence)
		{  // works with startMove = 3!!
				//  and MoveSequenceCreatedWith start Point 0
				int randStart = UnityEngine.Random.Range (3, 4);	
				ZigToPoint (sequence, sequence.getExitPosition (), randStart);	
				
				int direction = getDirectionalBearing (sequence.getExitPosition (), 4);
				sequence.Zig (direction, 2);
				sequence.fallback ();
				sequence.Zig (direction * -1, 3);
		}

		/*** 
		 * 
		 * pretty sure this is working but keep an eye on it
		 */
		public static void typeAFallbackZigEntryAltZigExit (MoveSequenceEnhanced sequence)
		{ 
			
				int randStart = UnityEngine.Random.Range (3, 4);			
				ZigToPoint (sequence, sequence.getExitPosition (), randStart);
				int direction = getDirectionalBearing (sequence.getExitPosition ());

				sequence.Zig (direction, 2);
				sequence.fallback ();
				sequence.Zig (direction * -1, 1);
				sequence.Zig (direction, 2);

		}
		
		/**
		 * This is confirmed
		 */
		public static void typeAFallbackAltEntryZigExit (MoveSequenceEnhanced sequence)
		{ // works with a start move of 4....?
				
				int randStart = UnityEngine.Random.Range (3, 4);			

				ZigToPoint (sequence, sequence.getExitPosition (), randStart);
				
				int direction; //= getDirectionalBearing (sequence.getExitPosition (), 5);
			
				if (randStart == 3) { 
						direction = DirectionalCodes.Right;
				} else {
						direction = DirectionalCodes.Left;
				}

				sequence.AlternateWDirection (getNumAlts (), direction);
				sequence.fallback ();
				sequence.Zig (direction * -1, 3);
		}

		public static void typeAFallbackAltEntryAltZigExit (MoveSequenceEnhanced sequence)
		{
			
				int randStart = UnityEngine.Random.Range (3, 4);			

				ZigToPoint (sequence, sequence.getExitPosition (), randStart);
		
				int direction = getDirectionalBearing (sequence.getExitPosition ());
				sequence.AlternateWDirection (getNumAlts (), direction);
				sequence.fallback ();
				sequence.Zig (direction * -1, 1);
				sequence.Zig (direction, 2);
		}
		
		private static int getNumAlts ()
		{ 

				
				if (UnityEngine.Random.Range (0, 100) <= 5) { 
						return 3;
				}

				return 3;
			
		}

		public static void backToBackFallback (MoveSequenceEnhanced sequence)
		{ 
				int direction = getDirectionalBearing (sequence.getExitPosition (), 7);

				if (direction == DirectionalCodes.Left) { 
						ZigToPoint (sequence, sequence.getExitPosition (), 7);
				} else { // right directions
						ZigToPoint (sequence, sequence.getExitPosition (), 1); // was 0 
				} 
			
				sequence.Zig (direction, 2);
			
				sequence.fallback ();
				sequence.Zig (direction, 1);
				sequence.fallback ();
				sequence.Zig (direction, 1);					
				sequence.Zig (direction, 2);					
		}

		private static void ZigToPoint (MoveSequenceEnhanced sequence, int startMove, int desiredPoint)
		{
				// could have just called Sequence.getExitPosition() directly here
				// instead of passing in startMove....

				if (startMove < desiredPoint) {
						sequence.Zig (Right, desiredPoint - startMove);
				} else if (startMove > desiredPoint) {
						sequence.Zig (Left, startMove - desiredPoint);
				}


		}

		public static int getDirectionalBearing (int currentPosition, int sequenceWidth)
		{ 

				if (currentPosition + sequenceWidth < 8) { // was 7 
						return DirectionalCodes.Right;
				} else if (currentPosition - sequenceWidth > 1) { // was 0 
						return DirectionalCodes.Left;
				} else {
					
						if (UnityEngine.Random.Range (0, 100) <= 50) { 
								return DirectionalCodes.Right;
						} else {
								return DirectionalCodes.Left;
						}
				}
	
	
		}

		/**
		 * use shared method rather than do this everywhere...although 
		 * it could be a tad more general..
		 */
		public static int getDirectionalBearing (int currentPosition)
		{ 		
				
				if (currentPosition <= 3) {
						return DirectionalCodes.Right;
				} else if (currentPosition >= 5) {   // this was 6 before I changed it on the train to match the classic fallback
						return DirectionalCodes.Left;
				} else {
					
						if (UnityEngine.Random.Range (0, 100) <= 50) { 
								return DirectionalCodes.Right;
						} else {
								return DirectionalCodes.Left;
						}
				} 
		}
}


