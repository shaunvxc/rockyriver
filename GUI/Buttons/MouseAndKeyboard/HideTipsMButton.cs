using UnityEngine;
using System.Collections;

public class HideTipsMButton : BaseMouseButton
{
	public override void onClick ()
	{
		TipsManager.Instance.HideTips();
	}
	
}

