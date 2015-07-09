using UnityEngine;
using System.Collections;

public class RetryMouseButton : BaseMouseButton
{
	
	public override void onClick ()
	{
		Application.LoadLevel (Application.loadedLevel);	
	}
	
}


