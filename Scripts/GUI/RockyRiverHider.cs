using UnityEngine;
using System.Collections;

public class RockyRiverHider : MonoBehaviour
{
			
		public static RockyRiverHider Instance;
		private GUITextureButton[] buttons;

		void Awake ()
		{
				if (Instance == null) {
						Instance = this;	
				}

	
		}

		void Start ()
		{ 
			

				buttons = GetComponentsInChildren<GUITextureButton> ();

				if (Application.systemLanguage == SystemLanguage.Chinese) { 
						buttons [0].Deactivate ();
						buttons [1].Deactivate ();
				} else {

						//buttons [0].Deactivate ();
						//buttons [1].Deactivate ();
						buttons [2].Deactivate ();
				}
		}

		public void Deactivate ()
		{
				gameObject.SetActive (false);
		}

		public void Activate ()
		{
				gameObject.SetActive (true);
		}
		
}

