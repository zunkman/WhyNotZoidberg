using UnityEngine;
using System.Collections;

public class testPlayerScript : MonoBehaviour
{

    private float looperHorSpeed;
    private float looperHorAcceleration;
    private float looperJumpSpeed;
    private float looperGravity;
    private float looperWidth;
    private float looperHeight;
    private int looperRaycast;
    private float looperTapGravity;
    private float looperMaxSlope;
    [SerializeField]
    private float looperHealth;
    private float looperDamage;

    private float looperFullHealth;

    //Gamehandler object
    private GameObject gameHandlerObject;

    // Use this for initialization
    void Start ()
    {
        looperFullHealth = 100.0f;

        looperHorSpeed = 11.0f; //normaly 800
        looperHorAcceleration = 50.0f;
        looperJumpSpeed = 26.0f;
        looperGravity = 55.0f;
        looperWidth = 0.2f;
        looperHeight = 1.0f;
        looperRaycast = 20;
        looperTapGravity = 100.0f;
        looperMaxSlope = 47.0f;
        looperHealth = looperFullHealth;
        looperDamage = 10.0f;

        transform.parent.GetComponent<Player>().horspeed = looperHorSpeed;
        transform.parent.GetComponent<Player>().horaccel = looperHorAcceleration;
        transform.parent.GetComponent<Player>().jumpspeed = looperJumpSpeed;
        transform.parent.GetComponent<Player>().gravity = looperGravity;
        transform.parent.GetComponent<Player>().width = looperWidth;
        transform.parent.GetComponent<Player>().height = looperHeight;
        transform.parent.GetComponent<Player>().raycasts = looperRaycast;
        transform.parent.GetComponent<Player>().tapGrav = looperTapGravity;
        transform.parent.GetComponent<Player>().maxSlope = looperMaxSlope;
        //transform.parent.GetComponent<Player>().health = looperHealth;
        //transform.parent.GetComponent<Player>().damage = looperDamage;

        //Getting the gamehandler object
        gameHandlerObject = GameObject.FindGameObjectWithTag("gameHandler");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
