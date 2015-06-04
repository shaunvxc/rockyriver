using UnityEngine;
using System.Collections;

public class CycleFishManager : MonoBehaviour
{
		public delegate void CycleFishEventDelegate ();
		public  event CycleFishEventDelegate CycleFishEvent;
		
		public static CycleFishManager Instance;
		private GUITexture texture;
		private bool pushed;
		private Transform _transform;

		void Awake ()
		{	
				if (Instance == null) {
					Instance = this;
				}
				
				_transform = transform;
				texture = GetComponent<GUITexture> ();
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
						pushed = false;
					
						if(CycleFishEvent != null) {
							CycleFishEvent();
						}
					}
				}
		}
		
		public GUITexture getTexture() {
			return texture;
		}
		
		public void Activate ()
		{ 
			gameObject.SetActive (true);
		}
		
		public void Deactivate ()
		{ 
			gameObject.SetActive (false);
		}

}

