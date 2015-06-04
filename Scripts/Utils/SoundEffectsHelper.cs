using UnityEngine;
using System.Collections;

public class SoundEffectsHelper : MonoBehaviour
{
	
		public static SoundEffectsHelper Instance;
		
		// public instances, injected into code via Unity
		public AudioClip swimSound;
		public AudioClip buttonClick;
		public AudioClip deathSound;	
		
		private bool muted = true;			
		
		private Transform _transform;

		void Awake ()
		{
				// Register the singleton
				if (Instance != null) {
						Debug.LogError ("Multiple instances of SoundEffectsHelper!");
				}
		
				Instance = this;
				_transform = transform;
		}

		void Start() {
							
				if (!PlayerPrefs.HasKey ("Sound")) {
						muted = true;
				} else {
						if(PlayerPrefs.GetInt("Sound") == 0) {
							muted = true;
						}
						else {
							muted = false;
						}
				}
		}


		public void updateSoundPrefs(int soundPref) {
				if (soundPref == 0) { 
						muted = true;
				} else {
						muted = false;
				}
		}

		
		public void MakeSwimSound () {
				MakeSound (swimSound);
		}
		
		public void MakeButtonClick() { 
			//	MakeSound (buttonClick);
		}

		public void MakeDeathSound() { 
				MakeSound (deathSound);
		}
		
		private void MakeSound (AudioClip originalClip)
		{	

				if (!muted) {
						AudioSource.PlayClipAtPoint (originalClip, _transform.position);
				}
		}
}
