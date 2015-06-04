
using UnityEngine;
using System.Collections;

public class ScreenshotCamera : MonoBehaviour
{

	public static ScreenshotCamera Instance;

	public delegate void ScreenReadyEventDelegate ();
	
	public  event ScreenReadyEventDelegate ScreenReadyEvent;
	
	public Texture2D screenshot { get; private set; }
	
	private bool capturing = false;
	
	private Rect captureRect;
	
	private int oldAntiAliasingSettings;
	
	private int noAACountdown;

	void Awake() { 

			if (Instance == null) {
				Instance = this;
			}
	}


	public void TakeScreenshot (float startX, float startY, float endX, float endY)
	{
		oldAntiAliasingSettings = QualitySettings.antiAliasing;
		QualitySettings.antiAliasing = 0;
		captureRect = new Rect (startX, startY, endX - startX, endY - startY);
		noAACountdown = 2;
		capturing = true;
	}
	
	void OnPostRender ()
	{
		if (capturing)
		{
			noAACountdown--;
			if (noAACountdown > 0)
				return;
			
			screenshot = new Texture2D (Mathf.RoundToInt (captureRect.width), Mathf.RoundToInt (captureRect.height), TextureFormat.ARGB32, true);
			screenshot.ReadPixels (captureRect, 0, 0, true);
			screenshot.Apply ();
			
			capturing = false;
			
			QualitySettings.antiAliasing = oldAntiAliasingSettings;
			
			if (ScreenReadyEvent != null)
				ScreenReadyEvent ();
		}
	}
	
}


