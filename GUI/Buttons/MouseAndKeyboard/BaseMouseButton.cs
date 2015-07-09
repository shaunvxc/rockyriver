using UnityEngine;
using System.Collections;
/**
 * 
 * Button implementation to handle mouse input for browser version of the game. 
 */
public class BaseMouseButton : MonoBehaviour, IButton
{
	private GUITexture texture;
	private Transform _transform;
	private bool pushed;

	void Awake ()
	{
		texture = GetComponent<GUITexture> ();
		_transform = transform;
	}

	void OnMouseDown() { 

		if (texture.HitTest (Input.mousePosition, Camera.main)) {
			if (!pushed) { 
				_transform.position = new Vector3 (_transform.position.x, _transform.position.y - .01F, _transform.position.z);
				pushed = true;	
			}
		} 	
	}

	void OnMouseUpAsButton() {
		if(pushed) { 
			Debug.Log ("OnMouseUpAsButton");
			onClick();
			pushed = false;
		}

	}

	// would be nice to require this method to be overridden!
	public virtual void onClick ()
	{
		Debug.Log ("Error! Using a button with no implementation of onClick()-- this means some button will not be responsive!");
	}


	public void Activate ()
	{ 
		transform.gameObject.SetActive (true);
	}
	
	public void Deactivate ()
	{
		transform.gameObject.SetActive (false);
	}
}

