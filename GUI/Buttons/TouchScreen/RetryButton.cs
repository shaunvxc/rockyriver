using UnityEngine;
using System.Collections;

public class RetryButton : BaseButton
{


	// would be nice to require this method to be overridden!
	public override void onClick ()
	{

		Debug.Log ("Reloading level");
		Application.LoadLevel (Application.loadedLevel);

	}
	
}

