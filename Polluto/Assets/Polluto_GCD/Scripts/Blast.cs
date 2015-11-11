using UnityEngine;
using System.Collections;

/*
This will collect all the gameobjects that are in the blast zone.
*/
public class Blast : MonoBehaviour
{
    public GameObject[] bombTargets;
    public int targetNumber;
    public bool haveTargets;

	// Use this for initialization
	void Start ()
    {
        targetNumber = 0;
        bombTargets = new GameObject[100];
        haveTargets = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    // Checks if we collided with a possible target, if so we store them in blastTargets
    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy" || other.gameObject.tag == "Ground")
        {
            haveTargets = true;
            bombTargets[targetNumber] = other.gameObject;
            targetNumber++;
        }
    }
}
