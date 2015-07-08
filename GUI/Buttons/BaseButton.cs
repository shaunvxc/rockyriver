using UnityEngine;
using System.Collections;

/**
 *  Base class to handle the shared button functionality for  GUITextures
 * 
 */
public class BaseButton : MonoBehaviour, IButton
{
		private GUITexture texture;
		private Transform _transform;
		private bool pushed;

		void Awake ()
		{
				texture = GetComponent<GUITexture> ();
				_transform = transform;
		}

		void Update ()
		{

				for (int i = 0; i < Input.touchCount; i++) {
				
						Touch touch = Input.GetTouch (i);	
				
						if (texture.HitTest (touch.position, Camera.main)) {
								if (!pushed) { 
										_transform.position = new Vector3 (_transform.position.x, _transform.position.y - .01F, _transform.position.z);
										pushed = true;	
								}
						} 
				}

				if (Input.touchCount == 0) { 	
			
						if (pushed) {
							_transform.position = new Vector3 (_transform.position.x, _transform.position.y + .01F, _transform.position.z);
							onClick(); // execute the click action!!!
     					}

				}
				
		}
  	
		// would be nice to require this method to be overridden!
		public virtual void onClick ()
		{
				Debug.Log ("Error! Using a button with no implementation of onClick()-- this means some button will not be responsive!");
		}

		
		public void Activate ()
		{ 
			transform.gameObject.SetActive (true);
		}
		
		public void Deactivate ()
		{
			transform.gameObject.SetActive (false);
		}
	
}

