using UnityEngine;
using System.Collections;

/*
This script will handle respawning all items in a level that should respawn.
*/

public class RespawnHandler : MonoBehaviour
{
    public GameObject[] respawnObjects;
    public Camera theCamera;
    private GameObject[] originalObjects;
	// Use this for initialization

	void Start ()
    {
        originalObjects = new GameObject[respawnObjects.Length];
        for (int x = 0; x < respawnObjects.Length; x++)
        {
            originalObjects[x] = Instantiate(respawnObjects[x]) as GameObject;
            originalObjects[x].SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        check();
	}

    // This checks all the gameObjects, to see if they're dead. If they are not active, 
    void check()
    {
        for (int x = 0; x < respawnObjects.Length; x++)
        {
            if (!respawnObjects[x].activeSelf)
            {
                // If the item is on camera, we do nothing.
                if (theCamera.WorldToViewportPoint(respawnObjects[x].transform.position).x < 1 && theCamera.WorldToViewportPoint(respawnObjects[x].transform.position).x > 0 && theCamera.WorldToViewportPoint(respawnObjects[x].transform.position).y < 1 && theCamera.WorldToViewportPoint(respawnObjects[x].transform.position).y > 0)
                {

                }

                else
                {
                    Destroy(respawnObjects[x]);
                    respawnObjects[x] = Instantiate (originalObjects[x]) as GameObject;
                    respawnObjects[x].SetActive(true);
                }
            }
        }
    }
}
