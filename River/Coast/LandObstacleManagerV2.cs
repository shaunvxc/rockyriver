using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LandObstacleManagerV2 : MonoBehaviour
{
	private List<RowOfLandObstacles> rows;

	public Transform baseTile;

	public float TileSize;
	private Sprite[] sprites;
	private RowOfObstacles previousRow;
	
	void Awake ()
	{ 
		rows = new List<RowOfLandObstacles> ();
		sprites = Resources.LoadAll<Sprite> ("TerrainTileAtlas");
	}
	
	void Start ()
	{ 

		if (sprites == null) { 
			sprites = Resources.LoadAll<Sprite>("TerrainTileAtlas");
		}

	//	Debug.Log ("Starting up LandObstacleManager");
	}
	

	public void buildUpObstacleInfo (RowOfObstacles row, RowOfObstacles nextRow)
	{
		
		RiverObstacle[] obstacles = row.getObstacles ();	 
		RowOfLandObstacles landRow = new RowOfLandObstacles ();
		
		List<ObstacleInfoHolder> infoForRow = new List<ObstacleInfoHolder>();
		
		int numRocks = 0;
		
		if(previousRow != null) {
			numRocks = getNumRocksForRow();
		}
		
		bool bottomRight = false;
		bool topRight = false;	
		
		int rand = UnityEngine.Random.Range (0, 100);
		
		if(rand <= 50) {
			bottomRight  = true;
		}
		
		if(rand % 2 == 0) { 
			topRight = true;
		}
		
		if (numRocks > 0) {
			nextRow.setVisibleRocksInDirection(numRocks, topRight, !topRight);  // this is to make some rocks visible (ie don't draw land on top of them!!)
		}
		
		for (int i = 0; i < obstacles.Length; i++ ) {
			infoForRow.Add (ObstacleInfoHolder.buildHolderForBottomObstacle(row, previousRow, i));
			infoForRow.Add (ObstacleInfoHolder.buildHolderForTopObstacle(row , nextRow, i));   

		}
		
		
		for (int i = infoForRow.Count -1; i >=0; i--) {
			var infoHolder = infoForRow[i];
			infoHolder.recheckSidesV2(row, previousRow, nextRow);
			chooseTileForHolder(landRow, infoHolder);
		}
		
		rows.Insert (0, landRow);
		previousRow = row;
	}
	
	public void chooseTileForHolder (RowOfLandObstacles landRow,  ObstacleInfoHolder tileInfo)
	{
		
		if (tileInfo.isClearedAtIndex ()) {
			return;
		}
		
		if (tileInfo.isClearedToTheLeft ()) {  
			
			if (tileInfo.isClearedAbove ()  ) {
				landRow.addObstacle (createLandObstacle (TileIndexes.CoastOuterCornerTopLeft, tileInfo.getXPosition (), tileInfo.getYPosition ()));
			} else if (tileInfo.isClearedBelow ()) {
				landRow.addObstacle (createLandObstacle (TileIndexes.CoastOuterCornerBottomLeft, tileInfo.getXPosition (), tileInfo.getYPosition ())); 
			} else {
				landRow.addObstacle (createLandObstacle (TileIndexes.CoastEdgeRight, tileInfo.getXPosition (), tileInfo.getYPosition ()));
			}
		} else if (tileInfo.isClearedToTheRight ()) {
			
			if (tileInfo.isClearedAbove () && !tileInfo.isClearedBelow() ) {
				landRow.addObstacle (createLandObstacle (TileIndexes.CoastOuterCornerTopRight, tileInfo.getXPosition (), tileInfo.getYPosition ()));
			} else if (tileInfo.isClearedBelow () && !tileInfo.isClearedAbove() ) {
				landRow.addObstacle (createLandObstacle (TileIndexes.CoastOuterCornerBottomRight, tileInfo.getXPosition (), tileInfo.getYPosition ())); 
			} else {
				landRow.addObstacle (createLandObstacle (TileIndexes.CoastEdgeLeft, tileInfo.getXPosition (), tileInfo.getYPosition ()));
			}
		} else {
			
			if (tileInfo.isClearedToTheLowerLeft () && !tileInfo.isClearedBelow () && !tileInfo.isClearedAbove() && !tileInfo.isClearedToTheLowerRight ()) {
				landRow.addObstacle (createLandObstacle (TileIndexes.CoastInnerCornerTopRight, tileInfo.getXPosition (), tileInfo.getYPosition ()));
			} 
			else if (tileInfo.isClearedToTheLowerRight () && !tileInfo.isClearedBelow () && !tileInfo.isClearedAbove()  && !tileInfo.isClearedToTheLowerLeft ()) {
				landRow.addObstacle (createLandObstacle (TileIndexes.CoastInnerCornerTopLeft, tileInfo.getXPosition (), tileInfo.getYPosition ()));
			} 
			else if (tileInfo.isClearedToTheUpperLeft () && !tileInfo.isClearedAbove () && !tileInfo.isClearedBelow() && !tileInfo.isClearedToTheUpperRight ()) {
				landRow.addObstacle (createLandObstacle (TileIndexes.CoastInnerCornerBottomRight, tileInfo.getXPosition (), tileInfo.getYPosition ()));
			} 
			else if (tileInfo.isClearedToTheUpperRight () && !tileInfo.isClearedAbove () && !tileInfo.isClearedBelow()  && !tileInfo.isClearedToTheUpperLeft ()) {
				landRow.addObstacle (createLandObstacle (TileIndexes.CoastInnerCornerBottomLeft, tileInfo.getXPosition (), tileInfo.getYPosition ()));
			} 
			else if (tileInfo.isClearedBelow ()) { 
				landRow.addObstacle (createLandObstacle (TileIndexes.CoastEdgeBottom, tileInfo.getXPosition (), tileInfo.getYPosition ()));
			} 
			else if (tileInfo.isClearedAbove ()) {
				landRow.addObstacle (createLandObstacle (TileIndexes.CoastEdgeTop, tileInfo.getXPosition (), tileInfo.getYPosition ()));
			} 
			else {
				landRow.addObstacle( createLandObstacle (TileIndexes.Grass, tileInfo.getXPosition (), tileInfo.getYPosition ()));
			}
		}
	}
	
	private LandObstacle createLandObstacle (int spriteTransIdx, float x, float y)
	{
		var tile = ObjectPoolManager.CreatePooled (baseTile.gameObject, new Vector3 (x, y, 0F), Quaternion.identity);
		LandObstacle obstacle = tile.gameObject.GetComponent<LandObstacle> ();
		if (obstacle != null) { 
			obstacle.setSprite (sprites [spriteTransIdx]);
		}
	
		return obstacle;
	}


	public void ShiftRowDown( int i, int numRowsToShift) {

			if (i < rows.Count) {
				rows[i].shiftRowDown(numRowsToShift);
			}
	}

	public void ShiftRowsDown (int numRowsToShift)
	{ 
		for (int i = rows.Count - 1; i >=0; i--) {
			rows [i].shiftRowDown (numRowsToShift);
		}		
	}
	
	public void ShiftRowsUp (int numRowsToShift)
	{
		for (int i = 0; i < rows.Count; i++) {
			rows [i].shiftRowUp (numRowsToShift);
		} 
	}
	
	public void DeleteBottomRowIfEligible ()
	{	
		if (rows.Count >= 15 && rows [rows.Count - 1].getYCoord () <= -5) {
			rows [rows.Count - 1].deleteRow ();
			rows.RemoveAt (rows.Count - 1);
		}
	}
	
	private int getNumRocksForRow() {
		
		int rand = UnityEngine.Random.Range (0, 100);
		
		if (rand <= 50) {
			return 0;
		} 
		else {
			return 1;
		}
	}
}

