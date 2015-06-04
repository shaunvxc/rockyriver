using UnityEngine;
using System.Collections;
/**
 * 
 * @author Shaun Viguerie
 * 
 * (C) %SelloutSystems
 */
public class NetworkTester : MonoBehaviour
{
			
		private static bool pingSent = false;

		public static bool hasNetworkConnection;
		public static NetworkTester Instance;
		
		void Awake ()
		{ 
				if (Instance == null) {
						Instance = this;
				}	
			
		}

		void Start() {
						
			if (PanchoRiver.Instance.titleScene && !hasNetworkConnection) {  // test this!!
				StartCoroutine (CheckConnectionToMasterServer ());
			} 
				
		}
		
		
		public bool isConnectedToNetwork ()
		{			
				if (hasNetworkConnection) {

						return true;
				} 
				else {
						return false;
				}
		}

		private IEnumerator CheckConnectionToMasterServer() {
				
				Ping pingMasterServer = new Ping ("67.225.180.24");
				Debug.Log (pingMasterServer.ip);
				
				float startTime = Time.time;
				Debug.Log ("Start time of Ping: " + startTime);
				
				while (!pingMasterServer.isDone && Time.time < startTime + 3.0f) {
						yield return new WaitForSeconds (0.1f);
				}
			
				if (pingMasterServer.isDone) {
						Debug.Log ("Finish time of Ping: " + Time.time);			
						hasNetworkConnection = true;
				} else {
						Debug.Log ("Ping timed out-- not a good network connection");
						hasNetworkConnection = false;		
				}

				pingSent = true;
		}

}

