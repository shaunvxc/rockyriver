using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * 
 * @Author Shaun.Viguerie
 */
public class RiverObstacleManager : MonoBehaviour
{

		private List<RowOfObstacles> rows;
		private LandObstacleManagerV2 landObstacleManager;
		public float InitialObstacleStartLevel;
		
		public bool startup;
		public bool persistRiverSolution;

		private float LeftRiverBoundary;
		private float RightRiverBoundary;
		private float TopRiverBoundary;
		private float TileSize;
		private float HalfTileSize;
		private float offset;
		public  Transform Rock;
		public  int NumObstaclesPerRow;
		private IPathGenerator pathGenerator;
		private bool GenerateNewRowAtTop = false;
		private int  fallback = 0;

		private RowOfObstacles bottomExtraRow;
		
		private List<int> pathThroughRiver;
		
		private bool rookieMode;	

		void Awake ()
		{ 
				landObstacleManager = GetComponent<LandObstacleManagerV2> ();
				rows = new List<RowOfObstacles> ();
 		}
  
		void Start ()
		{

				if (landObstacleManager == null) {
						landObstacleManager = GetComponent<LandObstacleManagerV2> ();
				}
			
				LeftRiverBoundary = PanchoRiver.Instance.LeftRiverBoundary;
				RightRiverBoundary = PanchoRiver.Instance.RightRiverBoundary;
				TopRiverBoundary = PanchoRiver.Instance.TopRiverBoundary + 5;
				offset = PanchoRiver.Instance.CameraShiftOffset;

				TileSize = PanchoRiver.Instance.TileSize;
				HalfTileSize = TileSize * .5F;
		}
		
		
		public void AddRow (RowOfObstacles row)
		{
				rows.Insert (0, row);
		}

		public void SimpleShiftDown (int numRowsToShift)
		{ 
				for (int i = rows.Count - 1; i >=0; i--) {
						rows [i].shiftRowDown (numRowsToShift);
				}
		}

		public void ShiftRowsDown (int numRowsToShift)
		{ 		
			
				for (int i = rows.Count - 1; i >=0; i--) {
						rows [i].shiftRowDown (numRowsToShift);
						landObstacleManager.ShiftRowDown( i, numRowsToShift);
				}
				

				if (bottomExtraRow != null) {
					bottomExtraRow.shiftRowDown(1);
				}
				

				DeleteBottomRowIfEligible ();
				
				if (readyForNewRow ()) {
						GenerateRowWithPath ();	
				}
		}
		
		public float getLowestYPosition() {
			if (rows != null && rows.Count > 0) {
				return rows [rows.Count - 1].getYCoord ();				
			}
			

			return 0F;
				
		}

		public float getLowestXPosition() { 
				
				if (rows != null && rows.Count > 0) {
					return rows[rows.Count - 1].getFirstPositiveXPosition();
				}
				
				Debug.Log ("Rows are empty returning 0F");
				return 0F;
		}

		public float getBaseXPosition ()
		{

				if (rows != null && rows.Count > 0) {
						return rows [0].getFirstPositiveXPosition ();				
				}

				Debug.Log ("Rows are empty returning 0F");
				return 0F;
		}

		public void ShiftRowsUp (int numRowsToShift)
		{ 
	
				for (int i = 0; i < rows.Count; i++) {
						rows [i].shiftRowUp (numRowsToShift);
				} 
				
				if (bottomExtraRow != null) {
					bottomExtraRow.shiftRowUp(1);
				}
			
				landObstacleManager.ShiftRowsUp (numRowsToShift);
				
		}

		public void DeleteBottomRowIfEligible ()
		{	
				if (rows.Count >= 15 && rows [rows.Count - 1].getYCoord () <= -5) {
						rows [rows.Count - 1].deleteRow ();
						rows.RemoveAt (rows.Count - 1);
				} 
				else if (bottomExtraRow != null && rows [rows.Count - 1].getYCoord () <= -2) { // was - 1  // delete the extra row once we have enough obstacles
						bottomExtraRow.deleteRow();
						bottomExtraRow = null;
				}

				landObstacleManager.DeleteBottomRowIfEligible ();
		}

		public void DeleteTopRow ()
		{
				rows [0].deleteRow ();
				rows.RemoveAt (0);
		}

