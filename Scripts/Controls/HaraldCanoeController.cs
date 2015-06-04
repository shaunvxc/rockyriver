using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HaraldCanoeController : MonoBehaviour
{

	private bool alive = true;


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
	
	private SpriteRenderer renderer;
	private Sprite[] sprites;
	
	private int speedBonusCt;
	private int   distance;
	
	private float leftBoundary;
	private float rightBoundary;
	private bool locked = false;
	private int score;
	
	private int lastMove = 1;
	
	private bool activeBonus = false;
	
	// need to do something here to prevent to track the number of times a player
	// has fallen back, that way we don't increment distance ( just the number of strokes taken) until the player
	// makes up for the distance he lost. 
	private bool speedBonus = false;
	
	private int numSpeedBonuses = 0; // store the number of consecutive speed bonuses & increase the score faster based on it.
	
	/* Rotational Vectors  */
	private Vector3 FACE_LEFT = new Vector3 (0F, 0F, -20F);
	private Vector3 FACE_RIGHT = new Vector3 (0F, 0F, 20F);
	
	private bool queuedLeftMove = false;
	private bool queuedRightMove = false;
	
	private List<int> queuedMoves;
	
	void Awake ()
	{	
		gridMove = GetComponent<GridMoveWithDiagonals> ();		
		gridMove.haltAllMovement ();
		
		sprites = Resources.LoadAll <Sprite> ("HaraldAtlas");
	}
	
	void Start ()
	{ 					
		movementScale = PanchoRiver.Instance.TileSize; 
		
		leftBoundary = (PanchoRiver.Instance.LeftRiverBoundary * movementScale) + (PanchoRiver.Instance.CameraShiftOffset - .5F);
		rightBoundary = (PanchoRiver.Instance.RightRiverBoundary * movementScale) + (PanchoRiver.Instance.CameraShiftOffset);  //)+ .5F);
		
		
		locked = false;		
		
		SweetSpot = SweetSpot * movementScale;
		SweetSpotBottom = SweetSpotBottom * movementScale;
		
		renderer = GetComponent<SpriteRenderer> ();
		renderer.sprite = sprites [0];
		queuedMoves = new List<int> ();
				if (TimerCount == 0) { 
			TimerCount = 22;
				}
	}
	
	void Update ()
	{		
		
		
		if (moveY) {
			if (transform.position.y <= destinationVector.y) {
				locked = false;
				gridMove.haltYAxisMovement ();	
				
				haltYMovement ();
			}
		} else if (moveX) { 
			
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
		} else if (!currentLocked) {
			incrementTime ();
		}
		
	}
	
	
	
	public int calculateScore ()
	{
		return speedBonusCt + distance;
	}
	
	public string getScoreString() { 
		
		if (activeBonus) { 
			return (speedBonusCt + distance) + "+";
		}
		
		return (speedBonusCt + distance) + ""; 
	}
	
	public int getSpeedBonus() { 
		return speedBonusCt; // playerScore variable stores the number of points accumulated through moving quicky enough to gain speed bonuses. 
	}
	
	public int getDistance() { 
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
		
		gridMove.haltAllMovement ();
		moveX = false;
		timer = 0;
		
		centerCanoe ();
		
		locked = false;
		
		if (transform.position.y > SweetSpot) {
			PanchoRiver.Instance.shiftRowsDown (1, getScoreString(), calculateScore());  // saw an exception thrown here 
			transform.position = new Vector2 (transform.position.x, transform.position.y - (movementScale * .5F));			
			distance++;
		}
		
		if (queuedMoves.Count > 0) {
			int queuedMove = queuedMoves[0];
			queuedMoves.RemoveAt(0);
			if(queuedMove == 0) { 
				paddleLeft();
			}
			else { 
				paddleRight();
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

	public void kill ()
	{
		alive = false;
		
		
	}
	
	public void paddleLeft ()
	{
		
		if (transform.position.x > leftBoundary) {
			
			if(!locked) { 

				if (timer <= 2 && distance > 0) {
					
					if(speedBonus) { 
						speedBonusCt += 1;
						activeBonus = true;
					} else {
						speedBonus = true;
					}
					
				} else {
					speedBonus = false;
					activeBonus = false;
				}
				
				locked = true; // keep this thing locked!!!!!
				timer = 0;
				
				destinationVector = new Vector2 (transform.position.x - (movementScale * .5F), transform.position.y);
				
			//	renderLeftCanoe ();
				rotateCanoeLeft ();
				renderLeftBoat();
				gridMove.paddleLeftAndUp ();	

				movePositive = false;
				moveX = true;
				
			}
			else {
				queuedMoves.Add (0);
			}
		}
	}
	
	public void paddleRight ()
	{	
		
		if (transform.position.x < rightBoundary) { 
			
			if(!locked) { 

				if (timer <= 2 && distance > 0) {
					
					if(speedBonus) { 
						speedBonusCt += 1;
						activeBonus = true;
					} else {
						speedBonus = true;
					}
					
				} else {
					speedBonus = false;
					activeBonus = false;
					
				}
				
				timer = 0;
				locked = true;
				
				/* destination vector is no longer really needed */
				destinationVector = new Vector2 (transform.position.x + (movementScale * .5F), transform.position.y);
				renderRightBoat();
				//renderRightCanoe ();
				rotateCanoeRight ();
				gridMove.paddleRightAndUp ();
			
				movePositive = true;
				moveX = true;
			}
			else {
				queuedMoves.Add (1);
			}	
			
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
			//lastMove = 1;
		} else {
			PanchoRiver.Instance.shiftRowsUp (1);
			distance --; 
		}
		
	}

	public bool isAlive ()
	{
		return alive;
	}

	
	public bool isLocked ()
	{ 
		return locked;
	}
	
	private void resting ()
	{ 
		if (lastMove == 0) { 
						renderer.sprite = sprites [0];
				} else {
					renderer.sprite = sprites[2];
				}
	}

	
	
	public void renderDeathSprite() { 
		renderer.sprite = sprites [lastMove + 3];
	}


	private void renderLeftBoat() { 
		lastMove = 0;
		renderer.sprite = sprites [1];
	}

	private void renderRightBoat() {
		lastMove = 1;
		renderer.sprite = sprites [3];
	}


	private void renderLeftCanoe ()
	{ 
		
		if (lastMove == 0 && speedBonus) {
			renderer.sprite = sprites [1];
			lastMove = 1;
		} else {
			renderer.sprite = sprites [0];
			lastMove = 0;
		}
	}
	
	private void renderRightCanoe ()
	{ 
		if (lastMove == 2 && speedBonus) { 
			renderer.sprite = sprites [1];
			lastMove = 1;
		} else {
			renderer.sprite = sprites [2];
			lastMove = 2;
		}
		
	}
}

