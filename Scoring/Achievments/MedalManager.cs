using UnityEngine;
using System.Collections;

public class MedalManager : MonoBehaviour
{
		
		public static MedalManager Instance;
		private AchievementButton[] achievements;
		private GUIText[] medalNames;
		private GUITextureButton showAchievments;
		private GUITextureButton exitButton;
		private GUITextureButton newAchievements;
		private GUITexture rookieAchievements;
		private GUITexture veteranAchievements;
		private GUITexture[] textures;
		public  Texture questionMark;
		private int indexOfHighestAchievement;
		private readonly string achievementLevel = "AchievementLevel";
		private TabButton[] tabs;

		private bool rookieMode;
		private readonly string achievementCount = "AchievementCount";
		private bool completedAchievements = false;
		
		private bool achievementsShowing = false;					
		
		void Awake ()
		{ 
				if (Instance == null) {
						Instance = this;	
				}
				
				achievements = GetComponentsInChildren<AchievementButton> ();	
				medalNames = GetComponentsInChildren<GUIText> ();	
			
				var buttons = GetComponentsInChildren<GUITextureButton> ();
				showAchievments = buttons [0];
				exitButton = buttons [1];
				newAchievements = buttons [2];
				
				var textures = GetComponentsInChildren<GUITexture> ();				
				
				rookieAchievements = textures [0];
				veteranAchievements = textures [1];
				
				tabs = GetComponentsInChildren<TabButton> ();
				
				if (PlayerPrefs.HasKey (achievementCount) && PlayerPrefs.GetInt (achievementCount) == 8) { 
						completedAchievements = true;
				} else if (!PlayerPrefs.HasKey (achievementCount)) {
						PlayerPrefs.SetInt (achievementCount, 0);
				} 
						
		}
		
		void Start ()
		{

				deactivateAll ();
				
					
				if (PanchoRiver.Instance.getFallbackTime () > 10) {
						rookieMode = true;
				} else {
						rookieMode = false;
				}
			
				
				
		}
		
		public void disableMedalsButton ()
		{
				if (PanchoRiver.Instance.titleScene) { // if it is the title scene...
						showAchievments.Deactivate ();
				}
		}

		private void deactivateAll ()
		{
				if (!PanchoRiver.Instance.titleScene) {
						showAchievments.Deactivate ();
				}
				
				exitButton.Deactivate ();
				newAchievements.Deactivate ();
				
				hideAchievementWindows ();
				hideTabs ();
				
				hideMedalNames ();
				
				for (int i = 0; i < achievements.Length; i++) {	
						achievements [i].Deactivate ();
				}
		}

		private void hideMedalNames ()
		{
				for (int i =0; i < medalNames.Length; i++) {
						medalNames [i].gameObject.SetActive (false);
				}
		}

		private void showMedalNames ()
		{
				for (int i =0; i < medalNames.Length; i++) {
						medalNames [i].gameObject.SetActive (true);
				}
		}

		private void hideAchievementWindows ()
		{
				rookieAchievements.gameObject.SetActive (false);
				veteranAchievements.gameObject.SetActive (false);
		}

		private void hideTabs ()
		{
				tabs [0].Deactivate ();
				tabs [1].Deactivate ();
		}
		
		private void showTabs ()
		{	
				tabs [0].Activate ();
				tabs [1].Activate ();

				if (PanchoRiver.Instance.getFallbackTime () > 10 || PanchoRiver.Instance.titleScene) {
						tabs [0].SetSelected (true);	
						//	tabs [1].SetSelected (false);	
						rookieAchievements.gameObject.SetActive (true);
						renderRookieAchievements ();
							
				} else {
						//	tabs [0].SetSelected (false);	
						tabs [1].SetSelected (true);
						veteranAchievements.gameObject.SetActive (true);
						
						renderVeteranAchievements ();
				}
											
		}
		
		public void showRookie ()
		{ 
				veteranAchievements.gameObject.SetActive (false);
				rookieAchievements.gameObject.SetActive (true);
				renderRookieAchievements ();			
				tabs [0].toggle ();
				tabs [1].toggle ();
		}

