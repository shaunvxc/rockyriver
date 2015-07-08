// /**
//   * (C) %SelloutSystems
//   *
//   *
//   * @Author Shaun Viguerie
//   * 
//   */
using UnityEngine;
using System.Collections;

public class KeyboardControls : MonoBehaviour
{

		public bool DebugNoDeath;


		public float panchoStartRowOnRiver;
		private float gameScale;
		private PanchoCanoeController5 canoe;
		private bool alive;
		private bool firstMoveMade = false; // don't introduce the river current until the player has made 1 move...
	
		private float lowerTouchBoundary;
		private float pauseButtonX;
		private bool normal = false;
		private Transform _transform;
		private bool moveLeft = false;
		private bool moveRight = false;
		private bool moved = false;

		void Awake ()
		{
		
				alive = false;
				_transform = transform;
		}
	
		void Start ()
		{ 
				gameScale = PanchoRiver.Instance.TileSize;
		
		
				float baseXPosition = PanchoRiver.Instance.getBaseObstacleXPosition ();
				// was + baseXPosition
				_transform.position = new Vector2 (_transform.position.x, _transform.position.y + (PanchoRiver.Instance.TileSize * panchoStartRowOnRiver));
		
				canoe = GetComponent<PanchoCanoeController5> ();
		
				setPositionBasedOnObstaclePlacement ();
		
				float diff = Screen.height - 40F;
				lowerTouchBoundary = Screen.height - diff; // (Screen.height - 100F);	
		
				pauseButtonX = PauseButton.Instance.getXPosition ();
		
				if (!PlayerPrefs.HasKey ("Controls")) {
						normal = true;
				} else {
						if (PlayerPrefs.GetInt ("Controls") == 0) {
								normal = true;
						} else { 
								normal = false;
						}
				}
		}
	
		void Update ()
		{
				// for the pause button
				if (alive && Time.timeScale != 0 || (Time.timeScale == 0 && !firstMoveMade)) { //  && PanchoRiver.Instance.shouldStartGame())) {

						if (!canoe.isMoving ()) {
								
								if (!firstMoveMade && Input.anyKeyDown) { 
										startRiver ();
								}
									
								if (Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.RightArrow)) { 

										canoe.haltRiverForceMovement ();

										if (Input.GetKeyDown (KeyCode.LeftArrow) && !moved) { 
												
												if (normal) { 
													canoe.swimLeft ();						
												} else {
													canoe.swimRight ();
												}

												moved = true;				
												moveLeft = true;
										
												//			moveRight = false;
										} else if (Input.GetKeyDown (KeyCode.RightArrow) && !moved) {
										
												if (normal) { 
														canoe.swimRight ();						
												} else {
														canoe.swimLeft ();
												}
												moved = true;
												moveRight = true;
												//			moveLeft = false;
										}
								} else {

										if (moved && moveLeft) { 
												moveLeft = false;
												moved = false;
												//						moveLeft = false;
							
										} else if (moved && moveRight) {
											
												moveRight = false;

												moved = false;

										} 
								}
						}
	
				} else if (PanchoRiver.Instance.shouldStartGame () && canoe.isAlive ()) {
						alive = true;
				} else if (Time.timeScale == 0) {
						canoe.lockCurrent ();
				} else if(!alive && firstMoveMade) { 
						
					if(Input.GetKeyDown(KeyCode.Space)) {	
							Application.LoadLevel(Application.loadedLevel);
					}
				}
		}

		private void startRiver ()
		{
				firstMoveMade = true;
				canoe.unlockCurrent ();
				PanchoRiver.Instance.HidePrompt ();
		}
	
		private void setPositionBasedOnObstaclePlacement ()
		{
		
				if (_transform.position.x == 0F) {
						float baseXPosition = PanchoRiver.Instance.getBaseObstacleXPosition ();
						_transform.position = new Vector2 (_transform.position.x + baseXPosition, _transform.position.y);
				}
		
		}
	
	
		/**
		 * 
		 * kill the player when he hits an obstacle & initiate game restart sequence
		 */
	
		void OnTriggerEnter2D (Collider2D collision)
		{
				 
				alive = false;
				SoundEffectsHelper.Instance.MakeDeathSound ();				
			
		}
}

