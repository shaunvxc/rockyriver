using UnityEngine;
using System.Collections;

#if UNITY_ANDROID
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
#endif

public class LeaderboardsManager : MonoBehaviour
{

		private string leaderboardID;
		private string leaderboardName;  // might not even need this!!!
		
		private readonly string leaderboardIdRookie_ios = "com.selloutsystems.rockyriverrookie";
		private readonly string leaderboardIdRookie_android = "CgkImaeR4aEaEAIQAA";

		private readonly string leaderboardIdVeteran_ios = "com.selloutsystems.rockyriverveteran";
		private readonly string leaderboardIdVeteran_android = "CgkImaeR4aEaEAIQAQ";

		public static LeaderboardsManager Instance;
			
		private static bool playGamesActive = false;
		
		void Awake() { 

			if (Instance == null) { 	
				Instance = this;
			}
		}
		
		// Use this for initialization
		void Start ()
		{
				
				if (PanchoRiver.Instance.getFallbackTime () > 10) { 
						
						#if UNITY_IPHONE
							leaderboardID = "com.selloutsystems.rockyriverrookie";
						#elif UNITY_ANDROID
							leaderboardID = "CgkImaeR4aEaEAIQAA";
						#endif
					
				} 
				else {
						#if UNITY_IPHONE
							leaderboardID = "com.selloutsystems.rockyriverveteran";
						#elif UNITY_ANDROID
							leaderboardID = "CgkImaeR4aEaEAIQAQ";
						#endif
				}

				#if UNITY_ANDROID
					if(PanchoRiver.Instance.titleScene && !playGamesActive) { 
						PlayGamesPlatform.Activate();	
						playGamesActive = true;
					}
				#endif
				
		}


		public string getActiveLeaderboardId() { 	
			return leaderboardID;
		}
		
		public string getRookieLeaderboardId() {
			#if UNITY_IPHONE	
				return leaderboardIdRookie_ios;
			#elif UNITY_ANDROID
				return leaderboardIdRookie_android;
			#endif
			
				return "";
		}

		
		public string getVeteranLeaderboardId() {
			#if UNITY_IPHONE	
				return leaderboardIdVeteran_ios;
			#elif UNITY_ANDROID
				return leaderboardIdVeteran_android;
			#endif
			
			return "";
		}
		
}

