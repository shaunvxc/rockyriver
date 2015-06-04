using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/**
 * 
 * TODO This class is getting super messy and NEEDS to be cleaned up
 * 
 * @Author Shaun Viguerie 
 * 
 * (C) %SelloutSystems 2014
 * 
 */
public class PanchoCanoeController5 : MonoBehaviour
{
		public bool autoPilot;
		public float SweetSpot;
		public float SweetSpotBottom;
		
		private  int TimerCount;
		private int timer = 0;

	 	private bool currentLocked = true;
		
		private GridMoveWithDiagonals gridMove;
		private Vector2  destinationVector;
		
		private bool movePositive = false;
		private bool moveX = false;
		private bool moveY = false;
		
		private float movementScale;

		private SpriteRenderer fishRenderer;
		private Sprite[] sprites;
		private int speedBonusCt;
		private int   distance;
		private float leftBoundary;
		private float rightBoundary;
		private bool locked = false;

		private int lastMove = 1;
		private bool activeBonus = false;

		// need to do something here to prevent to track the number of times a player
		// has fallen back, that way we don't increment distance ( just the number of strokes taken) until the player
		// makes up for the distance he lost. 
		private bool speedBonus = false;

		/* Rotational Vectors  */
		private Vector3 FACE_LEFT = new Vector3 (0F, 0F, -20F);
		private Vector3 FACE_RIGHT = new Vector3 (0F, 0F, 20F);

		private List<int> queuedMoves;
		private bool alive = true;
		private bool noSpeedBonus = false;
		private int consecutiveFallbacks = 0;
		private Transform _transform;

		private static int fishCode = 1;	

		void Awake ()
		{	
				sprites = Resources.LoadAll <Sprite> ("SalmonAtlasFull");
	    
   		 //		CycleFishManager.Instance.CycleFishEvent += updateFish;	
				/*
				if (fishCode == 1) {
						sprites = Resources.LoadAll <Sprite> ("SalmonAtlasFull");
				} else if (fishCode == 2) {
						sprites = Resources.LoadAll <Sprite> ("CuseAtlas");			
				} else if (fishCode == 3) {
						sprites = Resources.LoadAll <Sprite> ("AsuAtlas");
				} */
	
				_transform = transform;
				
				gridMove = GetComponent<GridMoveWithDiagonals> ();		
				gridMove.haltAllMovement ();
  		}
	
		void Start ()
		{ 					
				movementScale = PanchoRiver.Instance.TileSize; 
		
				leftBoundary = (PanchoRiver.Instance.LeftRiverBoundary * movementScale) + (PanchoRiver.Instance.CameraShiftOffset - .5F);
				rightBoundary = (PanchoRiver.Instance.RightRiverBoundary * movementScale) + (PanchoRiver.Instance.CameraShiftOffset + .5F);

				
				locked = false;		
		
				SweetSpot = SweetSpot * movementScale;
				SweetSpotBottom = SweetSpotBottom * movementScale;
				
				fishRenderer = GetComponent<SpriteRenderer> ();
				fishRenderer.sprite = sprites [1];
				queuedMoves = new List<int> ();
				TimerCount = PanchoRiver.Instance.getFallbackTime ();
				

				if (TimerCount == 0) { 
						Debug.Log ("Timer count == 20");
						TimerCount = 20;
				}
		}
	
		void Update ()
		{		
				if (Time.timeScale != 0) {
		
						if (moveY) {
								if (_transform.position.y <= destinationVector.y) {
										locked = false;
										gridMove.haltYAxisMovement ();	
				
										haltYMovement ();
								}
						} else if (moveX) { 
			
								if (movePositive) { 
				
										if (_transform.position.x >= destinationVector.x) {
												gridMove.haltAllMovement ();
												haltXMovement ();
										}
				
								} else {
										if (_transform.position.x <= destinationVector.x) { 
												gridMove.haltAllMovement ();
												haltXMovement ();
										}
				
								}
						} else if (!currentLocked) {
								incrementTime ();
						}
				}
		}
		
		
		public int calculateScore ()
		{
				return speedBonusCt + distance;
		}

		public string getScoreString ()
		{ 

				if (activeBonus) { 
						return (speedBonusCt + distance) + "+";
				}

				return (speedBonusCt + distance) + ""; 
		}

		public void kill ()
		{
				alive = false;
		}

		public bool isAlive ()
		{
				return alive;
		}
		
		public int getSpeedBonus ()
		{ 
				return speedBonusCt; // playerScore variable stores the number of points accumulated through moving quicky enough to gain speed bonuses. 
		}

		public int getDistance ()
		{ 
				return distance;
		}
	
		public void unlockCurrent ()
		{ 
				currentLocked = false;		
		}
	
		public void lockCurrent ()
		{
				currentLocked = true;	
		}
	
		private void incrementTime ()
		{ 
				timer++;
				if (timer == 2) { 
						resting ();
				} else {
			
						if (timer >= TimerCount) {
								timer = 0;
								applyRiverCurrent ();
						}
				}
		}
	
		public void haltRiverForceMovement ()
		{
				if (moveY) {
						gridMove.haltAllMovement ();
						moveY = false;
				}
		}
	
		private void haltXMovement ()
		{ 
			//	resting ();
				gridMove.haltAllMovement ();
				moveX = false;
				timer = 0;
		
				center ();
		
				locked = false;
		
				if (_transform.position.y > SweetSpot) {
						
						PanchoRiver.Instance.shiftRowsDown (1, getScoreString (), calculateScore ());  // saw an exception thrown here 
						
						_transform.position = new Vector2 (_transform.position.x, _transform.position.y - (movementScale * .5F));			
						
					
						
						distance++;

						if (activeBonus) {  // keep eye on this 
								speedBonusCt ++;
						}
				}

				if(autoPilot) { 	
					resting();// salmon auto pilot needs to rest
				}
				
				// consider a coroutine here
				if (queuedMoves.Count > 0) {
						//Debug.Log ("# of moves in queue: " + queuedMoves.Count);
						int queuedMove = queuedMoves [0];
						queuedMoves.RemoveAt (0);
						if (queuedMove == 0) { 
								swimLeft ();
						} else { 
								swimRight ();
						}
				}
		}
	
