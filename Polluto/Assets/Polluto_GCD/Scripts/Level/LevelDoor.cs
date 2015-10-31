using UnityEngine;
using System.Collections;

public class LevelDoor : MonoBehaviour {

	[SerializeField] public string levelToLoad = "";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D (Collider2D other) {
		if(other.gameObject.tag == "Player") {
		if(levelToLoad != "") {
			Application.LoadLevel (levelToLoad);
		}
		}
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (levelToLoad != "")
            {
                Application.LoadLevel(levelToLoad);
            }
        }
    }
}
