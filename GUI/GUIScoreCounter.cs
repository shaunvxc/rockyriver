// /**
//   * (C) %SelloutSystems
//   *
//   *
//   * @Author Shaun Viguerie
//   * 
//   */
using UnityEngine;
using System.Collections;

public class GUIScoreCounter : MonoBehaviour
{
	private GUIText[] scoreCounters;

	private int currentBestScore;
	private bool newBest = false;

	public float xPos;
	public float yPos;

	void Awake()  {
		scoreCounters =  GetComponentsInChildren<GUIText>();
	}

	void Start() { 

		if(scoreCounters == null) { 
			scoreCounters = GetComponentsInChildren<GUIText>();
		}

		string bestScoreKey;
		if (PanchoRiver.Instance.getFallbackTime () > 10) { 
						bestScoreKey = "RookieBestScore";								
		} else {
						bestScoreKey = "VeteranBestScore";
		}

		if (PlayerPrefs.HasKey (bestScoreKey)) {
			currentBestScore = PlayerPrefs.GetInt (bestScoreKey);
		} else {
			currentBestScore = 0;
		}
	}

	public float getXCoord() { 
		return transform.position.y;
	}

	public void updateScore(string score, int numericScore) { 
	
		scoreCounters[0].text = score;
		scoreCounters[1].text = score;

		if(!newBest && currentBestScore > 0 && numericScore > currentBestScore) {	
			scoreCounters[0].color = new Color32(145, 14, 240, 255);
			scoreCounters[1].color = new Color32 (251, 242, 215, 255);
			newBest = true;
		}
	
	}

	public void clearScore() { 
		scoreCounters[0].gameObject.SetActive (false);
		scoreCounters [1].gameObject.SetActive (false);
	}
}

