using UnityEngine;
using System.Collections;

/**
 * 
 * 
 * try 2 approaches to improve this: 1. load ad from Awake()
 * 
 * 2. Load the ad at the end of the previous scene...wait until the ad is loaded to start the level?
 * 
 * wrap function calls to GoogleMobileAd in singleton class to have
 * direct control over the management and displaying of ads!
 */
public class AdMobManager : MonoBehaviour
{
		public static AdMobManager Instance;
		private bool shouldShowAd = false;	
		private static bool adLoaded = false;
		
		void Awake ()
		{

				if (Instance == null) {
						Instance = this;
				}

				if (!GoogleMobileAd.IsInited) {
						//	Debug.Log ("Google admob not inited, initing");
						GoogleMobileAd.Init ();
				}
				
				GoogleMobileAd.controller.addEventListener (GoogleMobileAdEvents.ON_INTERSTITIAL_AD_LOADED, interstitialAdLoaded);	
		}

		void Start ()
		{ 
				if (PanchoRiver.Instance.titleScene) { 
						shouldShowAd = false;		
				} 
				else {
						if(PlayerPrefs.HasKey(RockyKeys.CAN_SERVE_ADS)) {				
							if (NetworkTester.Instance.isConnectedToNetwork ()) {	
									if (UnityEngine.Random.Range (0, 100) <= 9) { 
											shouldShowAd = true;
									} 
									else if(adLoaded) {
											Debug.Log ("Showing ad in second case!!!");
											shouldShowAd = true;
									}
							} else {
									Debug.Log ("No connection, won't show ads!");
							}
						}
						else {
							shouldShowAd = false;
						}
				}
		}
	
		private void interstitialAdLoaded ()
		{
				adLoaded = true;	
		}


		public void loadInterstitialAd() {
				if (shouldShowAd ) {
						if(!adLoaded) {
							GoogleMobileAd.LoadInterstitialAd ();	// do this in fade scene script... or right at GameOver?
						}
						else {	
							Debug.Log ("Ad is already loaded from last round so show this one!");	
						}
				}
		}
		
			
		public void showAdIfEligible ()
		{ 
				if (shouldShowAd && adLoaded) { 
						GoogleMobileAd.ShowInterstitialAd ();
						adLoaded = false;
				} else if (shouldShowAd && !adLoaded) {
						Debug.Log ("Ad not loaded in time... F!");
				}
		}

}

