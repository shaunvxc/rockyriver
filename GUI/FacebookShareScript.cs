using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.IO;

//using Prime31;

public class FacebookShareScript : MonoBehaviour
{

		
		private GUITexture texture;
		private bool pushed;
		private bool _hasPublishActions;
		private string mode;

		private bool showFacebookDialog = false;
		private Texture2D screenCapture;

 		 //TODO listen for loginFailedEvents here
		void Awake ()
		{
				texture = GetComponent<GUITexture> ();	
			#if UNITY_IPHONE
			//		FacebookBinding.init ();		
			#endif

			#if UNITY_ANDROID
			//	    FacebookAndroid.init ();
			#endif
		}
	
		void Start ()
		{ 
    			pushed = false;
				
				if (PanchoRiver.Instance.getFallbackTime () > 10) {
						mode = "Rookie mode!";
				} else {
						mode = "Veteran mode!";
				}
		}
  
		void Update ()
		{
		
				for (int i = 0; i < Input.touchCount; i++) {
			
						Touch touch = Input.GetTouch (i);	
			
						if (texture.HitTest (touch.position, Camera.main)) {
								if (!pushed) { 
										transform.position = new Vector2 (transform.position.x, transform.position.y - .01F);
										pushed = true;	
										StartCoroutine(FacebookPostWithBytes(GUIManager.Instance.getScreenshotBytes()));
										//facebookUpdate();
								}
						} 
				}
		
		
				if (Input.touchCount == 0) { 
			
						if (pushed) {
								transform.position = new Vector2 (transform.position.x, transform.position.y + .01F);
								SoundEffectsHelper.Instance.MakeButtonClick ();
								pushed = false;
						} 
				}
		} 
		
		IEnumerator FacebookPostWithBytes(byte[] pngBytes) {
			var message = "I just scored " + PanchoRiver.Instance.getTotalScore () + " playing Rocky River on " + mode + " bit.ly/1ywhkwb";
			SPShareUtility.FacebookShareWithBytes (message, pngBytes);
			yield return null;
		}



		private void facebookPost(Texture2D screenshot) {
			var message = "I just scored " + PanchoRiver.Instance.getTotalScore () + " playing Rocky River on " + mode + " bit.ly/1ywhkwb";
			SPShareUtility.FacebookShare(message , screenshot);   
		}

  		void completionHandler( string error, object result )
		{
			if (error != null) {
					Debug.LogError (error);
			}
		}


		/* 
					private void facebookUpdate ()
		{ 
				//var pathToImage = Application.persistentDataPath + "/screenshot.png";
				if (screenCapture == null) { 
						string path = "file://" + System.IO.Path.Combine (Application.persistentDataPath, "screenshot.png");

						Texture2D screenshot = new Texture2D (Screen.width, Screen.height, TextureFormat.RGB24, false);		
						StartCoroutine (DownloadImage (path, screenshot));
				}else {
						facebookPost(screenCapture);
				}
		// facebookPost (GUIManager.Instance.getScreenShot ());
				//	var bytes = File.ReadAllBytes (Application.persistentDataPath + "/screenshot.png");
				//  screenshot.LoadImage (bytes);
				//  var message = "I just scored " + PanchoRiver.Instance.getTotalScore () + " playing Rocky River on " + mode + " bit.ly/1ywhkwb";
				//	SPShareUtility.FacebookShare(message , screenshot);                       
	    	     
				#if UNITY_IPHONE
					//		FacebookBinding.showFacebookComposer ("I just scored " + PanchoRiver.Instance.getTotalScore () + " playing Rocky River on " + mode + " bit.ly/1ywhkwb", pathToImage,  "");
				#endif


				#if UNITY_ANDROID
					//	var bytes = System.IO.File.ReadAllBytes( pathToImage );
					//	Facebook.instance.postImage( bytes, "im an image posted from Android", completionHandler );
				#endif
		}
	 */

	
}

