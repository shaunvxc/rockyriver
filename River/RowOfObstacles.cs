using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * 
 * Represents a row of obstacles, a list of these objects is used by RiverObstacleManger
 * 
 * @Author Shaun Viguerie
 * 
 */
public class RowOfObstacles
{

		private RiverObstacle[] _obstacles;
		private int _activeIndex;

		private List<RiverObstacle> obstacles;
		private int rowNumber;
		private int ClearedIndex;
		private int maxClearedTop;
		private int minClearedTop;
		private int maxClearedBottom;
		private int minClearedBottom;
		private List<int> clearedAtBottomIndices;
		private List<int> clearedAtTopIndices;
		private int arrayOffset = 0;
		
		
		
		public RowOfObstacles ()
		{				

				_obstacles = new RiverObstacle[9];
				_activeIndex = 0;

				obstacles = new List<RiverObstacle> ();
				clearedAtBottomIndices = new List<int> ();
				clearedAtTopIndices = new List<int> ();

				maxClearedBottom = -1;
				minClearedBottom = 10;
					

				maxClearedTop = -1;
				minClearedTop = 10;
		}
		

		public void setVisibleRocksInDirection (int numRocks, bool rightOnTop, bool rightOnBottom)
		{ 
						if (rightOnTop) {
							
								if (maxClearedTop < _obstacles.Length - 2) { // was 2
										setClearedTopAtIndex (maxClearedTop + 1);
								
								} else { 
										setClearedTopAtIndex (minClearedTop - 1);
								}
							
						} else {

								if (minClearedTop > 1) {
										setClearedTopAtIndex (minClearedTop - 1);
								} else {
										setClearedTopAtIndex (maxClearedTop + 1);
								}
						}		


						if (rightOnBottom) {
							
								if (maxClearedBottom < _obstacles.Length - 2) {  // was 2
 										setClearedBottomAtIndex (maxClearedBottom + 1);
								} else { 
										setClearedBottomAtIndex (minClearedBottom - 1);
								}
							
						} else {
							
								if (minClearedBottom > 1) {
										setClearedBottomAtIndex (minClearedBottom - 1);
								} else {
										setClearedBottomAtIndex (maxClearedBottom + 1);
								}
						}		

		}
		



		/**	
		 * Add an obstacle to the row
		 */
		public void addObstacle (RiverObstacle obstacle)
		{
				//obstacles.Add (obstacle);						

				_obstacles [_activeIndex] = obstacle;
				_activeIndex++;
		}

		public  RiverObstacle[] getObstacles ()  // List<RiverObstacle>
		{ 


				return _obstacles;
		}

		/**
		 * Shift the obstacles in the row up by the given number of rows
		 */
		public void shiftRowUp (int numRowsToShift)
		{		/*
				foreach (RiverObstacle obstacle in obstacles) {
						obstacle.moveObstacleUp (numRowsToShift);
				}
				*/
				for(int i = 0; i < _obstacles.Length; i++) {
					_obstacles[i].moveObstacleUp(numRowsToShift);
				}

				
		}
		
		/**
		 * Shift the obstacles in the row down by the given number of rows
		 */
		public void shiftRowDown (int numRowsToShift)
		{

				/*
				foreach (RiverObstacle obstacle in obstacles) {
						obstacle.moveObstacleDown (numRowsToShift);
				}	
				*/

				for(int i = 0; i < _obstacles.Length; i++) {
					_obstacles[i].moveObstacleDown(numRowsToShift);
				}
  		}
  
  /**
		 * Deletes all obstacles in the row
		 */
		public void deleteRow ()
		{ 

		/*	foreach (RiverObstacle obstacle in obstacles) {
						obstacle.DeleteObstacle ();
				}	
		*/
				for(int i = 0; i < _obstacles.Length; i++) {
					_obstacles[i].DeleteObstacle();
				}
  		}
  
