using UnityEngine;
using System.Collections;
//using Prime31;
using System.IO;
using System;

public class TwitterShareScript : MonoBehaviour
{

		private GUITexture texture;
		private bool pushed;
		private bool _hasPublishActions;
		private bool canUseTweetSheet;
		private string mode;
		private bool showTwitterDialog = false;
   		 //TODO listen for isLoginFailedEvent and handle errors there 
		private Texture2D screenCapture;

		void onEnable ()
		{
				// begin listening to twitter events 
				//TwitterManager.loginSucceededEvent += loginSuccessful;	
		}

		void OnDisable ()
		{
				// Remove all the event handlers from Twitter
				//TwitterManager.loginSucceededEvent -= loginSuccessful;
		}
    
		void Awake ()
		{
				texture = GetComponent<GUITexture> ();	
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
										StartCoroutine(TwitterPostWithBytes(GUIManager.Instance.getScreenshotBytes()));
										//twitterUpdate ();
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

		private void updateCanTweetFlag ()
		{
			#if UNITY_IPHONE
	//			canUseTweetSheet = TwitterBinding.isTweetSheetSupported () && TwitterBinding.canUserTweet ();
			#endif
		}
	

		IEnumerator TwitterPostWithBytes(byte[] bytes) { 
				var message = "I just scored " + PanchoRiver.Instance.getTotalScore () + " playing Rocky River on " + mode + " bit.ly/1ywhkwb";
				SPShareUtility.TwitterShareWithBytes (message, bytes);
				yield return null;
		}

		void twitterPost(Texture2D screenshot) {
				var message = "I just scored " + PanchoRiver.Instance.getTotalScore () + " playing Rocky River on " + mode + " bit.ly/1ywhkwb";
				SPShareUtility.TwitterShare (message, screenshot);
		}
	/*
		void loginSuccessful (string username)
		{
			Debug.Log ("Successfully logged in to Twitter: " + username + " Ready to post update!!");
			updateCanTweetFlag ();
			twitterUpdate ();
		}

		void screenshotReady() { 
			if (showTwitterDialog) {
					Debug.Log ("Twitter: ScreenShot is ready!");
					showTwitterDialog = false;
					twitterUpdate ();
			}
		}

	private void twitterUpdate ()
	{ 		
		
		//var pathToImage = Application.persistentDataPath + "/screenshot.png";
		if (screenCapture == null) {
			string path = "file://" + System.IO.Path.Combine (Application.persistentDataPath, "screenshot.png");
			
			Texture2D screenshot = new Texture2D (Screen.width, Screen.height, TextureFormat.RGB24, false);
			StartCoroutine (DownloadImage (path, screenshot));
		} else {
			twitterPost(screenCapture);
		}
		//	var bytes = File.ReadAllBytes (Application.persistentDataPath + "/screenshot.png");			
		//	screenshot.LoadImage (bytes);
		//	var message = "I just scored " + PanchoRiver.Instance.getTotalScore () + " playing Rocky River on " + mode + " bit.ly/1ywhkwb";
		//	SPShareUtility.TwitterShare (message, screenshot);
		
		
		#if UNITY_IPHONE
					if (!TwitterBinding.isLoggedIn ()) {
						Debug.Log ("user is not logged in, showing login dialog!");
						TwitterBinding.showLoginDialog ();
				} else {

						Debug.Log ("logged in, now we can show the tweet composer !!!");

						TwitterBinding.showTweetComposer ("I just scored " + PanchoRiver.Instance.getTotalScore () + " playing Rocky River on " + mode, pathToImage);
				}

		#elif UNITY_ANDROID
			if(!TwitterAndroid.isLoggedIn()) { 
							Debug.Log ("user is not logged in, showing login dialog!");
							TwitterAndroid.showLoginDialog ();
					}
					else {
							var bytes = System.IO.File.ReadAllBytes( pathToImage );
							TwitterAndroid.postStatusUpdate( "I just scored " + PanchoRiver.Instance.getTotalScore () + " playing Rocky River on " + mode, bytes );
					} 
		#endif
	}
	*/
}

