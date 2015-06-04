using UnityEngine;
using System.Collections;

public class SwipeToStartPrompt : MonoBehaviour
{

		private MoveAndRecycleTile[] hands;
		private TapHand[] tapHands;

		private GUITextureButton howToButton;
		
		
		
		private int moveCount;
		private bool animate = false;
		
		public bool tap;
		private int tapTime = 25;
		private int timer = 0;
		
		void Awake ()
		{
				hands = GetComponentsInChildren<MoveAndRecycleTile> ();							
				howToButton = GetComponentInChildren<GUITextureButton> ();
				tapHands = GetComponentsInChildren<TapHand> ();
				var buttons = GetComponentsInChildren<GUITextureButton> ();
				/*
				if (Application.systemLanguage == SystemLanguage.Chinese) { 
						howToButton = buttons [1];
						buttons [0].Deactivate ();
				} else {
						howToButton = buttons[0];
						buttons[1].Deactivate();
				} */
		}
		
		void Start () { 	

				startAnimation ();		
				
		}
		
		void Update ()
		{
				if (animate) {
						
						if(tap) { 
							timer++;
							if(timer >= tapTime) { 
								tapHands[moveCount].disableRenderer();
								updateMoveCount();
								tapHands[moveCount].enableRenderer();
								timer = 0;
							}
						}
						else {
							if (!hands [moveCount].getIsMoving ()) {

									updateMoveCount ();
									hands [moveCount].setMoving ();
							} 
						}
				} 
		}

		public GUITexture getHowToTexture() { 
			return howToButton.getTexture ();
		}

		public void startAnimation ()
		{		

				if (tap) { 
						tapHands[0].enableRenderer();
						moveCount = 0;
						animate = true;
				} else {
						hands [0].setMoving ();
						moveCount = 0;
						animate = true;
				}
		}

		public void Activate ()
		{ 
				gameObject.SetActive (true);
		}

		public void Deactivate ()
		{ 
				animate = false;
				gameObject.SetActive (false);
		}

		private void updateMoveCount ()
		{ 


				if (moveCount == 0) { 
						moveCount = 1;
				} else {
						moveCount = 0;
				}

		}



}
