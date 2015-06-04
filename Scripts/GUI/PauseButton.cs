using UnityEngine;
using System.Collections;

public class PauseButton : MonoBehaviour
{
	
		private GUITexture texture;
		public static PauseButton Instance;
		private bool pushed;
		private  bool paused = false;
		private int reloadDelay = 0;
		private float xPosition;

		void Awake ()
		{
				if (Instance == null) {
						Instance = this;
				}
				texture = GetComponent<GUITexture> ();
		}

		public bool isPaused ()
		{
				return paused;
		}
	
		void Start ()
		{
				reloadDelay = 0;
				xPosition = texture.pixelInset.x;
				Deactivate ();
		}
		
		public float getXPosition() { 
			return xPosition;
		}

		public void Activate ()
		{
				transform.gameObject.SetActive (true);
		}

		public void Deactivate ()
		{
				transform.gameObject.SetActive (false);
		}

		void Update ()
		{ 
		
				if (reloadDelay < 5) { 
						reloadDelay++;
				} else {
			
						for (int i = 0; i < Input.touchCount; i++) {
				
								Touch touch = Input.GetTouch (i);	
								if (touch.phase == TouchPhase.Began) {
										if (texture.HitTest (touch.position, Camera.main)) {
												if (!pushed) { 
														if (!paused) {
																Time.timeScale = 0;
															//				paused = true;
														}
														transform.position = new Vector2 (transform.position.x, transform.position.y - .01F);
														pushed = true;
												}
										} 
								}
						}
			
			
						if (Input.touchCount == 0) { 
								if (pushed) {
										pushed = false;
										transform.position = new Vector2 (transform.position.x, transform.position.y + .01F);
										if (!paused) {
												paused = true;
										} else {
												Time.timeScale = 1;
												paused = false;
										}
										reloadDelay = 0;
								}
						}
				}
		}
}
