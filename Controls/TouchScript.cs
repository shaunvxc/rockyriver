using UnityEngine;
using System.Collections;

/**
 * 
 * Script for touch controls, initially just a drag movement
 * 
 * @Author Shaun Viguerie
 * 
 * 
 */
public class TouchScript : MonoBehaviour
{

		private bool isBeingMoved = false;
		private int  fingerId = -1;

		void Update ()
		{		
		
				if (Input.touchCount == 0) {
						return;
				} else if (Input.touchCount == 1) {
						
						
						Vector2 touchPosition = Input.GetTouch (0).position;
						touchPosition = Camera.main.ScreenToWorldPoint (touchPosition);
						

						if (Input.GetTouch (0).phase == TouchPhase.Began) {

								Debug.Log ("Touch phase Began, distance from player position = " + Vector2.Distance (transform.position, touchPosition));
						
								if (!isBeingMoved && Vector2.Distance (transform.position, touchPosition) < 4F) { 
										isBeingMoved = true;
										fingerId = Input.GetTouch (0).fingerId;
										transform.position = touchPosition;
								}

						} else if (Input.GetTouch (0).phase == TouchPhase.Ended) { 
								Debug.Log ("Touch phase is ended at point " + touchPosition + ", tapCount = " + Input.GetTouch (0).tapCount);
								isBeingMoved = false;
								fingerId = -1;
						} else if (Input.GetTouch (0).phase == TouchPhase.Canceled) { 
								Debug.Log ("Touch phase is cancelled at point " + touchPosition);
								isBeingMoved = false;
								fingerId = -1;	
						} else if (Input.GetTouch (0).phase == TouchPhase.Stationary) { 
								Debug.Log ("Touch phase stationary at point " + touchPosition);
						} else if (isBeingMoved && Input.GetTouch (0).fingerId == fingerId) { 
								Debug.Log ("moving player along the touch, tapCount = " + Input.GetTouch (0).tapCount);
								transform.position = touchPosition;
						}
				} else if (Input.touchCount > 1) {
					
						Debug.Log ("More than 1 touch at once");
							
						if (fingerId != -1) {


						}	
				
				}
		}
}
