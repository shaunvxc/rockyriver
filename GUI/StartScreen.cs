using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour
{

		public static StartScreen Instance ;

		public static int RELOAD_LIMIT = 10;

		private GUITextureButton[] buttonScripts;
		private TogglePreferenceButton[] settingsButtons;

		private GUIText forHaraldText;

		void Awake ()
		{		
				if (Instance == null) {
						Instance = this;
				}

				buttonScripts = GetComponentsInChildren<GUITextureButton> ();
				settingsButtons = GetComponentsInChildren<TogglePreferenceButton> ();
				
		}
		
		void Start ()
		{ 
				buttonScripts [2].Deactivate ();
				buttonScripts [3].Deactivate ();
				//buttonScripts [4].Deactivate (); // keep rate showing for now
				buttonScripts [5].Deactivate (); // deactivate home	
					
				settingsButtons [1].Deactivate ();
		}

		public void showSecondDialog ()
		{ 
		//		buttons [1].GetComponent<GUITextureButton> ().Deactivate ();

				buttonScripts [1].Deactivate ();
				buttonScripts [2].Activate ();
				buttonScripts [3].Activate ();
				
				buttonScripts [4].Deactivate (); // deactivate rate
				buttonScripts [5].Activate (); // activate home
    
				settingsButtons [0].Deactivate ();
				settingsButtons [1].Activate ();
				MedalManager.Instance.disableMedalsButton ();
				/*
				if (forHaraldText != null) {
						buttonScripts [5].Activate ();
						forHaraldText.gameObject.SetActive (true);
				} */

		}
}
