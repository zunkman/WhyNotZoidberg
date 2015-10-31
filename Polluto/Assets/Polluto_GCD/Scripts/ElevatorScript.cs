using UnityEngine;
using System.Collections;

public class ElevatorScript : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 finishPosition;
    private Vector3 newCurrentPosition;
    private Vector3 newPlayerPos;

    private GameObject player;

    public bool elevatorBroken;
    public bool missionComplete;
    private bool onElevator;
   
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
        playerElevatorAccel = 2.0f;
        playerElevatorSpeed = 1.5f;

	}
	
	// Update is called once per frame
	void Update ()
    {
        moveElevator();
	}

    /* Checks for player collision with the elevator and two other parameters to tell it which way to move */
    void OnTriggerEnter(Collider colliderEnter)
    {
        if (colliderEnter.gameObject.tag == "Player" && elevatorBroken == false && missionComplete == false)
        {
            atTop = 1;
            player = GameObject.FindGameObjectWithTag(colliderEnter.gameObject.tag);
            onElevator = true;
            newPlayerPos = player.transform.position;
            //player.transform.parent = this.gameObject.transform;
            //saveAccel = player.GetComponent<Player>().horaccel;
            //saveSpeed = player.GetComponent<Player>().horspeed;
            //player.GetComponent<Player>().horaccel = playerElevatorAccel;
            //player.GetComponent<Player>().horspeed = playerElevatorSpeed;

        }

        if (colliderEnter.gameObject.tag == "Player" && elevatorBroken == false && missionComplete == true)
        {
            atTop = 2;
            player = GameObject.FindGameObjectWithTag(colliderEnter.gameObject.tag);
            //player.transform.parent = this.gameObject.transform;
            //saveAccel = player.GetComponent<Player>().horaccel;
            //saveSpeed = player.GetComponent<Player>().horspeed;
            //player.GetComponent<Player>().horaccel = playerElevatorAccel;
            //player.GetComponent<Player>().horspeed = playerElevatorSpeed;
        }

    }

    /* Resets the players original stats */
    void OnTriggerExit(Collider colliderExit)
    {
        if (colliderExit.gameObject.tag == "Player")
        {
            onElevator = false;
            //player.transform.parent = null;
            //player.GetComponent<Player>().horaccel = saveAccel;
            //player.GetComponent<Player>().horspeed = saveSpeed;
        }

        /*if (colliderExit.gameObject.tag == "Player")
        {
            atTop = 2;
            player.transform.parent = null;
            player.GetComponent<Player>().horaccel = saveAccel;
            player.GetComponent<Player>().horspeed = saveSpeed;
        }*/

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
                    missionComplete = false;
                    this.gameObject.GetComponent<BoxCollider>().enabled = false;
                }
                else
                {
                    newPlayerPos.y -= elevatorSpeed * Time.deltaTime;
                    newCurrentPosition.y -= elevatorSpeed * Time.deltaTime;
                }
                if (onElevator)
                    player.gameObject.transform.position = newPlayerPos;
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
