using UnityEngine;
using System.Collections;

public class ClickAndHideButton : MonoBehaviour
{

	private GUITexture texture;

	private bool pushed;
	
	public bool hideOnPush;
	
	private int reloadDelay = 0;
	
	void Awake() {
		texture = GetComponent<GUITexture> ();
	}
	
	void Start() {
		reloadDelay = 1;
	}
	
	void Update() { 
		
		if (reloadDelay < 1) { 
			reloadDelay++;
		} else {
			
			for (int i = 0; i < Input.touchCount; i++) {
				
				Touch touch = Input.GetTouch (i);	
				
				if (texture.HitTest (touch.position, Camera.main)) {
					if (!pushed) { 
					//	SoundEffectsHelper.Instance.MakeButtonClick();
						transform.position = new Vector2 (transform.position.x, transform.position.y - .01F);
						pushed = true;
					}
				} 
			}
			
			
			if (Input.touchCount == 0) { 
				if (pushed) {
					SoundEffectsHelper.Instance.MakeButtonClick();
					transform.position = new Vector2 (transform.position.x, transform.position.y + .01F);
					
					if(hideOnPush) { // this should only be used for buttons on the home screen
						//StartScreen.Instance.flipHighScore();
					//	TutorialManager.Instance.incrementButtons();
						Deactivate();
					}

					pushed = false;
					reloadDelay = 0;
				}
			}
		}
	}


	public bool isActive() { 

		return transform.gameObject.activeSelf;
	}

	public void Activate() { 
		transform.gameObject.SetActive (true);
	}
	
	public void Deactivate()  {
		transform.gameObject.SetActive (false);
	}
}

