using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using System.IO;
#if UNITY_IPHONE
using UnityEngine.SocialPlatforms.GameCenter;
#endif

#if UNITY_ANDROID
using GooglePlayGames;
#endif



/**
 * @Author Shaun Viguerie
 * 
 * (C) %SelloutSystems
 */
public class GUIManager : MonoBehaviour
{
		private GUIText[]   scoreAndDistance;
		private GUITexture[] buttons;
		private GUITextureButton[] buttonScripts;
		private bool  restartGame = false;
		
		private float distance;

		private int playerScore;
		private int playerSpeedBonus;

		private string scoreKey = "BestScore";
		private int bestScore;
		
		private int overallBestScore;

		private PlayerPrefsManager playerPlayerPrefsManager;
		private string prefix;

		private FacebookShareScript facebookButton;
		private TwitterShareScript twitterButton;
		
		public static GUIManager Instance;
		
		private readonly string UnreportedBestScore = "UnreportedBest";

		private string path;
		private byte[] pngBytes;
		private float scoreWindowY;


		void Awake ()
		{
				if (Instance == null) {
						Instance = this;
				}

				scoreAndDistance = GetComponentsInChildren<GUIText> ();
				buttons = GetComponentsInChildren<GUITexture> ();

				buttonScripts = GetComponentsInChildren<GUITextureButton> ();

				facebookButton = GetComponentInChildren<FacebookShareScript> ();
				twitterButton = GetComponentInChildren<TwitterShareScript> ();
	  		 	
				if (PanchoRiver.Instance.getFallbackTime () > 10) { 
						prefix = "Rookie";
				} else {
						prefix = "Veteran";
				}
		
				playerPlayerPrefsManager = new PlayerPrefsManager (prefix);

				bestScore = playerPlayerPrefsManager.getBestScore ();
				overallBestScore = playerPlayerPrefsManager.getOverallBestScore ();

				path = "file://" + System.IO.Path.Combine (Application.persistentDataPath, "screenshot.png");
				
		}	

		void Start ()
		{ 
				if (scoreAndDistance == null) {
						scoreAndDistance = GetComponentsInChildren<GUIText> ();
				}

				scoreWindowY = buttons [0].pixelInset.y; //  + buttons[0].pixelInset.height;
				
				buttons [0].gameObject.SetActive (false);	 // hide the score button 				

				buttonScripts [0].Deactivate ();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
				buttonScripts [1].Deactivate ();
				buttonScripts [2].Deactivate ();
				
				facebookButton.gameObject.SetActive (false);
				twitterButton.gameObject.SetActive (false);

				bestScore = PlayerPrefs.GetInt (playerPlayerPrefsManager.getBestScoreKey ());
		}

		public int getBestScore() { 
			return bestScore;	
		}
		
		public byte[] getScreenshotBytes() { 
			return pngBytes;
		}

		public float getScoreWindowYPosition() {
			//return 		buttons [0].pixelInset.y + buttons[0].pixelInset.height;
			return scoreWindowY;
		}
		
		public void enableFacebookAndTwitter ()
		{

				facebookButton.gameObject.SetActive (true);
				twitterButton.gameObject.SetActive (true);
		}

		private void activateRestartUI ()
		{					
	
				
				buttonScripts [0].Activate ();
				buttonScripts [1].Activate ();
				buttonScripts [2].Activate ();
					
				facebookButton.gameObject.SetActive (true);
				twitterButton.gameObject.SetActive (true);
				
				MedalManager.Instance.didReceiveNewAchievement (playerScore, playerSpeedBonus);
			
				Invoke ("preloadScreenshot", .2F);
				
		}
	
		public void clearGUITexts ()
		{ 		
				clear (0, 4);
		}
			
		private void clear (int start, int stop)
		{
				for (int i = start; i < scoreAndDistance.Length; i++) {
						scoreAndDistance [i].text = "";
				}
		}
  
		public void promptRestartGui (int distance, int speedBonus)
		{ 					
				buttons [0].gameObject.SetActive (true);
				buttons [buttons.Length - 1].gameObject.SetActive (false);
				buttons [buttons.Length - 2].gameObject.SetActive (false);
				
				clear (4, 8);
				restartGame = true;

				int totalScore = distance + speedBonus;

				scoreAndDistance [0].text = "Distance: " + distance;
				scoreAndDistance [1].text = "Speed Bonus: " + speedBonus;

				scoreAndDistance [2].text = "Total Score: " + totalScore;

				if (totalScore > bestScore) {
				
						scoreAndDistance [3].color = new Color32 (145, 15, 240, 255);
						scoreAndDistance [3].text = prefix + " Best: " + totalScore;
						
						bestScore = totalScore;
						
						if (Social.localUser.authenticated) {
								ReportScore (bestScore, LeaderboardsManager.Instance.getActiveLeaderboardId());
						}
						else {
								PlayerPrefs.SetInt(UnreportedBestScore + prefix, 1);	
						}
		
				} else {
						scoreAndDistance [3].text = prefix + " Best: " + bestScore;
				}
			
				playerPlayerPrefsManager.updatePrefs (totalScore);

				Application.CaptureScreenshot ("screenshot.png");

				playerScore = totalScore;
				playerSpeedBonus = speedBonus;

			
				Invoke ("activateRestartUI", .6F);
		}
		
		private void preloadScreenshot() { 
			#if !UNITY_EDITOR
				if (NetworkTester.Instance.isConnectedToNetwork ()) { 
						StartCoroutine (DownloadImage ());	
				} else {	
						StartCoroutine (DownloadImage());	
						// activateRestartUI();
				}
			#endif				
		}
		
		private void ReportScore (int score, string leaderboardID)
		{
				//Debug.Log ("Reporting score " + score + " on leaderboard " + leaderboardID);
				Social.ReportScore (score, leaderboardID, success => {
						Debug.Log (success ? "Reported score to leaderboard successfully" : "Failed to report score");
						if(success) {
							if(PlayerPrefs.HasKey(UnreportedBestScore + prefix)) {
									PlayerPrefs.DeleteKey(UnreportedBestScore + prefix);  
							}
						}
						else {
								PlayerPrefs.SetInt(UnreportedBestScore + prefix, 1);	
						}
				});
		}
	
		/**
		 * load the image in a coroutine
		 */
		IEnumerator DownloadImage() { 
				
				WWW www = new WWW (path);
				yield return www;
				pngBytes = www.bytes;
				
				//		activateRestartUI ();
		}

}
