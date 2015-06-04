using UnityEngine;
using System.Collections;
/**
 * 
 * 
 * Try and keep all localization stuff in 1 place! ;)
 * 
 * @author Shaun Viguerie
 * @date Jan 14 2015 (26th Birthday)
 */
public class LocalizationTextureSelector : MonoBehaviour
{
		private GUITexture texture;
			
		public Texture Chinese;
		public Texture English;
		public Texture Japanese;

		void Awake() { 				
				
				texture = GetComponent<GUITexture> ();
			
				if (Application.systemLanguage == SystemLanguage.English) { 
						texture.texture = English;
				} else if (Application.systemLanguage == SystemLanguage.Chinese) {
						texture.texture = Chinese;
				} else if (Application.systemLanguage == SystemLanguage.Japanese) { 
						// texture.texture = Japanese;			
				}			
		}
}

