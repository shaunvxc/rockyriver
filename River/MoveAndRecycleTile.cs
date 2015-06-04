using UnityEngine;
using System.Collections;

/**
 * 
 *@author Shaun Viguerie
 * 
 */
public class MoveAndRecycleTile : MonoBehaviour
{
		public float speedInterval;
		public float x;
		public float  y_start;
		public float y_end;
		private bool moving = false;

		private SpriteRenderer renderer;
		private Transform _transform;
		
		void Awake ()
		{ 
				renderer = GetComponent<SpriteRenderer> ();
				_transform = transform;
		}

		void Start ()
		{ 
			
				_transform.position = new Vector2 (x, y_start);		

				disableRenderer ();
		}
	
		void Update ()
		{

				if (moving) {
						if (_transform.position.y <= y_end) { 	
								moving = false;
								disableRenderer ();
								_transform.position = new Vector2 (_transform.position.x, y_start); 
						} else {
								_transform.position = new Vector2 (_transform.position.x, _transform.position.y - speedInterval);
						}
				}
		}

		private void disableRenderer ()
		{

				if (renderer != null) { 
						renderer.enabled = false;
				}
				
		}

		public float getYPosition() {
			return _transform.position.y;
		}
		
		public void setMoving ()
		{ 

				enableRenderer ();	

				moving = true;
		}
		
		public bool getIsMoving() { 
				return moving;
		}

		public bool rendererEnabled() {
			return renderer.enabled;
		}

		public void enableRenderer ( )
		{	

				if (renderer != null) { 
						renderer.enabled = true;
				} else {
						Debug.Log ("renderer is null!");
				}
				
		}

}