		public void GenerateFreshObstacles ()
		{
			init (); // initialize the system if Start() was never called after Application.LoadLevel(loadedLevel);
			
			initObstacles ();
			
			renderExtraRowAtBottom ();
		}

		public List<int> getPathThroughRiver() { 
			
			if (persistRiverSolution) {
				return pathThroughRiver;
			}
			
			Debug.Log ("Error!!! Cannot retrieve solution to river in the current scene!!!");
			return null;  
		}	
	
  		/** Private methods **/
  		 
  		private void init ()
		{	
				if (landObstacleManager == null) {
						landObstacleManager = GetComponent<LandObstacleManagerV2> ();
				}

				LeftRiverBoundary = PanchoRiver.Instance.LeftRiverBoundary;
				RightRiverBoundary = PanchoRiver.Instance.RightRiverBoundary;
				TopRiverBoundary = PanchoRiver.Instance.TopRiverBoundary + 3;
				offset = PanchoRiver.Instance.CameraShiftOffset;
			
				TileSize = PanchoRiver.Instance.TileSize;
				HalfTileSize = TileSize * .5F;
						
				if (PanchoRiver.Instance.getFallbackTime () > 10 || PanchoRiver.Instance.demoScene) {
					rookieMode = true;
				}
				
				if (persistRiverSolution) {
					pathThroughRiver = new List<int>();  // allocate a list to track all moves for the startup sequence
				}
		}


		private RowOfObstacles createRowV2 (float y)
		{
					
				RowOfObstacles row = new RowOfObstacles ();		
								
				for (float j = LeftRiverBoundary; j <= RightRiverBoundary; j+= HalfTileSize) {  // possibly tile size...
						if (j == LeftRiverBoundary) {
								
							//row.addObstacle (createRiverObstacle (j, y, offset * -2F));

							row.addObstacle (createRiverObstacle (j, y, offset * -1F));
						
						} 

						row.addObstacle (createRiverObstacle (j, y, 0F));
						if(j == RightRiverBoundary) {
							row.addObstacle(createRiverObstacle(j, y, offset));
						}
				}


				return row;
			
		}

		private void renderExtraRowAtBottom(){
				bottomExtraRow = createRowV2 (-2);
		}


		private RiverObstacle createRiverObstacle (int j, float leftOffset)
		{ 
				var rock = ObjectPoolManager.CreatePooled (Rock.gameObject, new Vector3 ((j * TileSize) + offset + leftOffset, TopRiverBoundary * TileSize, 0F), Quaternion.identity);
				return rock.gameObject.GetComponent<RiverObstacle> ();
		}

		private RiverObstacle createRiverObstacle (int x, int y, float leftOffset)
		{ 
				var rock = ObjectPoolManager.CreatePooled (Rock.gameObject, new Vector3 ((x * TileSize) + offset + leftOffset, y * TileSize, 0F), Quaternion.identity);
				return rock.gameObject.GetComponent<RiverObstacle> ();
		}

		private RiverObstacle createRiverObstacle (int x, float y, float leftOffset)
		{ 
				var rock = ObjectPoolManager.CreatePooled (Rock.gameObject, new Vector3 ((x * TileSize) + offset + leftOffset, y, 0F), Quaternion.identity);
				return rock.gameObject.GetComponent<RiverObstacle> ();
		}

		private RiverObstacle createRiverObstacle (float x, float y, float leftOffset)
		{ 		
	
				var rock = ObjectPoolManager.CreatePooled (Rock.gameObject, new Vector3 ((x * TileSize) + offset + leftOffset, y, 0F), Quaternion.identity);
				return rock.gameObject.GetComponent<RiverObstacle> ();
		}

		private void initObstacles ()
		{
				
				
				if (startup) {
						initObstaclesForStartup ();
				} else {
					
					int lower = 1;
					int upper = 8;
      				
					AddRow (createRowV2 (TopRiverBoundary - 8));

					if(rookieMode) {
						rows[0].clearRange(lower, upper);
						lower++;
						upper--;
					}

					pathGenerator = new ObstaclePathGeneratorV3 (rows [0].getNumObstaclesInRow () - 1);
				
					for (int i = 7; i >= 0; i -- ) {
						
						AddRow (createRowV2(TopRiverBoundary - i));
						
						if(rookieMode && upper - lower > 2) {
							rows[0].clearRange(lower, upper);
							
							//if( i % 2 == 0) {
								lower++;
								upper--;
							//}
						}
					
						GeneratePathForRows();
						renderLandTiles();
					}
				
				}
		}
		


