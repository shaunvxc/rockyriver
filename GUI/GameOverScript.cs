using UnityEngine;
using System.Collections;

public class GameOverScript : MonoBehaviour
{


		private GUITexture gameOverTexture;
		private bool isActive = false;
		public float stopPosition;
		public float moveDistance;
		private int distance;
		private int speedBonus;
		private	float stopPoint;

		void Awake ()
		{
				gameOverTexture = GetComponent<GUITexture> ();
		}

		void Start ()
		{ 
				gameOverTexture.enabled = false;
		//		stopPoint = Screen.height - moveDistance;
				stopPoint = GUIManager.Instance.getScoreWindowYPosition () + 25F;
				/*
				if (Camera.main.orthographicSize > 3.4) { 
					stopPoint -= 150F;
				}

				*/
		}

		void Update ()
		{
				if (isActive) {
						float y = gameOverTexture.pixelInset.y;
		
						if (gameOverTexture.pixelInset.y > stopPoint) { //transform.position.y > stopPosition) { 

								if (y - 10F < stopPoint) {
										float diff = y - stopPoint;
										gameOverTexture.pixelInset = new Rect (gameOverTexture.pixelInset.x, gameOverTexture.pixelInset.y - diff, gameOverTexture.pixelInset.width, gameOverTexture.pixelInset.height);
								} else {
										gameOverTexture.pixelInset = new Rect (gameOverTexture.pixelInset.x, gameOverTexture.pixelInset.y - 10F, gameOverTexture.pixelInset.width, gameOverTexture.pixelInset.height);
								}
			
						} else {
								isActive = false;
								Invoke ("NotifyPanchoRiver", .3F);
						}
				}
		}

		private void NotifyPanchoRiver ()
		{ 
				PanchoRiver.Instance.PromptRestartGUI (distance, speedBonus);
		}
	
		public void initiateGameOverSequeunce (int distance, int speedBonus)
		{ 
				gameOverTexture.enabled = true;
				isActive = true;
				this.distance = distance;
				this.speedBonus = speedBonus;				
		}

		public void disableTexture ()
		{
				gameOverTexture.enabled = false;	
		}

		public void enableTexture ()
		{
				gameOverTexture.enabled = true;	
		}
		
}
