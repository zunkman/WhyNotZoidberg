using UnityEngine;
using System.Collections;

public class ElevatorScript : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 finishPosition;
    private Vector3 newCurrentPosition;

    private GameObject player;

    private bool elevatorBroken;
    public bool missionComplete;
   
    private int atTop;

    private float saveAccel;
    private float saveSpeed;
    private float elevatorSpeed;
    private float playerElevatorAccel;
    private float playerElevatorSpeed;

    // Use this for initialization
    void Start ()
    {
        startPosition = this.gameObject.transform.position;
        finishPosition = new Vector3(-7.5f, -71.5f, 0.0f);
        newCurrentPosition = startPosition;
        elevatorBroken = false;
        missionComplete = false;
        elevatorSpeed = 9.5f;
        playerElevatorAccel = 1.0f;
        playerElevatorSpeed = 0.5f;

	}
	
	// Update is called once per frame
	void Update ()
    {
        moveElevator();
	}

    void OnTriggerEnter(Collider colliderEnter)
    {
        if (colliderEnter.gameObject.tag == "Player" && elevatorBroken == false && missionComplete == false)
        {
            atTop = 1;
            player = GameObject.FindGameObjectWithTag(colliderEnter.gameObject.tag);
            player.transform.parent = this.gameObject.transform;
            saveAccel = player.GetComponent<Player>().horaccel;
            saveSpeed = player.GetComponent<Player>().horspeed;
            player.GetComponent<Player>().horaccel = playerElevatorAccel;
            player.GetComponent<Player>().horspeed = playerElevatorSpeed;

        }
        else if (colliderEnter.gameObject.tag == "Player" && elevatorBroken == true && missionComplete == true)
        {
            atTop = 2;
            player = GameObject.FindGameObjectWithTag(colliderEnter.gameObject.tag);
            player.transform.parent = this.gameObject.transform;
            saveAccel = player.GetComponent<Player>().horaccel;
            saveSpeed = player.GetComponent<Player>().horspeed;
            player.GetComponent<Player>().horaccel = playerElevatorAccel;
            player.GetComponent<Player>().horspeed = playerElevatorSpeed;
        }

    }

    void OnTriggerExit(Collider colliderExit)
    {
        if (colliderExit.gameObject.tag == "Player")
        {
            player.transform.parent = null;
            player.GetComponent<Player>().horaccel = saveAccel;
            player.GetComponent<Player>().horspeed = saveSpeed;
        }
        else if (colliderExit.gameObject.tag == "Player")
        {
            atTop = 2;
            player.transform.parent = null;
            player.GetComponent<Player>().horaccel = saveAccel;
            player.GetComponent<Player>().horspeed = saveSpeed;
        }

    }

    void moveElevator()
    {
        switch (atTop)
        {
            case 1:
                if (newCurrentPosition.y <= finishPosition.y)
                {
                    newCurrentPosition.y = finishPosition.y;
                    atTop = 3;
                    elevatorBroken = true;
                }
                else
                {
                    newCurrentPosition.y -= elevatorSpeed * Time.deltaTime;
                }
                this.gameObject.transform.position = newCurrentPosition;
                break;
            case 2:
                if (newCurrentPosition.y >= startPosition.y)
                {
                    newCurrentPosition.y = startPosition.y;
                    atTop = 3;
                }
                else
                {
                    newCurrentPosition.y += elevatorSpeed * Time.deltaTime;
                }
                this.gameObject.transform.position = newCurrentPosition;
                break;

            default:
                break;
        }
    }
}
