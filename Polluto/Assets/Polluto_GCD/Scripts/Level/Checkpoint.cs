using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    public bool active;
    [SerializeField] private bool changeSpawn, updateMission;
    [SerializeField] private string updateText;
    private GameObject UI, gameHandler;


	// Use this for initialization
	void Start ()
    {
        UI = GameObject.FindGameObjectWithTag("UI").gameObject;
        gameHandler = GameObject.FindGameObjectWithTag("gameHandler");
        active = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            active = true;

            if (updateMission)
            {
                UI.GetComponentInChildren<PlayerUI>().updateObjective(updateText);
            }

            if (changeSpawn)
            {
                gameHandler.GetComponentInChildren<GameHandler>().playerSpawnOne.transform.position = this.transform.position;
            }
        }
    }
}
