using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RowOfLandObstacles
{


		private LandObstacle[] _obstacles;
		private int _activeIndex;
		private List<LandObstacle> obstacles;
		private int rowNumber;
		private List<int> clearedIndices;

		public RowOfLandObstacles ()
		{				

				_obstacles = new LandObstacle[18];
				_activeIndex = 0;

				obstacles = new List<LandObstacle> ();
				clearedIndices = new List<int> ();
		}

		public int getNumLandTiles ()
		{ 
				return obstacles.Count;
		}

		public float getYCoord ()
		{ 
			
				if (_obstacles [0] != null) {
						return _obstacles [0].getYCoord ();
				}

				/*	
			if (obstacles != null && obstacles.Count >= 1) {
				return obstacles[0].getYCoord();
			}
		*/
				return -1;
		}

		/**	
		 * Add an obstacle to the row
		 */
		public void addObstacle (LandObstacle obstacle)
		{
//		obstacles.Add (obstacle);						

				_obstacles [_activeIndex] = obstacle;
				_activeIndex++;
		}

		public void addclearedIndex (int idx)
		{
				clearedIndices.Add (idx);
		}

		public bool isClearedIndex (int idx)
		{
				return clearedIndices.Contains (idx);
		}


		/**
	 * Shift the obstacles in the row up by the given number of rows
	 */
		public void shiftRowUp (int numRowsToShift)
		{
				/*
		foreach (LandObstacle obstacle in obstacles) {
			obstacle.moveObstacleUp (numRowsToShift);
		} */	

				for (int i = 0; i < _obstacles.Length; i++) {
						if (_obstacles [i] != null) {
								_obstacles [i].moveObstacleUp (numRowsToShift);
						}
				}

		}
	
		/**
		 * Shift the obstacles in the row down by the given number of rows
		 */
		public void shiftRowDown (int numRowsToShift)
		{
				/*
		foreach (LandObstacle obstacle in obstacles) {
			obstacle.moveObstacleDown (numRowsToShift);
		}	
		*/
				for (int i = 0; i < _obstacles.Length; i++) {
						if (_obstacles [i] != null) {
								_obstacles [i].moveObstacleDown (numRowsToShift);
						}
				}	

		}
	
		/**
		 * Deletes all obstacles in the row
		 */
		public void deleteRow ()
		{ 	/*
		foreach (LandObstacle obstacle in obstacles) {
			obstacle.DeleteObstacle ();
		}	
		*/
				for (int i = 0; i < _obstacles.Length; i++) {
						if (_obstacles [i] != null) {
								_obstacles [i].DeleteObstacle ();
						}
				}	
		}

}


