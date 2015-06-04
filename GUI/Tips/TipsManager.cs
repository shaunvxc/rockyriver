using UnityEngine;
using System.Collections;

public class TipsManager : MonoBehaviour
{

		public static TipsManager Instance;
		private GUITexture tipsWindow;
		
		private GUIText[] tips;
		private GUITextureButton exitButton;
		private bool active = false;

		void Awake ()
		{ 

				if (Instance == null) { 
						Instance = this;
				}
				
				tips = GetComponentsInChildren<GUIText> ();
				tipsWindow = GetComponentInChildren<GUITexture> ();		
				
				exitButton = GetComponentInChildren<GUITextureButton> ();
				HideTipsSafe ();
    
 		 }
		

		public bool getActive() {
			return active;
		}
	
		public void ShowTips() { 
			active = true;
			tipsWindow.gameObject.SetActive (true);

			for(int i = 0; i < tips.Length; i++) {
				tips[i].gameObject.SetActive(true);
			}
			
			exitButton.Activate ();
  		}	
  		private void HideTipsSafe() { 
			active = false;
			tipsWindow.gameObject.SetActive (false);
			
			for(int i = 0; i < tips.Length; i++) {
				tips[i].gameObject.SetActive(false);
			}
			
			exitButton.Deactivate ();
 	 }
 
		public void HideTips() { 
			
			HideTipsSafe ();
			if (!PanchoRiver.Instance.titleScene && !PanchoRiver.Instance.demoScene) { 
					PanchoRiver.Instance.setStartGame();
			}
		}

}

