
using System;
using UnityEngine;

/**
* (C) %SelloutSystems
*
*  Convenience class to get rid of some of the boilerplate that was filling up LandObstacleManager.... though
* this may prove to simply be a temporary fix 
*  
* @Author Shaun Viguerie
* 
*/
public class ObstacleInfoHolder
{
		private float xPosition;
		private	float yPosition;
		private int index;
		private bool isRock;
		bool clearedAtIndex;
		bool clearedAbove;
		bool clearedBelow;
		bool clearedRight;
		bool clearedLeft;
		bool clearedUpperRight;
		bool clearedUpperLeft;
		bool clearedLowerRight;
		bool clearedLowerLeft;
	

		private ObstacleInfoHolder next;
		private ObstacleInfoHolder previous;
		
		private ObstacleInfoHolder (float xPos, float yPos, int index)
		{
				this.xPosition = xPos;
				this.yPosition = yPos;
				this.index = index;
		}

		public bool getIsRock ()
		{ 
				return isRock;
		}
		
		public void recheckSidesV2 (RowOfObstacles row, RowOfObstacles previousRow, RowOfObstacles nextRow)
		{
				if (yPosition == row.getObstacles () [0].getBottomRockYCoord ()) {
					
						checkSidesForBottom (this, row, previousRow, this.index);

						if (shouldBeRock ()) { 
							clearedAtIndex = true;
							row.setClearedBottomAtIndex (index);
							isRock = true;
						} 
								
				} else {	
					
						checkSidesForTop (this, row, nextRow, this.index);

						if (shouldBeRock ()) { 
							clearedAtIndex = true;
							row.setClearedTopAtIndex (index);
							isRock = true;
						} 
					
				}
		}

		public  void recheckSides (RowOfObstacles row)
		{ 

				if (yPosition == row.getObstacles () [0].getBottomRockYCoord ()) {
						// get reference to next and previous row as well..
						
						if (row.isClearedBottomIndex (index + 1)) {
								clearedRight = true;
						}
						
						if (row.isClearedBottomIndex (index - 1)) {
								clearedLeft = true;
						}
					

						if (shouldBeRock ()) { 
								clearedAtIndex = true;
								row.setClearedBottomAtIndex (index);
								isRock = true;
						} 
					
				} else {	
						// maybe need references to next & previous rows too?
						
						if (row.isClearedTopIndex (index + 1)) {
								clearedRight = true;
						}
							
						if (row.isClearedTopIndex (index - 1)) {
								clearedLeft = true;
						} 
						
						if (shouldBeRock ()) { 
								clearedAtIndex = true;
								row.setClearedTopAtIndex (index);
								isRock = true;
						} 

				}

				
		}
		
		public ObstacleInfoHolder getNext ()
		{ 
				return next;
		}
	 
		public ObstacleInfoHolder getPrevious ()
		{ 
				return next;	
		}
				
		public  bool shouldBeRock ()
		{ 
			return clearedAbove && clearedBelow && (clearedRight || clearedLeft);
		}

		public void setPrevious (ObstacleInfoHolder previous)
		{
				this.previous = previous;
				previous.setNext (this);
		}

		public void setNext (ObstacleInfoHolder next)
		{ 
				this.next = next;
		}
		
		public int getIndex ()
		{ 
				return index;	
		}

		public float getYPosition ()
		{
				return yPosition;
		}

		public float getXPosition ()
		{
				return xPosition;
		}

		public bool isClearedAtIndex ()
		{
				return clearedAtIndex;
		}
		
		public bool isClearedAbove ()
		{ 
				return clearedAbove;	
		}
		
		public bool isClearedBelow ()
		{ 
				return clearedBelow;	
		}
		
		public bool isClearedToTheRight ()
		{ 
				return clearedRight;	
		}
		
		public bool isClearedToTheLeft ()
		{
				return clearedLeft;		
		}
		
		public bool isClearedToTheUpperRight ()
		{ 
				return  clearedUpperRight;	
		}
		
		public bool isClearedToTheUpperLeft ()
		{
				return clearedUpperLeft;			
		}
		
		public bool isClearedToTheLowerRight ()
		{
				return clearedLowerRight;	
		}
		
		public bool isClearedToTheLowerLeft ()
		{ 
				return clearedLowerLeft;	
		}

		private static void checkSidesForBottom (ObstacleInfoHolder holder, RowOfObstacles row, RowOfObstacles previousRow, int index)
		{ 
				
				if (row.isClearedBottomIndex (index)) {
						holder.clearedAtIndex = true;
				}
				
				if (row.isClearedBottomIndex (index + 1)) {
						holder.clearedRight = true;
				}
				
				if (row.isClearedBottomIndex (index - 1)) { 
						holder.clearedLeft = true;
				}
				
				if (row.isClearedTopIndex (index)) { 
						holder.clearedAbove = true;
				}
				
				if (previousRow == null || previousRow.isClearedTopIndex (index)) {
						holder.clearedBelow = true;
				}
				
				if (row.isClearedTopIndex (index - 1)) { 
						holder.clearedUpperLeft = true;
				}
				
				if (row.isClearedTopIndex (index + 1)) { 
						holder.clearedUpperRight = true;
				}
				
				if (previousRow != null && previousRow.isClearedTopIndex (index - 1)) { 
						holder.clearedLowerLeft = true;
				}
				
				if (previousRow != null && previousRow.isClearedTopIndex (index + 1)) { 
						holder.clearedLowerRight = true;
				}
		}

