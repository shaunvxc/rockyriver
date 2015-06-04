using UnityEngine;
using System.Collections;

/**
 * (C) %SelloutSystems
 * 
 * @Author Shaun Viguerie
 * 
 * For now, land will simply cover up obstacles, so no colliders should be necessary!!
 * 
 */
public class LandObstacle : MonoBehaviour
{

		public float TileSize;
		public int destroyYCoord;
		private float tileScaleSize;
		
		private SpriteRenderer renderer;
		
		private TileType currentTileType;
		
		private Transform _transform;

		void Awake() { 
			renderer = GetComponent<SpriteRenderer> ();
			_transform = transform;
		}

		void Start ()
		{	
				if (TileSize == 0) {
					tileScaleSize = PanchoRiver.Instance.TileSize;
				} else {
					tileScaleSize = TileSize;
				}
				
				renderer = GetComponent<SpriteRenderer> ();

		}

		public float getYCoord ()
		{
				return _transform.position.y;
		}
	

		/**
		 * 
		 * Called from PanchoRiver afer the player has paddled enough up-river to generate more content
		 */
		public void moveObstacleDown (int numRowsToShift)
		{ 
				_transform.position = new Vector2 (_transform.position.x, _transform.position.y - (tileScaleSize * numRowsToShift * .5F));									
		}
	
		public void moveObstacleUp (int  numRowsToShift)
		{
				_transform.position = new Vector2 (_transform.position.x, _transform.position.y + (tileScaleSize * numRowsToShift * .5F));									
		}
	
		/**
		 * Delete (recycle) the object
		 */
		public void DeleteObstacle ()
		{ 
				if (gameObject != null) {
						ObjectPoolManager.DestroyPooled (gameObject);
				} else {
						Debug.Log ("Error, gameObject is null, attempting to destory (recycle) already nulled gameObject!");
				}
		}


		public void setSprite(Sprite sprite) { 
			renderer.sprite = sprite;	
		}


}

