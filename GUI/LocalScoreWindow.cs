using UnityEngine;
using System.Collections;

public class LocalScoreWindow : MonoBehaviour
{

		public static LocalScoreWindow Instance;
		private GUITexture scoreWindow;
		private GUIText[] scoreAndDistance;
		private PlayerPrefsManager playerPrefsManager;
		private string prefix;
		


		// Use this for initialization
		void Awake ()
		{

				if (Instance == null) {
						Instance = this;
				}
			
				scoreWindow = GetComponentInChildren<GUITexture> ();
				scoreAndDistance = GetComponentsInChildren<GUIText> ();	
				
		}
		
		void Start ()
		{
				if (PanchoRiver.Instance.getFallbackTime () > 10) { 
						prefix = "Rookie";
				} else {
						prefix = "Veteran";
				}
		
				playerPrefsManager = new PlayerPrefsManager (prefix);
				
				
				hideAll ();		
		}
				
		private void hideAll ()
		{
				scoreWindow.enabled = false;
				for (int i =0; i < scoreAndDistance.Length; i++) {
						scoreAndDistance [i].enabled = false;
				}
		}

		private void showAll ()
		{
				scoreWindow.enabled = true;
				for (int i =0; i < scoreAndDistance.Length; i++) {
						scoreAndDistance [i].enabled = true;
				}
		}
		
		public void showLocalBestScores ()
		{	
				showAll ();
			
				scoreAndDistance [0].text = "Best " + prefix + " Runs: ";	
			
				scoreAndDistance [1].text = "\t\t 1. " + playerPrefsManager.getBestScore ();
				scoreAndDistance [2].text = "\t\t 2. " + playerPrefsManager.getSecondBestScore ();
				scoreAndDistance [3].text = "\t\t 3. " + playerPrefsManager.getThirdBestScore ();		
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

