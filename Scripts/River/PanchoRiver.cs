using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * 
 * 
 *    Singleton class that responsible for rendering the river as well
 * as managing some of the other aspects of the river (ie, the Obstacles);
 * 
 * Much of the game is scaled around the public variables exposed in this class
 * 
 * @Author Shaun Viguerie
 *  // https://github.com/playgameservices/play-games-plugin-for-unity/issues/276
 */
public class PanchoRiver : MonoBehaviour
{

		/*  for scaling the game  */
		public static PanchoRiver Instance;
		
		public float TileSize;
		public float BigWaterTileSize;
		public Transform OpenWater;

		/* these should probably be read only */
		public float LeftRiverBoundary;
		public float RightRiverBoundary;
		public float BottomRiverBoundary;
		public float TopRiverBoundary;
		public float CameraShiftOffset;  // need to define what this is....
	

		private RiverObstacleManager obstacleManager;
		private GUIManager guiManager;
		private GUIScoreCounter scoreCounter;
		private GameOverScript gameOverScript;
		private SwipeToStartPrompt swipeToStart;
		private CycleFishManager changeFish;	

		private bool startGame;

		public bool titleScene;
		public bool demoScene;

		private static int fallbackTimer = 20;  // keep an eye on this

		private int totalScore = 0; // was nothing

		private string leaderboardName = "Rocky River ";
		private string leaderboardID = "com.selloutsystems.rockyriver";

  
		void Awake ()
		{ 
				Application.targetFrameRate = 60;
									
				if (Instance != null) { 
						Debug.LogError ("Error, created more than 1 instance of pancho grid");
				}
    			
				Instance = this;
			
				guiManager = GetComponentInChildren<GUIManager> ();		
				scoreCounter = GetComponentInChildren<GUIScoreCounter> ();
				obstacleManager = GetComponent<RiverObstacleManager> ();
				gameOverScript = GetComponentInChildren<GameOverScript> ();
				
				swipeToStart = GetComponentInChildren<SwipeToStartPrompt> ();
				changeFish = GetComponentInChildren<CycleFishManager> ();
				startGame = false;
		}
  
		void Start ()
		{ 		                                                          
				obstacleManager.GenerateFreshObstacles ();
				constructRiver ();
				totalScore = 0;
				
				if (guiManager != null) { // guimanager is not null in the GAME SCENE!!! (it is NULL in the title scene!)
						guiManager.clearGUITexts ();
						startGame = true;
				} 
		
				if (fallbackTimer > 10) {
						leaderboardName += "Rookie";
						leaderboardID += "rookie";
				} else {
						leaderboardName += "Veteran";
						leaderboardID += "veteran";
				}

				if (!titleScene && !demoScene && !PlayerPrefs.HasKey(RockyKeys.CAN_SERVE_ADS) && PlayerPrefs.HasKey (RockyKeys.NUM_DEATHS_BEFORE_SCORING_20)) {
					int numDeathsWithoutScoring35 = PlayerPrefs.GetInt (RockyKeys.NUM_DEATHS_BEFORE_SCORING_20);
					if(numDeathsWithoutScoring35 == 3 || numDeathsWithoutScoring35 == 9 || numDeathsWithoutScoring35 == 20 || numDeathsWithoutScoring35 == 35 || numDeathsWithoutScoring35 == 40) { 
						TipsManager.Instance.ShowTips();
						startGame = false;
					}
				}
		}
		
		
		public List<int> getStartupMoves() {
			
			if (demoScene) { 
				return obstacleManager.getPathThroughRiver();
			}

			Debug.Log ("Error!! Trying to use startup moves in non startup scene!!");
			return null;
		}

		public void hideGameOver() {
			if (gameOverScript != null) {
				gameOverScript.disableTexture();
			}
		}

		public void showGameOver() {
			if (gameOverScript != null) {
				gameOverScript.enableTexture();
			}
		}

		public string getLeaderboardName() {
			return leaderboardName;
		}

		public string getLeaderboardId() { 
			return leaderboardID;
		}
		
		public int getTotalScore ()
		{ 
				return totalScore;
		}

		/**
		 * 
		 * This value SHOULD ONLY BE SET from the timer scene!!!!
		 */
		public void setTimerCount (int timer)
		{ 
				if (titleScene) {
						fallbackTimer = timer;
				}
		}

		public int getFallbackTime ()
		{
				return fallbackTimer;
		}
		
		public GUITexture getHowToButtonTexture() {
			return swipeToStart.getHowToTexture ();
		}
		
		public bool touchingSwitchFishButton(Vector2 touchPosition) {

			if (changeFish != null) {	
				return changeFish.getTexture().HitTest(touchPosition, Camera.main);			
			}

			return false;
		}
	

		/**
		 * called from SwipeDownToPaddle2 after the first move to hide the prompt!!!!
		 */
		public void HidePrompt ()
		{ 
				//	Time.timeScale = 1;
				guiManager.gameObject.SetActive (false);
				swipeToStart.Deactivate ();	
				
				if (changeFish != null) {
						changeFish.Deactivate ();
				}
				
				PauseButton.Instance.Activate ();
		}

		public void setStartGame() {
			startGame = true;		
		}

		public bool shouldStartGame ()
		{
				return startGame;
		}

		public void shiftRowsDown (int numRowsToShift, string currentScore, int numericScore)
		{ 
				shiftRowsDown (numRowsToShift);
				if(scoreCounter != null) { 
					scoreCounter.updateScore (currentScore, numericScore);			
				}

		}

		public void shiftRowsDown (int numRowsToShift)
		{ 			
				obstacleManager.ShiftRowsDown (numRowsToShift);				
		}

		public void shiftRowsUp (int numRowsToShift)
		{ 		
				obstacleManager.ShiftRowsUp (numRowsToShift);		
		}

		public float getLowestObstacleYPosition() { 
			if (obstacleManager != null) { 
				return obstacleManager.getLowestYPosition();
			}
			
			return 0F;
		}

		public float getLowestObstacleXPosition() { 
			if (obstacleManager != null) { 
				return obstacleManager.getLowestXPosition();
			}
			
			return 0F;
		}

		public float getBaseObstacleXPosition ()
		{
				if (obstacleManager != null) {
						return obstacleManager.getBaseXPosition ();
				}
	
				return 0F;
		}

		public void ClearScore ()
		{

				scoreCounter.clearScore ();
		}

		public void InitiateGameOverSequence (int distance, int speedBonus)
		{
				if (gameOverScript != null) { 
						AdMobManager.Instance.loadInterstitialAd();
						PauseButton.Instance.Deactivate ();
						gameOverScript.initiateGameOverSequeunce (distance, speedBonus);
				}
			
		}
		
		public void PromptRestartGUI (int distance, int speedBonus)
		{ 
				if (guiManager != null) {
						guiManager.gameObject.SetActive (true);
						guiManager.promptRestartGui (distance, speedBonus);
				}
				
				totalScore = distance + speedBonus;
		}
		
		private void constructRiver ()
		{ 
			
				for (float i = BottomRiverBoundary; i <=TopRiverBoundary; i+= BigWaterTileSize) { 
						
						for (float j = LeftRiverBoundary; j <= RightRiverBoundary; j+= BigWaterTileSize) {	
								ObjectPoolManager.CreatePooled (OpenWater.gameObject, new Vector3 (j + CameraShiftOffset, i, 0F), Quaternion.identity);	
						}
				}
	
		
		}
}