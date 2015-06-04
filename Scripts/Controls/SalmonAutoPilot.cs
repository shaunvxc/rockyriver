using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SalmonAutoPilot : MonoBehaviour
{
		
		private PanchoCanoeController5 controller;
		public List<int> startScreenMoves;
			
		private int lastMove;
		private int moveIdx = 0;
		private Transform _transform;
		private bool initialized;
		private float gameScale;
		private float LeftRiverBoundary;
		private int timer = 0;
		private Vector2 resetPosition;

		void Awake ()
		{ 
				controller = GetComponent<PanchoCanoeController5> ();			
				_transform = transform;				
		}

		void Start ()
		{ 	
				initialized = false;			
		}

		void Update ()
		{
				if (!initialized) {						

						startScreenMoves = PanchoRiver.Instance.getStartupMoves ();
						if (startScreenMoves != null && startScreenMoves.Count > 0 ) {	
								LeftRiverBoundary = PanchoRiver.Instance.LeftRiverBoundary;
	
								_transform.position = new Vector2 (LeftRiverBoundary + startScreenMoves [0] * .5F, _transform.position.y + (PanchoRiver.Instance.TileSize * PanchoRiver.Instance.getLowestObstacleYPosition ()));
								moveIdx++;
								lastMove = startScreenMoves [0];
								
								resetPosition = _transform.position;

								initialized = true;
						}

				} else if (!controller.isMoving ()) {
						
						if (timer < 14) {  // was 16
								
								timer++;
							
								if(timer == 6) { // was 9

									TapHandController.Instance.hideBothHands();
									if(startScreenMoves[moveIdx] == -1) {
										TapHandController.Instance.renderFallback();		
									}
									
									
								}
								else if(timer == 12 && startScreenMoves[moveIdx] != -1) {
									if (startScreenMoves [moveIdx] > lastMove) {
										TapHandController.Instance.renderRightHand();
									} else if (startScreenMoves [moveIdx] < lastMove) {
										TapHandController.Instance.renderLeftHand();
									}
								}

						} else {
						
								if (moveIdx < startScreenMoves.Count) { 
								
										int move = startScreenMoves[moveIdx];

										if ( move != -1 ) {  // startScreenMoves [moveIdx] != - 1) {
												
												if (startScreenMoves [moveIdx] > lastMove) {
														controller.swimRight ();
												} else if (startScreenMoves [moveIdx] < lastMove) {
														controller.swimLeft ();
												}
												
											//	lastMove = startScreenMoves [moveIdx];
												lastMove = move;
												timer = 0;
												moveIdx++;
										} else {
											
												if(timer < 18) {
													timer++;
												} 
												else {
												//	TapHandController.Instance.hideBothHands();
													controller.moveFishDown ();
													timer = 0 ;
													moveIdx++;
												}
										}
								} 

								if(moveIdx == 20) { 
									startScreenMoves.RemoveRange(0, moveIdx );
									moveIdx = 0;
								}
						}

				}
		}

		private void reset ()
		{ 
			
				_transform.position = resetPosition;
				moveIdx = 1;
				lastMove = startScreenMoves [0];
		}
		

}

