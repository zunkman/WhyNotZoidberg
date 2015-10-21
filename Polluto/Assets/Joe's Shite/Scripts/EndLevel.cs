using UnityEngine;
using System.Collections;

/*
This script will handle ending the level, by calling the appropriate function on GameHandler.
*/
public class EndLevel : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Ya did it.");
        }
    }
}
