using UnityEngine;
using System.Collections;

public class TabButton : MonoBehaviour
{
		private GUITexture tab;
		public string tabLabel;
		private GUIText text;
		
		public bool isActive;
		
		private float activeZ;
		private float inactiveZ;
		
		private bool pushed;

		public bool isNovice;
		
		void Awake() {
			tab = GetComponent<GUITexture> ();	
		}	

		void Start() {
			
			//	activeZ = transform.position.z;	
			//	inactiveZ = activeZ - 20F;
		}
		
		void Update() {

				for (int i = 0; i < Input.touchCount; i++) {
					
					Touch touch = Input.GetTouch (i);	
					
					if (tab.HitTest (touch.position, Camera.main)) {
						if (!pushed) { 
							pushed = true;	
						}
					} 
				}
				
				if (Input.touchCount == 0) { 
					
					if (pushed) {
						SoundEffectsHelper.Instance.MakeButtonClick ();
						pushed = false;
						if(!isActive) {
							if(isNovice ) {
								MedalManager.Instance.showRookie();
							}
							else {
								MedalManager.Instance.showVeteran();
							}
						}
					}
				}
		}

			
		public void SetSelected(bool selected) {
			if (selected) {
						isActive = true;
						transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z + 5F);
			} else {
						isActive = false;
						transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z - 5F);
			}
		}
				
		public void toggle() {
			
			if (isActive) {
						isActive = false;	
						transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z - 5F);
			} 
			else {
						isActive = true;
						transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z + 5F);
			}
		}

		public void Activate() {
			gameObject.SetActive (true);	
		}

		public void Deactivate() {
			
			if (isActive) {

					transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z - 5F);
					isActive = false;
			} 

			gameObject.SetActive (false);	
		}
		
}

