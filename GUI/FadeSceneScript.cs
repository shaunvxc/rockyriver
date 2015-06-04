using UnityEngine;
using System.Collections;

public class FadeSceneScript : MonoBehaviour
{
	public float fadeSpeed = 3.5f;          // Speed that the screen fades to and from black.

	public static FadeSceneScript Instance;

	
	private bool sceneStarting = true;      // Whether or not the scene is still fading in.
	private bool flashingWhite = false;

	private bool fadeToBlack = false;

	void Awake ()
	{
	
		if (Instance == null) { 
			Instance = this;
		}

		// Set the texture so that it is the the size of the screen and covers it.
		guiTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
	}
	
	
	void Update ()
	{
		// If the scene is starting...
		if (sceneStarting) {
						// ... call the StartScene function.
						StartScene ();
				} else if (flashingWhite) {
						FlashWhite ();
				} else if (fadeToBlack) { 

						fadeOutAndReload();
				}
	}
	
	
	void FadeToClear ()
	{
		// Lerp the colour of the texture between itself and transparent.
		guiTexture.color = Color.Lerp(guiTexture.color, Color.clear, fadeSpeed  * 1.5F * Time.deltaTime);
	}
	
	
	void FadeToBlack ()
	{	
		// Lerp the colour of the texture between itself and black.
		guiTexture.color = Color.Lerp(guiTexture.color, Color.black, fadeSpeed * Time.deltaTime);
	}





	void FadeToWhite ()
	{
		// Lerp the colour of the texture between itself and black.
		guiTexture.color = Color.Lerp(guiTexture.color, Color.white, fadeSpeed * 2F  * Time.deltaTime);
	}


	void StartScene ()
	{
		// Fade the texture to clear.
		FadeToClear();
		
		// If the texture is almost clear...
		if(guiTexture.color.a <= 0.05f)
		{
			// ... set the colour to clear and disable the GUITexture.
			guiTexture.color = Color.clear;
			guiTexture.enabled = false;
			
			// The scene is no longer starting.
			sceneStarting = false;
		}
	}


	public void FlashScreenWhite() { 
		flashingWhite = true;
		sceneStarting = false;
		guiTexture.enabled = true;
	}

	private void fadeOutAndReload() { 
		FadeToBlack ();
		if (guiTexture.color.a >= .5F) { 
			Application.LoadLevel(Application.loadedLevel);
		}
	}


	public void FadeToBlackAndReload() { 
		fadeToBlack = true;
		sceneStarting = false;
		guiTexture.enabled = true;
	}

	public void FlashWhite() {

		FadeToWhite ();
		if (guiTexture.color.a >= .4F) { 
			flashingWhite = false	;
			guiTexture.color = Color.clear;
			guiTexture.enabled = false;
		}
	
	}
	
	public void EndScene ()
	{
		// Make sure the texture is enabled.
		guiTexture.enabled = true;
		
		// Start fading towards black.
		FadeToBlack();
		
		// If the screen is almost black...
		if(guiTexture.color.a >= 0.95f) {
			// ... reload the level.
		//	Application.LoadLevel(0);  // maybe load an ad here 
		}
	}
}
