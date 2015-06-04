using UnityEngine;
using System.Collections;

public class PanchoRiverTile : MonoBehaviour
{
		public int   TileSize;
		public int   destroyYCoord;
		public float speedInterval;
		private float tileSizeScale;
		private float startYCoord;
		private float movementRate;
		
		private Transform _transform;
		
		private float destroyBound;
		
		void Start ()
		{ 
				if (TileSize == 0) { 
						tileSizeScale = PanchoRiver.Instance.TileSize;
				} else {
						tileSizeScale = TileSize;
				}
				
				_transform = transform;
				startYCoord = PanchoRiver.Instance.TopRiverBoundary;
 				
				int fallbackTime = PanchoRiver.Instance.getFallbackTime ();
				
				if (fallbackTime == 0) { 
						movementRate = .025F;
				} else if (fallbackTime > 10) {
						movementRate = .03F;
				} else { 
						movementRate = .04F;
				}
							
				destroyBound = destroyYCoord * tileSizeScale;
		}
		
		void Update ()
		{
				
				if (_transform.position.y < destroyBound) { // (destroyYCoord * tileSizeScale)) {					
						if (gameObject != null) { 
								_transform.position = new Vector2 (_transform.position.x, startYCoord * tileSizeScale);
						}	
				} else if (Time.timeScale != 0) { 					
						_transform.position = new Vector2 (_transform.position.x, _transform.position.y - movementRate);		
				}	
			
		}

		public float getYPosition ()
		{ 
				return _transform.position.y;	
		}
}

