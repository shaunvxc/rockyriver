using UnityEngine;
using System.Collections;

public class LocalizationStringManager : MonoBehaviour
{

		public static LocalizationStringManager Instance;

		private string distance;
		private string speedBonus;
		private string totalScore;
		private string best;
		private string modePrefix;

		private bool rookie = false;

		void Awake ()
		{ 
				if (Instance == null) { 
						Instance = this;
				}
		}

		void Start ()
		{
				if (PanchoRiver.Instance.getFallbackTime () > 10) { 
					rookie = true;
				} else {
					rookie = false;
				}
				
				
				
				if (Application.systemLanguage == SystemLanguage.English) {
					
					distance = "Distance: ";
					speedBonus = "Speed Bonus: ";
					totalScore = "Total Score: ";	
					best = " Best: ";
					
					if (rookie) {
						
					} else {
						
					}
					
				} else if (Application.systemLanguage == SystemLanguage.Chinese) {
					
				} else if (Application.systemLanguage == SystemLanguage.Japanese) {
					
				} else {
					
				}
	
		}

		public string getDistanceString() {
			return distance;		
		}

		public string getSpeedBonusString() { 	
			return speedBonus;
		}

		public string getTotalScoreString() { 
			return totalScore;
		}

		public string getModePrefixString() { 
			return modePrefix;		
		}

		public string getBestString() {
			return best;
		}
}

