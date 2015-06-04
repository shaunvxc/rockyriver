using UnityEngine;
using System.Collections;

/**
 * 
 * Represents an obstacle on the river, each obstacle is a 1 x 2 column of rocks
 * 
 * @Author Shaun Viguerie
 * 
 */
public class RiverObstacle : MonoBehaviour
{			
		public  float TileSize;
		private float tileScaleSize;
		public  int destroyYCoord;
			
		private SpriteRenderer[] renderers;
		private SpriteRenderer   topRock;
		private SpriteRenderer   bottomRock;
		private BoxCollider2D    topCollider;
		private BoxCollider2D    bottomCollider;
		
		private Transform _transform;

		private Transform _topRockTrans;
		private Transform _bottomRockTrans;
		

		void Awake() {
				_transform = transform;
		}

		void Start ()
		{	

				if (TileSize == 0) {
						tileScaleSize = PanchoRiver.Instance.TileSize;
				} else {
						tileScaleSize = TileSize;
				}

				initTopAndBottomRockRenderers (_transform.gameObject.GetComponentsInChildren<SpriteRenderer> ());
		}



		public float getYCoord ()
		{
				return _transform.position.y;
		}
	

		public float getTopRockYCoord ()
		{
					checkRenderers ();
					return _topRockTrans.position.y;
			//		return topRock.gameObject.transform.position.y;
		}

		public float getBottomRockYCoord ()
		{
				checkRenderers ();
				return _bottomRockTrans.position.y;
				//return bottomRock.gameObject.transform.position.y;
		}

		public float getBaseXPosition ()
		{
				return _transform.position.x;
		}
		

		/**
		 * Delete (recycle) the object
		 */
		public void DeleteObstacle ()
		{ 
				if (gameObject != null) {
						resetObstaclesBeforeDestroying ();
						ObjectPoolManager.DestroyPooled (gameObject);
				} else {
						Debug.Log ("Error, gameObject is null, attempting to destory (recycle) already nulled gameObject!");
				}
		}

		/**
		 * 
		 * Called from PanchoRiver afer the player has paddled enough up-river to generate more content
		 */
		public void moveObstacleDown (int numRowsToShift)
		{ 
				_transform.position = new Vector2 (_transform.position.x, _transform.position.y - (tileScaleSize * numRowsToShift * .5F));									
		}

		public void moveObstacleUp (int numRowsToShift)
		{
				_transform.position = new Vector2 (_transform.position.x, _transform.position.y + (tileScaleSize * numRowsToShift * .5F));									
		}
		

		public bool isClearedAtTop() {
			
			if (topRock == null) {
				checkRenderers();
			}

			if (topRock != null) {
				return !(topRock.enabled);
			}

			return false;
			
		}

		public bool isClearedAtBottom() {
			
			if (bottomRock == null) {
				checkRenderers();
			}
			
			if (bottomRock != null) {
				return !(bottomRock.enabled);
			}
			
			return false;
			
		}

		public void clearTopRock ()
		{
			
				checkRenderers ();

				if (topRock != null) {
				
						topRock.enabled = false;
						if (topCollider != null) {
								topCollider.enabled = false;
						}

				} else {
						Debug.Log ("Error!! Top Rock is null, cannot clear obstacles!");
				}

		}

		public void clearBottomRock ()
		{
				
				checkRenderers ();	
				
				if (bottomRock != null) {

						bottomRock.enabled = false;
						if (bottomCollider != null) { 
								bottomCollider.enabled = false;
						}
						
				} else {
						Debug.Log ("Error! BottomRock is  null, cannot clear obstacles!");
				}

		}

		public void setSpriteOnBothTopAndBottom(Sprite sprite) {
			setSpriteOnBottomRock (sprite);
			setSpriteOnTopRock (sprite);
		}

		public void setSpritesOnBothTopAndBottom(Sprite topSprite, Sprite bottomSprite) {
			setSpriteOnBottomRock (bottomSprite);
			setSpriteOnTopRock (topSprite);
		}

		public void setSpriteOnTopRock(Sprite sprite) {

			
			if (topRock == null) {
				checkRenderers();
			}

			if (topRock != null && sprite != null) {
				topRock.sprite = sprite;

			} else {
				Debug.Log ("SetSpriteOnTopRock: Sprite is null!");
			}
			
		}


		public void setSpriteOnBottomRock(Sprite sprite) {

			if (bottomRock == null) {
				checkRenderers();
			}
			
			if (bottomRock != null && sprite != null) {
				bottomRock.sprite = sprite;
			} else {
				Debug.Log ("SetSpriteOnBottomRock: Sprite is null!");
			}
		}

		public void resetBothRocks ()
		{
				resetTopRock ();
				resetBottomRock ();
		}

		public void resetBottomRock ()
		{
				checkRenderers ();
				if (bottomRock != null) {
						bottomRock.enabled = true;
						if (bottomCollider != null) {
								bottomCollider.enabled = true;
						}
				}

		}

		public void resetTopRock ()
		{
				checkRenderers ();
				if (topRock != null) {
						topRock.enabled = true;
						if (topCollider != null) {
								topCollider.enabled = true;
						}
				}
		
		}

		public void clearObstacles ()
		{	
				checkRenderers ();

				if (renderers != null) {
						clearTopRock ();
						clearBottomRock ();
				} else {
						Debug.Log ("Renderers are null, cannot clear obstacles");
				}
		}

		private void checkRenderers ()
		{
			if (renderers == null) {
				initTopAndBottomRockRenderers (_transform.gameObject.GetComponentsInChildren<SpriteRenderer> ());
			}
			
		}

		private void resetObstaclesBeforeDestroying ()
		{

				checkRenderers ();
			

				if (bottomRock != null) {
						bottomRock.enabled = true;
						
						if (bottomCollider != null) {
								bottomCollider.enabled = true;
						}
				}
				

				if (topRock != null) {
						topRock.enabled = true;
						if (topCollider != null) {
								topCollider.enabled = true;
						}
				}

			

		}
  	
		private void initTopAndBottomRockRenderers (SpriteRenderer[] sprites)
		{			
				if (sprites != null) {
						if (renderers == null || topRock == null || bottomRock == null) {
								if (sprites.Length == 2) {
								
										if (sprites [0].transform.position.y > sprites [1].transform.position.y) {
												topRock = sprites [0];
												bottomRock = sprites [1];
												

										} else {
												topRock = sprites [1];
												bottomRock = sprites [0];
												
										}

										renderers = sprites;
										_topRockTrans = topRock.gameObject.transform;
										_bottomRockTrans = bottomRock.gameObject.transform;
										initTopAndBottomColliders ();
								}
						}
				} else {
						Debug.Log ("Error, sprites are null, init failed!");
				}
		}
		
		private void initTopAndBottomColliders ()
		{
			
				if (topRock != null) {
						topCollider = topRock.gameObject.GetComponent<BoxCollider2D> ();
				}

				if (bottomRock != null) {
						bottomCollider = bottomRock.gameObject.GetComponent<BoxCollider2D> ();
				}
			
		}
}