		public float getFirstPositiveXPosition ()
		{
			/*
				foreach (RiverObstacle obstacle in obstacles) {
						if (obstacle.getBaseXPosition () > 0) {
								return obstacle.getBaseXPosition ();	
						}
				}
				*/

				for(int i = 0; i < _obstacles.Length; i++) {
					if(_obstacles[i].getBaseXPosition() > 0) {
						return _obstacles[i].getBaseXPosition();
					}
				}
				Debug.Log ("Returning 0!! : " + _obstacles.Length);
   				 return 0F;
		}

		

		/**
		 * Expose the size of the row to RiverManager for use during obstacle Generation
 		 */
		public int getNumObstaclesInRow ()
		{
				return _obstacles.Length;
		}
		
		public float getTopRockYCoord ()
		{
				if (_obstacles.Length > 0) {
						return _obstacles [0].getTopRockYCoord ();
				}
				return -1F;
		}

		public float getBottomRockYCoord ()
		{
				if (_obstacles.Length > 0) {
						return _obstacles [0].getBottomRockYCoord ();
				}
				return -1F;
		}

		/**
		 * Get the level of the obstacles in the row
		 */
		public float getYCoord ()
		{ 
				
				if (_obstacles.Length > 0) {
						return _obstacles [0].getYCoord ();
				}
			
				Debug.LogError ("Error, querying obstacles y coordinate on empty obstacle list");
				return -1F;	
		}
		
		
		/**
		 * Clears both top and bottom obstacles at the given index
		 */
		public void clearAtIndexOnly (int index)
		{
		
				index += arrayOffset;
		
				if (index < _obstacles.Length) {
			
						_obstacles [index].clearObstacles ();
						setClearedTopAtIndex (index);
						setClearedBottomAtIndex (index);
						ClearedIndex = index;
				}
		}


		public void clearRange(int startIndex, int endIndex) {
			
				for (int i = startIndex; i < endIndex; i++) {
					clearAtIndexOnly(i);
				}
			
		}


		public void makeEdgeRow() { 
			
				for(int i = 1; i < _obstacles.Length -1; i++) { 
					clearAtIndexOnly(i);
				}
			
		}

		
		/**
		 * Clears the top rock at the given index
		 */
		public void clearTopRockAtIndex (int index)
		{
			
				index += arrayOffset;
			
				if (index < _obstacles.Length) {
						
						_obstacles [index].clearTopRock ();
						setClearedTopAtIndex (index);	
				}
		}
			
		public float getClearedPositionInRiver() { 
			return _obstacles[ClearedIndex].getBaseXPosition();
		}
	
		/**
		 * Clears the bottom rock at the given index
		 */
		public void clearBottomRockAtIndex (int index)
		{
			
				index += arrayOffset;
			
				if (index < _obstacles.Length) {
								
						_obstacles [index].clearBottomRock ();

						setClearedBottomAtIndex (index);
				}
		}
		

		

		public void setClearedTopAtIndex (int index)
		{ 

				if (index < minClearedTop) { 

						minClearedTop = index;
				}

				if (index > maxClearedTop) { 

						maxClearedTop = index;		
				}
			

				clearedAtTopIndices.Add (index);
		}

		public void setClearedBottomAtIndex (int index)
		{



				if (index < minClearedBottom) { 
				
						minClearedBottom = index;
				}
			
				if (index > maxClearedBottom) { 	
						maxClearedBottom = index;		
				}

				clearedAtBottomIndices.Add (index);
		}
		
		public bool isClearedTopIndex (int index)
		{
			
				return clearedAtTopIndices.Contains (index);
		}

		public bool isClearedBottomIndex (int index)
		{
				return clearedAtBottomIndices.Contains (index);
		}

		public List<int> getClearedTopIndices ()
		{ 
				return clearedAtTopIndices;
		}

		public List<int> getClearedBottomIndices ()
		{ 
				return clearedAtBottomIndices;
		}

}