		/**
		 * We have a separate method right now for startup because we want to generate a bigger river 
		 * that fills up the full screen
		 */
		private void initObstaclesForStartup ()
		{
				AddRow (createRowV2 (TopRiverBoundary - 12));
				pathGenerator = new ObstaclePathGeneratorV3 (rows [0].getNumObstaclesInRow () - 1);
				AddRow (createRowV2 (TopRiverBoundary - 11));
				GeneratePathForRows ();
			
				for (int i = 10; i >= 0; i--) {
						AddRow (createRowV2 (TopRiverBoundary - i));
						GeneratePathForRows ();
						renderLandTiles();
				} 
		}
	
		private void GenerateRowWithPath ()
		{
				if (GenerateNewRowAtTop && (rows [0].getTopRockYCoord () < TopRiverBoundary)) {
						AddRow (createRowV2 (TopRiverBoundary));
						GenerateNewRowAtTop = false;
				} else {
						AddRow (createRowV2 (rows [0].getTopRockYCoord () + .75F));
						if (GenerateNewRowAtTop) {
								GenerateNewRowAtTop = false;
						}
				}	

				GeneratePathForRows ();		
				renderLandTiles ();
		}

		private void GeneratePathForRows ()
		{ 		

				int bound = 2;

				bool clearAtIndex = false;
				
				for (int i = 0; i < bound; i++) {

						int move = pathGenerator.getNext ();

						if (move == -1) {
								
								if (pathGenerator.getNumTotalMoves () % 2 != 0) {
		
										if (2 + fallback < rows.Count) {	
											rows [2 + fallback].clearBottomRockAtIndex (pathGenerator.getPreviousMove ());
										}
										
										fallback++;
								} else {
										rows [1 + fallback].clearBottomRockAtIndex (pathGenerator.getPreviousMove ());
								}
								
								i--;
								bound++;
								
						} else {
																			// this might be it!!!
								if (pathGenerator.getNumTotalMoves () == 1 ) { // || pathGenerator.isNewSequence ()) { 
										rows [1].clearAtIndexOnly (pathGenerator.getPreviousMove ());
								} 
								
								int numTotalMoves = pathGenerator.getNumTotalMoves ();
								
								if (numTotalMoves % 2 == 0 && !clearAtIndex) { 
										rows [1 + fallback].clearTopRockAtIndex (move);					
										rows [0 + fallback].clearBottomRockAtIndex (move);	
								} else {
										

										rows [1].clearAtIndexOnly (move);

										if (2 + fallback < rows.Count && !clearAtIndex) { 	
											rows [2 + fallback].clearTopRockAtIndex (pathGenerator.getPreviousMove ());  // we might not need this
										}
									
										if (clearAtIndex) { 
												clearAtIndex = false; 
										}
									
								}
						
	
								if (fallback > 0) {
										fallback--;
								}
						}
					
						if(persistRiverSolution) {
							pathThroughRiver.Add(move);
						}
				}

				/*
				if (3 < rows.Count) { 
						landObstacleManager.buildUpObstacleInfo (rows [3], rows [2]);  
				} */

		}

		private void renderLandTiles() { 
			if (3 < rows.Count) { 
				landObstacleManager.buildUpObstacleInfo (rows [3], rows [2]);  
			}	
		}
		

		private bool readyForNewRow ()
		{
				if (countRowsAboveTopBoundary () > 0) {
						return false;
				} else {

						return true;
				}
		}

		private int countRowsAboveTopBoundary ()
		{
				int ct = 0;
				for (int i = 0; i < rows.Count; i++) {
						
						if (rows [i].getTopRockYCoord () > TopRiverBoundary) {
								ct++;
						} else {
								break;
						}
				}
	
	
				return ct;
		}

		private void printRows ()
		{

				for (int i = 0; i < rows.Count; i++) {
						Debug.Log ("Row[" + i + "].yPos  = " + rows [i].getYCoord ());
						
				}
		}
}

