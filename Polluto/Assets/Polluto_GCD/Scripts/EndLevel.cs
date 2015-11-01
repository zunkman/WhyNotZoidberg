using UnityEngine;
using System.Collections;

/*
This script will handle ending the level, by calling the appropriate function on GameHandler.
*/
public class EndLevel : MonoBehaviour
{
    [SerializeField] private bool oneHere, twoHere, twoPlayers;
    [SerializeField] private GameObject handler;
	// Use this for initialization
	void Start ()
    {
        handler = GameObject.FindGameObjectWithTag("gameHandler");

        if (handler.GetComponent<GameHandler>().getNumPlayers() > 1)
        {
            twoPlayers = true;
        }

        else
        {
            twoPlayers = false;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (twoPlayers && oneHere && twoHere)
        {
            Application.LoadLevel("MainMenu");
        }

        if (!twoPlayers && oneHere)
        {
            Application.LoadLevel("MainMenu");
        }
	}

    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponentInParent<Player>().playerNumber == 1)
        {
            oneHere = true;
        }

        if (twoPlayers && other.gameObject.tag == "Player" && other.gameObject.GetComponentInParent<Player>().playerNumber == 2)
        {
            twoHere = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponentInParent<Player>().playerNumber == 1)
        {
            oneHere = false;
        }

        if (twoPlayers && other.gameObject.tag == "Player" && other.gameObject.GetComponentInParent<Player>().playerNumber == 2)
        {
            twoHere = false;
        }
    }
}
