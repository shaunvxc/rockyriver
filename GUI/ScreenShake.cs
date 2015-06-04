using UnityEngine;
using System.Collections;
/**
 * @Author Shaun Viguerie
 */
public class ScreenShake : MonoBehaviour
{

		public Camera mainCamera; // set this via inspector

		private float shakeAmount = 0.045F;

		private Vector3 originalCameraPosition;
		private bool up = true;
		private PanchoCanoeController5 fish;
	//	private HaraldCanoeController harald;
		
		//public bool isHaraldMove;
		void Awake ()
		{ 
			originalCameraPosition = mainCamera.transform.position;
			fish = GetComponent<PanchoCanoeController5> ();
	//		harald = GetComponent<HaraldCanoeController> ();

			
		}

		void OnTriggerEnter2D (Collider2D coll)
		{
				if (fish.isAlive ()) {
						
						FadeSceneScript.Instance.FlashScreenWhite ();		
						PanchoRiver.Instance.ClearScore();
							
						fish.HaltAllMovement ();
						fish.kill ();
						fish.lockCurrent ();
						fish.renderDeathSprite ();

						InvokeRepeating ("CameraShake", 0, .01f);
						Invoke ("showGameOver", .35F);
						Invoke ("StopShaking", 0.55f);
				}

		}
		
		void CameraShake ()
		{
				if (shakeAmount > 0) {
						
						float quakeAmt;
						
						if(up) {
							quakeAmt = shakeAmount;
							up = false;
						} else {
							quakeAmt = shakeAmount * -1F;
							up = true;
						}
					
						Vector3 pp = mainCamera.transform.position;
						pp.y += quakeAmt;
						mainCamera.transform.position = pp;
				}


		}

		private void showGameOver() { 

			PanchoRiver.Instance.InitiateGameOverSequence(fish.getDistance(), fish.getSpeedBonus());	
		}
	
		void StopShaking ()
		{
				CancelInvoke ("CameraShake");
				mainCamera.transform.position = originalCameraPosition;

		}
}

