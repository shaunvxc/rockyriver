using UnityEngine;
using System.Collections;

public class ShowTipsMButton : BaseMouseButton
{
	
	public override void onClick ()
	{
		TipsManager.Instance.ShowTips();
	}
	
}

