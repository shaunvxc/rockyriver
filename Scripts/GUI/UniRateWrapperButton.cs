using UnityEngine;
using System.Collections;

/**
 * Wrapper to call UniRatePrompt when the rate button is pushed
 * 
 */
public class UniRateWrapperButton : MonoBehaviour
{

		private GUITexture texture;
		private bool pushed;

		void Awake ()
		{
				texture = GetComponent<GUITexture> ();	
		}

		void Start ()
		{ 
				pushed = false;
		}

		void Update ()
		{

				for (int i = 0; i < Input.touchCount; i++) {
			
						Touch touch = Input.GetTouch (i);	
			
						if (texture.HitTest (touch.position, Camera.main)) {
								if (!pushed) { 
										transform.position = new Vector2 (transform.position.x, transform.position.y - .01F);
										pushed = true;	
								}
						} 
				}
		
		
				if (Input.touchCount == 0) { 
			
						if (pushed) {
								transform.position = new Vector2 (transform.position.x, transform.position.y + .01F);
								SoundEffectsHelper.Instance.MakeButtonClick ();
								pushed = false;

								UniRate.Instance.PromptIfNetworkAvailable ();
						}
				}

		}
}
