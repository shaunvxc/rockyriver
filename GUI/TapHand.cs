using UnityEngine;
using System.Collections;

public class TapHand : MonoBehaviour
{
		
		private SpriteRenderer _renderer;
		public float x;
		public float  y_start;
		
		private Transform _transform;

		void Awake() { 
			_renderer = GetComponent<SpriteRenderer> ();	
			_transform = transform;
		}
	
		void Start ()
		{	
			_transform.position = new Vector2 (x, y_start);
			disableRenderer ();		
		}
		
		
		public void disableRenderer ()
		{
			if (_renderer != null) { 
				_renderer.enabled = false;
			}
			
		}

		public void enableRenderer ( )
		{	
			
			if (_renderer != null) { 
				_renderer.enabled = true;
			} else {
				Debug.Log ("renderer is null!");
			}
			
		}


}

