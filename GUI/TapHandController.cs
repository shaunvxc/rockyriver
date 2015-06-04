using UnityEngine;
using System.Collections;

public class TapHandController : MonoBehaviour
{	
		
		public static TapHandController Instance;
		private TapHand rightHand;
		private TapHand leftHand;
		
		private TapHand fallback;
		
		private bool normal;

		void Awake ()
		{ 
				
				if (Instance == null) { 
						Instance = this;
				}

				if (!PlayerPrefs.HasKey ("Controls")) {
						normal = true;
				} else {
						if (PlayerPrefs.GetInt ("Controls") == 0) {
								normal = true;
						} else { 
								normal = false;
						}
				}
    
    
		}
  
		void Start ()
		{
				var tapHands = GetComponentsInChildren<TapHand> ();	
			
				leftHand = tapHands [0];
				rightHand = tapHands [1];
				fallback = tapHands [2];

		}
	
		public void renderLeftHand ()
		{
				fallback.disableRenderer ();
			
				if (normal) {
						rightHand.disableRenderer ();
						leftHand.enableRenderer ();
				} else {
						leftHand.disableRenderer ();
						rightHand.enableRenderer ();
				}
		}

		public void renderRightHand ()
		{ 		
				fallback.disableRenderer ();
				
				if (normal) { 
						leftHand.disableRenderer ();
						rightHand.enableRenderer ();
				} else {
						rightHand.disableRenderer ();
						leftHand.enableRenderer ();
				}
		}
		

		public void renderFallback() { 
			//hideBothHands ();
			fallback.enableRenderer ();
		}
		
		
		public void hideBothHands ()
		{ 
				leftHand.disableRenderer ();	
				rightHand.disableRenderer ();
		}
}

