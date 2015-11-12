using UnityEngine;
using System.Collections;

public class Polluto_SFX : MonoBehaviour {

	[SerializeField] public AudioSource[] soundList = null;
	
	[SerializeField] private static AudioSource[] soundsList = null;
	private static bool loadOnceSFX = false;
		
	void Awake ()
	{
		//pickupSnd = soundList[0];
		if(loadOnceSFX == false) {
			//Debug.Log ("wasFalse");
			loadOnceSFX = true;
			DontDestroyOnLoad (this.gameObject);
			DontDestroyOnLoad (this.transform.parent.gameObject);
			soundsList = new AudioSource[soundList.Length];
			for(int i=0;i<soundList.Length;i++) {
				DontDestroyOnLoad (soundList[i]);
				soundsList[i] = soundList[i];
				DontDestroyOnLoad (soundsList[i]);
			}
            Polluto_SFX.playSound ("levelload");
		} else {
			Debug.Log ("SFX_wasTrue");
            if (this.transform.parent)
            {
                Destroy (this.transform.parent.gameObject);
            }
			Destroy (this.gameObject);
		}
	}

	public static void playSound(string soundID){
	int SID = 0;
		switch(soundID){
		case "pickup":
			SID = 0;
			break;
		case "newlevel":
		case "levelload":
        case "testmusic":
			SID = 1;
			break;
		case "hitwall":
		case "hitbarrier":
        case "teleport":
			SID = 2;
			break;
		case "shoot":
			SID = 3;
			break;
		case "bombDeath":
			SID = 4;
			break;
		case "bossDeath":
			SID = 5;
			break;
		case "playerdeath":
		case "playerDeath":
			SID = 6;
			break;
		case "bump"://missing audio clip?
			SID = 7;
			break;
		default:
			SID = 0;
			break;
		}
		if(SID < soundsList.Length && SID >= 0) {
			soundsList[SID].Play ();
			//Debug.Log ("Playing: " + soundsList[SID]);
		} else {
			//Debug.Log ("Sound: " + SID + " does not exist... at the moment.");
		}
	}
}
