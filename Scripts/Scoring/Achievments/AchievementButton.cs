using UnityEngine;
using System.Collections;

public class AchievementButton : MonoBehaviour
{				
		public int scoreBenchmark;
		public int speedBonusBenchmark;
			
		public int achievementLevel;
		
		public string name;
		
		private GUITexture button;
		private GUIText text;
		
		
		void Awake ()
		{
			button = GetComponent<GUITexture> ();						
			text = GetComponent<GUIText> ();
		}

		void Start() {				
			//gameObject.SetActive (false);
			//		text.text = name + "";
		}
		
		
		public bool scoreMeetsCriteria(int score, int speedBonus) {
	
				if (speedBonusBenchmark == 0) {
						return score >= scoreBenchmark;
				} else {
						return score >= scoreBenchmark && speedBonus >= speedBonusBenchmark;
				}
				
		}
		
		public void Activate() {
			gameObject.SetActive (true);
		}
		
		public void Deactivate() { 
			gameObject.SetActive (false);
		}

		
		public string getName() {
			if (PlayerPrefs.HasKey(name)) {
				return name;
			}
			
			return "????";
		}
		
		public void RenderAchievement() {
						
				if (PlayerPrefs.HasKey(name)) {
						gameObject.SetActive (true);	
				} else {
						
						gameObject.SetActive (true);
						button.texture = MedalManager.Instance.questionMark;
				}

		}
		
		public Rect getPixens() {
			return button.pixelInset;
		}

		public void disableTexture() {
			button.enabled = false;	
		}
		
		public void setPixens(Rect inset) {
			button.pixelInset = new Rect(inset.x, button.pixelInset.y, inset.width, inset.height);
		}
		
		public void setTexture(Texture texture) { 
			button.texture = texture;
		}

}

