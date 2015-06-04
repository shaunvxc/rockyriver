using UnityEngine;
using System.Collections;

public class GUITextureButton : MonoBehaviour
{


		private GUITexture texture;
		public bool reloadCurrentLevel;
		public string levelToLoad;
		private bool pushed;
		public bool startGameButton;
		public 	int fallbackTimerSetting;


		public bool hideOnPush;
		public bool enableFacebookAndTwitter;

		public bool showAchievements;
		public bool hideAchievements;
	
		public bool showTips;
		public bool hideTips;
	
		public bool shiftRiverDown;

		private int reloadDelay = 0;
		private bool isTitleScene;
					
		private Transform _transform;
		
		public bool demoScene; // this is an ugly hack!!!! So that we don't throw a null pointer when pressing the X button from the demo scene without an Achievmentmanager

		void Awake ()
		{
				texture = GetComponent<GUITexture> ();
				_transform = transform;
		}

		void Start ()
		{
				reloadDelay = 1;
				isTitleScene = PanchoRiver.Instance.titleScene;
		}

		void Update ()
		{ 

				
				for (int i = 0; i < Input.touchCount; i++) {
			
						Touch touch = Input.GetTouch (i);	
				
						if (texture.HitTest (touch.position, Camera.main)) {
								if (!pushed) { 
										_transform.position = new Vector3 (_transform.position.x, _transform.position.y - .01F, _transform.position.z);
										pushed = true;	
								}
						} 
				}

				
				if (Input.touchCount == 0) { 	
						
						if (pushed) {

								_transform.position = new Vector3 (_transform.position.x, _transform.position.y + .01F, _transform.position.z);
								
								// TODO this absurd IF ladder NEEDS to be CLEANED UP!!! Make extensions of this base class, and call an abstract method executeAction() from here!!
								// too many actions being handled by 1 class!!
								if (hideOnPush && !MedalManager.Instance.getAchievementsShowing()) { // this should only be used for buttons on the home screen
								
										if (enableFacebookAndTwitter) {
												GUIManager.Instance.enableFacebookAndTwitter ();
										} 
										else if(PanchoRiver.Instance.titleScene) {
												StartScreen.Instance.showSecondDialog ();
										} 
										
										pushed = false;		
										Deactivate ();
								}
								else if(showAchievements) { //  && !AchievementManager.Instance.getAchievementsShowing()) {
										
										pushed = false;
										MedalManager.Instance.DisplayAchievementsWindow();
								}
								else if(hideAchievements) {
										
										pushed = false;
										MedalManager.Instance.HideAchievements();
								}
								else if(showTips) {
										pushed = false;
										TipsManager.Instance.ShowTips();
								}
								else if(hideTips) {
										pushed = false;
										TipsManager.Instance.HideTips();
								}
								else if(shiftRiverDown) { 	
										pushed = false;
										PanchoRiver.Instance.shiftRowsDown(1);
								}
								else if (reloadCurrentLevel) {
										pushed = false;
										
										if (!isTitleScene) {
												AdMobManager.Instance.showAdIfEligible ();
										}  
										
										Application.LoadLevel (Application.loadedLevel);
			
								} else if (levelToLoad != null && !string.Equals (levelToLoad, "") && (demoScene || !MedalManager.Instance.getAchievementsShowing())) {
								
										
										pushed = false;
									
										if (startGameButton) {
												PanchoRiver.Instance.setTimerCount (fallbackTimerSetting);
										}
										
										if(string.Equals (levelToLoad, "TitleScene") && !isTitleScene) { 
												AdMobManager.Instance.showAdIfEligible();
										}
										
										Application.LoadLevel (levelToLoad);
								} else {
										pushed = false;
								}

								reloadDelay = 0;
						}
				}
		}
		
		public GUITexture getTexture() { 
			return texture;
		}

		public void Activate ()
		{ 
				transform.gameObject.SetActive (true);
		}
				
		public void Deactivate ()
		{
				transform.gameObject.SetActive (false);
		}
}