		public void showVeteran ()
		{ 


				rookieAchievements.gameObject.SetActive (false);
				veteranAchievements.gameObject.SetActive (true);
				renderVeteranAchievements ();
				tabs [0].toggle ();
				tabs [1].toggle ();
		}
		
		public void renderRookieAchievements ()
		{
				
				for (int i = 0; i < 8; i++) {
						if (i <= 3) {
							achievements [i].RenderAchievement ();
							medalNames [i].text = achievements [i].getName ();

							if(i == 3 && achievements[i].getName ().Contains ("?")) {
								achievements[i].setPixens(achievements[0].getPixens());
							}

						} else {
								achievements [i].Deactivate ();
						}
				}
		}
		
		public void renderVeteranAchievements ()
		{
					
				for (int i = 0; i < 8; i++) {	
						if (i <= 3) {
								achievements [i].Deactivate ();
						} else {
							
								achievements [i].RenderAchievement ();
								medalNames [i - 4].text = achievements [i].getName ();
								
								if (i == 7 && achievements [i].getName ().Contains ("?")) {
										achievements [i].setPixens (achievements [i - 3].getPixens ());
								}
						}
				}
		}
		
		public bool getAchievementsShowing() {
			return achievementsShowing;
		}

		public void DisplayAchievementsWindow ()
		{


				showAchievments.Deactivate ();
				
				
				newAchievements.Deactivate ();
				
				if (PanchoRiver.Instance.titleScene) {
						RockyRiverHider.Instance.Deactivate ();
				} else {
						PanchoRiver.Instance.hideGameOver ();
				}
				
				showMedalNames ();	
				showTabs ();
				
				exitButton.Activate ();
				
				achievementsShowing = true;
		}

		public void HideAchievements ()
		{

				deactivateAll ();
				if (!PanchoRiver.Instance.titleScene) {
						PanchoRiver.Instance.showGameOver ();
				} else {	
						RockyRiverHider.Instance.Activate();
					
						showAchievments.Activate();
				}	

				achievementsShowing = false;

		}
		

		public void didReceiveNewAchievement (int score, int speedBonus)	
		{
				
				if (!completedAchievements) {
						
						bool gotNewAchievement = false;	

						if (rookieMode) {
								if (earnedNewRookieMedal (score, speedBonus)) {
										gotNewAchievement = true;
								}
						} else {
								if (earnedNewVeteranMedal (score, speedBonus)) {
										gotNewAchievement = true;			
								}
						}	

						if (gotNewAchievement) {
								newAchievements.Activate ();	
						}
				}
		}

		private bool earnedNewRookieMedal (int score, int speedBonus)
		{

				bool ret = false;
				for (int i = 0; i <= 3; i++) {
						if (!PlayerPrefs.HasKey (achievements [i].name)) {

								if (achievements [i].scoreMeetsCriteria (score, speedBonus)) {
										Debug.Log ("Earned new rookie  medal!!");
										PlayerPrefs.SetInt (achievements [i].name, 1);	
										PlayerPrefs.SetInt (achievementCount, PlayerPrefs.GetInt (achievementCount) + 1);

										ret = true;			

								} else {
										break;
								}
						}
				}
				
				return ret;
		}

		private bool earnedNewVeteranMedal (int score, int speedBonus)
		{	
				bool ret = false;

				for (int i = 4; i < achievements.Length; i++) {
				
						if (!PlayerPrefs.HasKey (achievements [i].name)) {

								if (achievements [i].scoreMeetsCriteria (score, speedBonus)) {
										Debug.Log ("Earned new rookie medal!!");
										PlayerPrefs.SetInt (achievements [i].name, 1);	
										PlayerPrefs.SetInt (achievementCount, PlayerPrefs.GetInt (achievementCount) + 1);

										ret = true;			
								} else {
										break;
								}
						}
				}
							
				return ret;
		}
}

