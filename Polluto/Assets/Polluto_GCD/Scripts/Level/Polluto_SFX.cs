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
    //Polluto_SFX.playSound ("levelload");
	public static void playSound(string soundID){
	int SID = 0;
		switch(soundID){
		case "levelload":
		case "newlevel":
			SID = 0;//sfx2a - Rising
			break;
        case "teleport":
			SID = 1;//sfx3a - Volley Up
			break;
        case "zap":
			SID = 2;//sfx4a - BugZapper
			break;
		case "shoot":
		case "hitwall":
		case "hitbarrier":
        case "hit":
			SID = 3;//damage - Soft Hit
			break;
		case "bombDeath":
		case "pickup":
            SID = 4;//health up - Muffled tone
			break;
		case "bossDeath":
			SID = 5;//menu ding - DOS Tone
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
