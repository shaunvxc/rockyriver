using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour {
	
	private ClickAndHideButton button;
	public static TutorialManager Instance;

	private static int numTutorialRuns  = 0 ;


	void Awake () {
			button = GetComponentInChildren<ClickAndHideButton> ();

			if (Instance == null) {
					Instance = this;
			}
			
	}


	void Start() { 
		
		if (numTutorialRuns >= 5) { 
						button.Deactivate ();
		} else {
						numTutorialRuns++;
		}
	}


	void Update() { 

			if (!button.isActive ()) {
			//	PanchoRiver.Instance.unlockAndStartGame();			
			}
	}


	public void Deactivate() { 
		gameObject.SetActive (false);
	}


}
