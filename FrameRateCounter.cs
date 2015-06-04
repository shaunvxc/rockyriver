using UnityEngine;
using System.Collections;

public class FrameRateCounter : MonoBehaviour
{
	int frameCount = 0;
	float dt = 0.0F;
	float fps = 0.0F;
	float updateRate = 4.0F;  // 4 updates per sec.
	
	void Update()
	{
		frameCount++;
		dt += Time.deltaTime;
		if (dt > 1.0/updateRate)
		{
			fps = frameCount / dt ;
			frameCount = 0;
			dt -= 1.0F/updateRate;

			Debug.Log ("Fps = " + fps);
		}
	}
}