		private void haltYMovement ()
		{ 									
				moveY = false;
				moveX = false;
				timer = 0;
		}
	
		public void HaltAllMovement ()
		{ 
				gridMove.haltAllMovement ();
		}
	
		public bool isMoving ()
		{ 
				return moveX;
		}

		public bool isMovingInEitherDirection() { 
			return moveX || moveY;
		}
	
		private void center ()
		{
				float currentRotation = _transform.localEulerAngles.z * -1;
				_transform.Rotate (new Vector3 (0F, 0F, currentRotation));
		}
	
		private void rotateLeft ()
		{ 
				_transform.Rotate (FACE_RIGHT);
		}
	
		private void rotateRight ()
		{ 
				_transform.Rotate (FACE_LEFT);
		}
	
		public void swimLeft ()
		{

				if (_transform.position.x > leftBoundary && alive) {

						if (!locked) { 

								if (timer <= 4 && !noSpeedBonus && distance > 0 && consecutiveFallbacks <= 0) {
									
										if (speedBonus) { 
												//	speedBonusCt += 1;
												activeBonus = true;
										} else {
												speedBonus = true;
										}
								
								} else {
										speedBonus = false;
										activeBonus = false;
										

										if (consecutiveFallbacks > 0) {
												consecutiveFallbacks--;
										}
								
										if (noSpeedBonus) {
												noSpeedBonus = false;
										}
								}
								
								locked = true; // keep this thing locked!!!!!
								timer = 0;
								
								destinationVector = new Vector2 (_transform.position.x - (movementScale * .5F), _transform.position.y);
								
								renderLeftSprite ();
								rotateLeft ();
								gridMove.paddleLeftAndUp ();	
								
								movePositive = false;
								moveX = true;

						} else {
								queuedMoves.Add (0);
						}
				}
		}
	
		public void swimRight ()
		{	
				
				if (_transform.position.x < rightBoundary && alive) { 
						
						if (!locked) { 

								if (timer <= 4 && !noSpeedBonus && distance > 0 && consecutiveFallbacks <= 0) {
											
										if (speedBonus) { 
												//		speedBonusCt += 1;
												activeBonus = true;
										} else {
												speedBonus = true;
										}

								} else {
										speedBonus = false;
										activeBonus = false;
										
										if (consecutiveFallbacks > 0) {
												consecutiveFallbacks--;
										}
										
										if (noSpeedBonus) { 
												noSpeedBonus = false;
										}
								}
								
								timer = 0;
								locked = true;
								
								/* destination vector is no longer really needed */
								destinationVector = new Vector2 (_transform.position.x + (movementScale * .5F), _transform.position.y);
								
								renderRightSprite ();
								rotateRight ();
								gridMove.paddleRightAndUp ();
								
								movePositive = true;
								moveX = true;
						} else {
								queuedMoves.Add (1);
						}	
						
				}
		}
	
		public void applyRiverCurrent ()
		{		
		
				if (_transform.position.y > SweetSpotBottom) { 
						destinationVector = new Vector2 (_transform.position.x, _transform.position.y - (movementScale * .5F));
						movePositive = false;
						moveX = false;
						moveY = true;
						gridMove.moveDown ();
						lastMove = 1;
						speedBonus = false;
					
						if(destinationVector.y <= SweetSpotBottom) {
							consecutiveFallbacks++;
						}
						else if(consecutiveFallbacks > 0 ) { 
							consecutiveFallbacks++;
						}

				} else {
						
						PanchoRiver.Instance.shiftRowsUp (1);
						distance--; 
						noSpeedBonus = true;
						consecutiveFallbacks++;
				}
		
		}

		
		public void moveFishDown() { 

				destinationVector = new Vector2 (_transform.position.x, _transform.position.y - (movementScale * .5F));
				movePositive = false;
				moveX = false;
				moveY = true;
				gridMove.moveDown ();
				lastMove = 1;
		}
		
		
		void updateFish() {

				if (sprites != null) {
						if (fishCode == 1) {
								sprites = Resources.LoadAll <Sprite> ("CuseAtlas");
								fishCode = 2;
						} else if (fishCode == 2) {
								sprites = Resources.LoadAll <Sprite> ("AsuAtlas");			
								fishCode = 3;
						} else if (fishCode == 3) {
								sprites = Resources.LoadAll <Sprite> ("SalmonAtlasFull");
								fishCode = 1;
						}

						fishRenderer.sprite = sprites [1];
				}
		}

		public bool isLocked ()
		{ 
				return locked;
		}
	
		private void resting ()
		{ 
				fishRenderer.sprite = sprites [1];
		}

		public void renderDeathSprite ()
		{ 
				fishRenderer.sprite = sprites [lastMove + 3];
		}
	
		private void renderLeftSprite ()
		{ 
			
				if (lastMove == 0 && speedBonus) {
						fishRenderer.sprite = sprites [1];
						lastMove = 1;
				} else {
						fishRenderer.sprite = sprites [0];
						lastMove = 0;
				}
		}
	
		private void renderRightSprite ()
		{ 
				if (lastMove == 2 && speedBonus) { 
						fishRenderer.sprite = sprites [1];
						lastMove = 1;
				} else {
						fishRenderer.sprite = sprites [2];
						lastMove = 2;
				}
		
		}
}

