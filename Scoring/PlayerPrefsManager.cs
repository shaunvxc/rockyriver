using UnityEngine;
using System.Collections;

public class PlayerPrefsManager
{

		private string BestScore;
		private string SecondBest ;
		private string ThirdBest;
		private int best;
		private int secondBest;
		private int thirdBest;
		
		private int overallBestScore;

		public PlayerPrefsManager (string prefix)
		{ 
				
				BestScore = prefix + "BestScore";
				SecondBest = prefix + "SecondBest";
				ThirdBest = prefix + "ThirdBest";

				init ();
		}

		public void updatePrefs (int score)
		{
				if (score > best) {
						setNewBest (score);							
				} else if (score > secondBest) {
						setSecondBest (score);
				} else if (score > thirdBest) {
						setThirdBest (score);
				}

				updateAdStatus (score);


		}
		
		public void init ()
		{

				if (!PlayerPrefs.HasKey (BestScore)) {
						best = 0;
						PlayerPrefs.SetInt (BestScore, 0);
				} else {
						best = PlayerPrefs.GetInt (BestScore);
				}
		
				if (!PlayerPrefs.HasKey (SecondBest)) {
						secondBest = 0;
						PlayerPrefs.SetInt (SecondBest, 0);
				} else {
						secondBest = PlayerPrefs.GetInt (SecondBest);
				}
		
				if (!PlayerPrefs.HasKey (ThirdBest)) {
						thirdBest = 0;
						PlayerPrefs.SetInt (ThirdBest, 0);
				} else {
						thirdBest = PlayerPrefs.GetInt (ThirdBest);
				}

				if (!PlayerPrefs.HasKey (RockyKeys.OVERALL_BEST)) {
					
						if(best == 0) { 
							overallBestScore = 0;
							PlayerPrefs.SetInt (RockyKeys.OVERALL_BEST, 0);
							if(!PlayerPrefs.HasKey(RockyKeys.FIRST_RUN)) {
								PlayerPrefs.SetInt(RockyKeys.FIRST_RUN, 1);
								Application.LoadLevel(LevelKeys.DemoScene);
							} 
						} else {
							
							PlayerPrefs.SetInt (RockyKeys.OVERALL_BEST, best);
							overallBestScore = best;
						
							if(!PlayerPrefs.HasKey (RockyKeys.FIRST_RUN)) {
								PlayerPrefs.SetInt(RockyKeys.FIRST_RUN, 1);
							}
							
						}

				} else {
					overallBestScore = PlayerPrefs.GetInt (RockyKeys.OVERALL_BEST);
				}
				
				if (!PlayerPrefs.HasKey (RockyKeys.CAN_SERVE_ADS)) {
					if (overallBestScore > LevelKeys.MinAdEligibleScore) {
						PlayerPrefs.SetInt (RockyKeys.CAN_SERVE_ADS, 1);
					}
				}

				

				
		}
		
		/**
		 * To only be called after dying
		 */
		private void updateAdStatus(int totalScore) { 
				
				if (!PlayerPrefs.HasKey (RockyKeys.CAN_SERVE_ADS)) {
						if (totalScore > LevelKeys.MinAdEligibleScore) {
								PlayerPrefs.SetInt (RockyKeys.CAN_SERVE_ADS, 1);
						} else {
								// need to signal for the next round to show the tips button!
								if (!PlayerPrefs.HasKey (RockyKeys.NUM_DEATHS_BEFORE_SCORING_20)) {
										PlayerPrefs.SetInt (RockyKeys.NUM_DEATHS_BEFORE_SCORING_20, 1);
								} else {
										int numDeathsWithoutScoring35 = PlayerPrefs.GetInt (RockyKeys.NUM_DEATHS_BEFORE_SCORING_20);
										numDeathsWithoutScoring35++;
										PlayerPrefs.SetInt (RockyKeys.NUM_DEATHS_BEFORE_SCORING_20, numDeathsWithoutScoring35);		
								}
						}
				}
		}
			
		public int getOverallBestScore() { 
			return overallBestScore;
		}

		public int getBestScore ()
		{ 
				return best;
		}
		
		public int getSecondBestScore ()
		{ 
				return secondBest;
		}

		public int getThirdBestScore ()
		{
				return thirdBest;
		}

		public string getBestScoreKey ()
		{ 
				return BestScore;
		}

		public string getSecondBestScoreKey ()
		{ 
				return SecondBest;
		}

		public string getThirdBestScoreKey ()
		{ 
				return ThirdBest;
		}

		private void reinit ()
		{
				init ();
		}

		private void setNewBest (int score)
		{
				PlayerPrefs.SetInt (BestScore, score);
				PlayerPrefs.SetInt (SecondBest, best);
				PlayerPrefs.SetInt (ThirdBest, secondBest);
				reinit ();
		}
	
		private void setSecondBest (int score)
		{
				PlayerPrefs.SetInt (SecondBest, score);
				PlayerPrefs.SetInt (ThirdBest, secondBest);
				reinit ();
		}

		private void setThirdBest (int score)
		{
				PlayerPrefs.SetInt (ThirdBest, score);	
				reinit ();
		}

}

