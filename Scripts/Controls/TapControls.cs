using UnityEngine;
using System.Collections;
/**
 * 
 * New Controls script -- will hopefully make the game easier!
 */
public class TapControls : MonoBehaviour
{
	
	public float panchoStartRowOnRiver;
	private float gameScale;
	
	private PanchoCanoeController5 canoe;
	
	private bool alive;
	private bool firstMoveMade = false; // don't introduce the river current until the player has made 1 move...

	private float lowerTouchBoundary;
	private float pauseButtonX;
	
	private bool normal = false;
	
	private Transform _transform;
	
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
		_transform.position = new Vector2 (_transform.position.x , _transform.position.y + (PanchoRiver.Instance.TileSize * panchoStartRowOnRiver));
		
		canoe = GetComponent<PanchoCanoeController5> ();
		
		setPositionBasedOnObstaclePlacement ();
	
		float diff = Screen.height - 40F;
		lowerTouchBoundary = Screen.height - diff; // (Screen.height - 100F);	
		
		pauseButtonX = PauseButton.Instance.getXPosition ();
		
		if (!PlayerPrefs.HasKey ("Controls")) {
			normal = true;
		} else {
			if(PlayerPrefs.GetInt("Controls") == 0) {
				normal = true;
			} 
			else { 
				normal = false;
			}
		}
		
	}
	
	void Update ()
	{
		
		if (alive && Time.timeScale != 0 || (Time.timeScale == 0 && !firstMoveMade)) { //  && PanchoRiver.Instance.shouldStartGame())) {
			
			for (int i = 0; i < Input.touchCount; i++) {
				
				Touch touch = Input.GetTouch (i);	
				Vector2 touchPosition = touch.position;
				touchPosition = Camera.main.ScreenToWorldPoint (touchPosition);
				
				if (touch.position.y < lowerTouchBoundary && System.Math.Abs(touch.position.x - pauseButtonX) < 20F) {
					continue;
				} else {
					
					
					
					 if (touch.phase == TouchPhase.Ended) { // && firstMoveMade) {
						
							if (!firstMoveMade) {	
								
								if(!PanchoRiver.Instance.getHowToButtonTexture().HitTest(touch.position, Camera.main)
							   			&& !PanchoRiver.Instance.touchingSwitchFishButton(touch.position)) {

									firstMoveMade = true;
									canoe.unlockCurrent ();
									PanchoRiver.Instance.HidePrompt();
								}
								
							}
							
							if(firstMoveMade) {
								canoe.haltRiverForceMovement ();
								
								//First check which axis was touched
								if(normal) {
									if (touchPosition.x < 0) {
										canoe.swimLeft ();	
										SoundEffectsHelper.Instance.MakeSwimSound();
									}
									else if (touchPosition.x > 0) {
										canoe.swimRight ();
										SoundEffectsHelper.Instance.MakeSwimSound();
									}
									else {
										Debug.Log ("User touched 0.0---> invalid paddle! should this be changed?");
									}
								}
								else {
									if (touchPosition.x < 0) {
										canoe.swimRight ();	
										SoundEffectsHelper.Instance.MakeSwimSound();
									} else if (touchPosition.x > 0) {
										canoe.swimLeft ();
										SoundEffectsHelper.Instance.MakeSwimSound();
									} else {
										Debug.Log ("User touched 0.0---> invalid paddle! should this be changed?");
									}
								}
						}
						
					}
				}
			}
			
			/*
			if (firstMoveMade) { // Input.touchCount == 0 &&
				canoe.unlockCurrent ();
				PanchoRiver.Instance.HidePrompt(); // sets the time back to 1!!!
			} */
		} else if (PanchoRiver.Instance.shouldStartGame () && canoe.isAlive ()) {
			alive = true;
		} else if (Time.timeScale == 0) {
			canoe.lockCurrent ();
		}
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