		private static void checkSidesForTop (ObstacleInfoHolder holder, RowOfObstacles row, RowOfObstacles nextRow, int index)
		{

				if (row.isClearedTopIndex (index)) {
						holder.clearedAtIndex = true;
				}
					
				if (row.isClearedTopIndex (index + 1)) {
						holder.clearedRight = true;
				}
					
				if (row.isClearedTopIndex (index - 1)) { 
						holder.clearedLeft = true;
				}
					
				if (row.isClearedBottomIndex (index)) { 
						holder.clearedBelow = true;
				}
					
				if (nextRow.isClearedBottomIndex (index)) {
						holder.clearedAbove = true;
				}
					
				if (nextRow.isClearedBottomIndex (index - 1)) { 
						holder.clearedUpperLeft = true;
				}
					
				if (nextRow.isClearedBottomIndex (index + 1)) { 
						holder.clearedUpperRight = true;
				}
					
				if (row.isClearedBottomIndex (index - 1)) { 
						holder.clearedLowerLeft = true;
				}
					
				if (row.isClearedBottomIndex (index + 1)) { 
						holder.clearedLowerRight = true;
				}
		
		
		}
	
		public static ObstacleInfoHolder buildHolderForBottomObstacle (RowOfObstacles row, RowOfObstacles previousRow, int index)
		{
					
				RiverObstacle rockTile = row.getObstacles () [index];
		
				ObstacleInfoHolder holder = new ObstacleInfoHolder (rockTile.getBaseXPosition (), rockTile.getBottomRockYCoord (), index);
					
				if (row.isClearedBottomIndex (index)) {
						holder.clearedAtIndex = true;
				}

				if (row.isClearedBottomIndex (index + 1)) {
						holder.clearedRight = true;
				}

				if (row.isClearedBottomIndex (index - 1)) { 
						holder.clearedLeft = true;
				}

				if (row.isClearedTopIndex (index)) { 
						holder.clearedAbove = true;
				}

				if (previousRow == null || previousRow.isClearedTopIndex (index)) {
						holder.clearedBelow = true;
				}

				if (row.isClearedTopIndex (index - 1)) { 
						holder.clearedUpperLeft = true;
				}

				if (row.isClearedTopIndex (index + 1)) { 
						holder.clearedUpperRight = true;
				}

				if (previousRow != null && previousRow.isClearedTopIndex (index - 1)) { 
						holder.clearedLowerLeft = true;
				}
		
				if (previousRow != null && previousRow.isClearedTopIndex (index + 1)) { 
						holder.clearedLowerRight = true;
				}


				if (holder.shouldBeRock ()) {
						holder.clearedAtIndex = true;
						row.setClearedBottomAtIndex (index);
						holder.isRock = true;
				}
			
				return holder;
		}
		
		public static ObstacleInfoHolder buildHolderForTopObstacle (RowOfObstacles row, RowOfObstacles nextRow, int index)
		{
		
				RiverObstacle rockTile = row.getObstacles () [index];
		
				ObstacleInfoHolder holder = new ObstacleInfoHolder (rockTile.getBaseXPosition (), rockTile.getTopRockYCoord (), index);
		
				if (row.isClearedTopIndex (index)) {
						holder.clearedAtIndex = true;
				}

				if (row.isClearedTopIndex (index - 1)) { 
					holder.clearedLeft = true;
				}

				if (row.isClearedTopIndex (index + 1)) {
					holder.clearedRight = true;
				}
				
				if (row.isClearedBottomIndex (index)) { 
						holder.clearedBelow = true;
				}
		
				if (nextRow.isClearedBottomIndex (index)) {
						holder.clearedAbove = true;
				}
		
				if (nextRow.isClearedBottomIndex (index - 1)) { 
						holder.clearedUpperLeft = true;
				}
		
				if (nextRow.isClearedBottomIndex (index + 1)) { 
						holder.clearedUpperRight = true;
				}
		
				if (row.isClearedBottomIndex (index - 1)) { 
						holder.clearedLowerLeft = true;
				}
		
				if (row.isClearedBottomIndex (index + 1)) { 
						holder.clearedLowerRight = true;
				}

		
				if (holder.shouldBeRock ()) {
						holder.clearedAtIndex = true;
						row.setClearedTopAtIndex (index);
						holder.isRock = true;
						
					
				}
				
				return holder;
		}

		private static void setAsBottomRock (ObstacleInfoHolder holder, RowOfObstacles row, int index)
		{
				holder.clearedAtIndex = true;
				row.setClearedBottomAtIndex (index);
				holder.isRock = true;
		}

		private static void setAsTopRock (ObstacleInfoHolder holder, RowOfObstacles row, int index)
		{
				holder.clearedAtIndex = true;
				row.setClearedTopAtIndex (index);
				holder.isRock = true;
		}
}

		

