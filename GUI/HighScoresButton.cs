using UnityEngine;
using System.Collections;

using UnityEngine.SocialPlatforms;

#if UNITY_IPHONE
using UnityEngine.SocialPlatforms.GameCenter;
#endif

#if UNITY_ANDROID
using GooglePlayGames;
#endif

public class HighScoresButton : MonoBehaviour
{

		private GUITexture texture;
		private bool pushed;
		private int reloadDelay = 0;
		private bool auth = false;

		private bool attemptedGameCenterConnection;
		
		private readonly string UnreportedBestScoreRookie = "UnreportedBestRookie";
		private readonly string UnreportedBestScoreVeteran = "UnreportedBestVeteran";

		void Awake ()
		{
				texture = GetComponent<GUITexture> ();
		}
			
		void Start ()
		{
				reloadDelay = 1;

				if (PanchoRiver.Instance.titleScene) { // authenticate when the user signs into the game
				
						if (NetworkTester.Instance.isConnectedToNetwork ()) {
								authenticate ();		
								attemptedGameCenterConnection = true;
						} else {
								attemptedGameCenterConnection = false;
						}
				}

		}
	
		void Update ()
		{ 
		
				if (reloadDelay < 1) { 
						reloadDelay++;
				} else {
			
						for (int i = 0; i < Input.touchCount; i++) {
				
								Touch touch = Input.GetTouch (i);	
				
								if (texture.HitTest (touch.position, Camera.main)) {
										if (!pushed) { 
												transform.position = new Vector2 (transform.position.x, transform.position.y - .01F);
												pushed = true;							
										}
								} 
						}
				
						if (Input.touchCount == 0) { 
								if (pushed) {
										SoundEffectsHelper.Instance.MakeButtonClick ();
										transform.position = new Vector2 (transform.position.x, transform.position.y + .01F);

										pushed = false;
										reloadDelay = 0;

										if (!MedalManager.Instance.getAchievementsShowing ()) {
												if (Social.localUser.authenticated) { 
												
#if UNITY_ANDROID
													((PlayGamesPlatform)Social.Active).ShowLeaderboardUI (LeaderboardsManager.Instance.getActiveLeaderboardId());  // only display if user is authenticated
#elif UNITY_IPHONE
													Social.ShowLeaderboardUI();
#endif
												} else {
													
														if (NetworkTester.Instance.isConnectedToNetwork ()) {
																authenticate ();
														}

												}
										}
								}
						}
				}

				if (!attemptedGameCenterConnection && PanchoRiver.Instance.titleScene && NetworkTester.Instance.isConnectedToNetwork ()) {
						authenticate ();
						attemptedGameCenterConnection = true;
				}	
				
		}

		private void authenticate ()
		{ 
				
			#if !UNITY_EDITOR
				if (!Social.localUser.authenticated) {
						Social.localUser.Authenticate (ProcessAuthentication);
				} else {
						auth = true;
				} 
			#endif
		}	

		/**
	 * 
	 * authentification callback!!
	 */
		void ProcessAuthentication (bool success)
		{
				if (success) {
						
						auth = true;
							
						if (PlayerPrefs.HasKey (UnreportedBestScoreRookie)) {
								ReportScore (PlayerPrefs.GetInt ("RookieBestScore"), LeaderboardsManager.Instance.getRookieLeaderboardId(), false);
						} else if (PlayerPrefs.HasKey (UnreportedBestScoreVeteran)) {
								ReportScore (PlayerPrefs.GetInt ("VeteranBestScore"), LeaderboardsManager.Instance.getVeteranLeaderboardId(), true);
						}
				} 
		}

		private void ReportScore (int score, string leaderboardID, bool veteran)
		{	
				Social.ReportScore (score, leaderboardID, success => {
						Debug.Log (success ? "Reported score to leaderboard successfully" : "Failed to report score");
						
						if (success) {
								if (veteran) {
										PlayerPrefs.DeleteKey (UnreportedBestScoreVeteran);
								} else {
										PlayerPrefs.DeleteKey (UnreportedBestScoreRookie);
								}
						}
				});
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

