using UnityEngine;
using System.Collections;

public class PanchoCanoeController4 : MonoBehaviour
{
		public float SweetSpot;
		public float SweetSpotBottom;
		public int TimerCount;
		private bool currentLocked = true;
		private int timer = 0;
		private GridMoveWithDiagonals gridMove;
		private Vector2  destinationVector;
		private bool movePositive = false;
		private bool moveX = false;
		private float movementScale;
		private bool moveY = false;

		private Transform leftCanoe;
		private Transform rightCanoe;
		private Transform restingState;	
			

		private float playerScore;
		private int   numStrokes;
		private int   distance;

		private float leftBoundary;
		private float rightBoundary;
		private bool locked = false;

		private int score;

		/* Rotational Vectors  */
		private Vector3 FACE_LEFT = new Vector3 (0F, 0F, -20F);
		private Vector3 FACE_RIGHT = new Vector3 (0F, 0F, 20F);
	
		void Awake ()
		{	
				gridMove = GetComponent<GridMoveWithDiagonals> ();		
				gridMove.haltAllMovement ();
		}
	
		void Start ()
		{ 					
				movementScale = PanchoRiver.Instance.TileSize; 
		
				leftBoundary = (PanchoRiver.Instance.LeftRiverBoundary * movementScale)   + (PanchoRiver.Instance.CameraShiftOffset - .5F);
				rightBoundary = (PanchoRiver.Instance.RightRiverBoundary * movementScale) + (PanchoRiver.Instance.CameraShiftOffset);  //)+ .5F);
				Debug.Log ("Right river boundary= " + rightBoundary);

				rightCanoe = transform.Find ("RightCanoe");
				leftCanoe = transform.Find ("LeftCanoe");
			
				restingState = transform.Find ("RestingSprite");
			
		
				if (leftCanoe != null) {
			
					//	leftCanoe.gameObject.active = false;
					leftCanoe.gameObject.SetActive(false);
					rightCanoe.gameObject.SetActive(false);
				}
		
		
				locked = false;		
		
				SweetSpot = SweetSpot * movementScale;
				SweetSpotBottom = SweetSpotBottom * movementScale;
		}
	
		void Update ()
		{		


				if (moveY) {
					if (transform.position.y <= destinationVector.y) {
						locked = false;
						gridMove.haltYAxisMovement();	
						
						haltYMovement ();
					}
				}
				else if (moveX) { 
						
						if (movePositive) { 
				
								if (transform.position.x >= destinationVector.x) {
										gridMove.haltAllMovement ();
										haltXMovement ();
								}
				
						} else {
								if (transform.position.x <= destinationVector.x) { 
										gridMove.haltAllMovement ();
										haltXMovement ();
								}
				
						}
				} 
				else if (!currentLocked) {
					incrementTime ();
				}
		
		}
		
		public float calculateScore() {
			
			int strokeDistanceDifferential = numStrokes - distance;
			playerScore += distance;
			playerScore -= strokeDistanceDifferential;
			return playerScore;
		}
	
		public void unlockCurrent ()
		{ 
				currentLocked = false;		
		}

		public void lockCurrent() {
			currentLocked = true;	
		}
	
		private void incrementTime ()
		{ 
				timer++;
				if (timer == 1) { 
						resting ();
				} else {

						if (timer >= TimerCount) {
								timer = 0;
								applyRiverCurrentToCanoe ();
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

				centerCanoe ();
		
				locked = false;
					
				if (transform.position.y > SweetSpot) {
						PanchoRiver.Instance.shiftRowsDown (1);  // saw an exception thrown here 
				//		RiverCoast.Instance.updateCoast ();
						transform.position = new Vector2 (transform.position.x, transform.position.y - (movementScale * .5F));			
				}

				numStrokes++;
				distance++;
		}
	
		private void haltYMovement ()
		{ 									
				moveY = false;
				moveX = false;
				distance--;
		}
	
		public void HaltAllMovement ()
		{ 
				gridMove.haltAllMovement ();
		}
	
		public bool isMoving ()
		{ 
				return moveX;
		}
	
		private void centerCanoe ()
		{
				float currentRotation = transform.localEulerAngles.z * -1;
				transform.Rotate (new Vector3 (0F, 0F, currentRotation));
		}
	
		private void rotateCanoeLeft ()
		{ 
				centerCanoe ();
				transform.Rotate (FACE_RIGHT);
		}
	
		private void rotateCanoeRight ()
		{ 
				centerCanoe ();
				transform.Rotate (FACE_LEFT);
		}
	
		public void paddleLeft ()
		{
		
				if (!locked && transform.position.x > leftBoundary) {
						if(timer <= 3) {
							Debug.Log ("SPEED BONUS!!!!");
							playerScore += 5F;
						}
					
						locked = true; // keep this thing locked!!!!!
						timer = 0;
			
						destinationVector = new Vector2 (transform.position.x - (movementScale * .5F), transform.position.y);

						renderLeftCanoe ();
						rotateCanoeLeft ();
						gridMove.paddleLeftAndUp ();	
				
						movePositive = false;
						moveX = true;
				} else if (transform.position.x <= leftBoundary) {
				//		Debug.Log ("Not paddling left, transform.position= " + transform.position.x + ", right boundary is= " + leftBoundary);
				}
		
		}
	
		public void paddleRight ()
		{	
		
				if (!locked && transform.position.x < rightBoundary) {

						if(timer <= 3) {
							Debug.Log ("SPEED BONUS!!!!");
							playerScore += .5F;
						}
							
						timer = 0;
						locked = true;
			
						/* destination vector is no longer really needed */
						destinationVector = new Vector2 (transform.position.x + (movementScale * .5F), transform.position.y);

						renderRightCanoe ();
						rotateCanoeRight ();
						gridMove.paddleRightAndUp ();
						
						movePositive = true;
						moveX = true;
			
				} else if (transform.position.x >= rightBoundary) {
				//		Debug.Log ("Not paddling right, transform.position= " + transform.position.x + ", right boundary is= " + rightBoundary);
				}
		
		}
	
		public void applyRiverCurrentToCanoe ()
		{		
			
				if (transform.position.y > SweetSpotBottom) { 
						destinationVector = new Vector2 (transform.position.x, transform.position.y - (movementScale * .5F));
						movePositive = false;
						moveX = false;
						moveY = true;
						gridMove.moveDown ();
				} else {
						PanchoRiver.Instance.shiftRowsUp (1);
				}
			
		}
	
		public bool isLocked ()
		{ 
				return locked;
		}

		private void resting() { 

			if (leftCanoe != null && rightCanoe != null && restingState != null) { 
				rightCanoe.gameObject.SetActive(false);
				leftCanoe.gameObject.SetActive (false);
				restingState.gameObject.SetActive(true);
			}
			
		}	

		private void renderLeftCanoe ()
		{ 
		
				if (leftCanoe != null && rightCanoe != null  && restingState != null) {
						restingState.gameObject.SetActive(false);	
						rightCanoe.gameObject.SetActive(false);
						leftCanoe.gameObject.SetActive (true);
		
						//rightCanoe.gameObject.active = false;
						//leftCanoe.gameObject.active = true;
				}
		
		
		}
	
		private void renderRightCanoe ()
		{ 
				if (leftCanoe != null && rightCanoe != null && restingState != null) {
					restingState.gameObject.SetActive(false);	

					leftCanoe.gameObject.SetActive(false); // false
					rightCanoe.gameObject.SetActive (true);
					
					//	leftCanoe.gameObject.active = false;
					//	rightCanoe.gameObject.active = true;
				}
		
		}
		
}

