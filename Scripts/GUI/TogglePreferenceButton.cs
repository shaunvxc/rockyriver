using UnityEngine;
using System.Collections;
/**
 * 
 * @Author Shaun Viguerie
 * 
 * (C) %SelloutSystems
 * 
 */
public class TogglePreferenceButton : MonoBehaviour
{
		
		public string preferenceKey;
		private GUITexture[] buttons;
		private int activeButtonIndex;
		private bool pushed;
		


		void Awake ()
		{ 
				buttons = GetComponentsInChildren<GUITexture> ();

				if (!PlayerPrefs.HasKey (preferenceKey)) {
						PlayerPrefs.SetInt (preferenceKey, 0);
						activeButtonIndex = 0;
				} else {
						activeButtonIndex = PlayerPrefs.GetInt (preferenceKey);
				}
								
		}

		void Start ()
		{
				buttons [getToggledActive ()].gameObject.SetActive (false);	
		}
	

		void Update ()
		{
				
				for (int i = 0; i < Input.touchCount; i++) {
					
						Touch touch = Input.GetTouch (i);	
					
						if (buttons [activeButtonIndex].HitTest (touch.position, Camera.main)) {
								if (!pushed) { 
										transform.position = new Vector2 (transform.position.x, transform.position.y - .01F);
										pushed = true;	
								}
						} 
				}


				if (Input.touchCount == 0) { 
					
						if (pushed) {
								SoundEffectsHelper.Instance.MakeButtonClick ();
							
								transform.position = new Vector2 (transform.position.x, transform.position.y + .01F);
								togglePreferences();
						}
						
						pushed = false;
				}
				
		}

		public void Activate() { 
			transform.gameObject.SetActive (true);
		}

		public void Deactivate() { 
			transform.gameObject.SetActive (false);
		}
			
		private void togglePreferences() {

			buttons [getToggledActive()].gameObject.SetActive (true);
			buttons [activeButtonIndex].gameObject.SetActive (false);
			
			activeButtonIndex = getToggledActive ();
			PlayerPrefs.SetInt (preferenceKey, activeButtonIndex);

			if (preferenceKey.Equals ("Sound")) {  // ugly special case for ahandling disabled sounds as soon as the option is selected 
				SoundEffectsHelper.Instance.updateSoundPrefs(activeButtonIndex);
			}

		}

		private int getToggledActive ()
		{
				if (activeButtonIndex == 0) {
						return 1;
				} else {
						return 0;
				}
		}
}

