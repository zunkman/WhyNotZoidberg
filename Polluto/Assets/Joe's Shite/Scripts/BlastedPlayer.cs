using UnityEngine;
using System.Collections;

/*
This script will handle player movement when the player has been blasted by a bomb.
*/

public class BlastedPlayer : MonoBehaviour
{
    [SerializeField] private float speed, maxSpeed, timePassed;
    public GameObject playerContainer, player;
    private Rigidbody playerRigid;
    private bool begun;

    // Use this for initialization
    void Start ()
    {
	    if (speed == 0)
        {
            speed = 10;
        }

        if (maxSpeed == 0)
        {
            maxSpeed = 20;
        }

        playerRigid = this.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        timePassed -= Time.deltaTime;
        playerContainer.transform.position = this.transform.position;
        player.transform.position = this.transform.position;
        if (begun)
        {
            applyInput(playerInput());
            playerContainer.transform.position = this.transform.position;
        }

        collisionCheck();
	}
    
    // This is called by bomb after it's given this gameobject instance it's parameters.
    public void begin()
    {
        player.GetComponent<Player>().enabled = false;
        begun = true;
    }

    // This will check if we've collided with something. If so, we disable this controller, and reenable the player controller.
    void collisionCheck ()
    {
        RaycastHit hit;

        if (Physics.Raycast(playerContainer.transform.position, Vector3.right, out hit, 1f) || Physics.Raycast(playerContainer.transform.position, Vector3.left, out hit, 1f) || Physics.Raycast(playerContainer.transform.position, Vector3.up, out hit, 1f))
        {
            if (hit.transform.gameObject.tag == "Ground")
            {
                playerRigid.velocity = new Vector3(0, 0.5f, 0);
                player.GetComponent<Player>().enabled = true;
                player.transform.position = this.transform.position;
                Destroy(this.gameObject);
            }
        }

        else if (timePassed <= 0 & Physics.Raycast(playerContainer.transform.position, Vector3.down, out hit, 1f))
        {
            playerRigid.velocity = new Vector3(0, 0.5f, 0);
            player.GetComponent<Player>().enabled = true;
            player.transform.position = this.transform.position;
            Destroy(this.gameObject);
        }
    }

    // This will obtain player input from the horizontal axis.
    float playerInput ()
    {
        float horizontalInput = 0;

        if (Input.GetAxis("Horizontal") >= 0.1 || Input.GetAxis("Horizontal") <= -0.1)
        {
            horizontalInput = Input.GetAxis("Horizontal");
        }

        return horizontalInput;
    }

    // This will add force to the rigidbody based on the player input.
    void applyInput (float horizontalInput)
    {
        if (horizontalInput >= 0.1f & playerRigid.velocity.x < maxSpeed)
        {
            playerRigid.AddForce(new Vector3(horizontalInput * speed, 0, 0));
        }

        else if (horizontalInput <= -0.1f & playerRigid.velocity.x > -maxSpeed)
        {
            playerRigid.AddForce(new Vector3(horizontalInput * speed, 0, 0));
        }

        else if (playerRigid.velocity.x > maxSpeed)
        {
            playerRigid.velocity = new Vector3(maxSpeed, playerRigid.velocity.y, playerRigid.velocity.z);
        }

        else if (playerRigid.velocity.x < -maxSpeed)
        {
            playerRigid.velocity = new Vector3(-maxSpeed, playerRigid.velocity.y, playerRigid.velocity.z);
        }

        else
        {

        }
    }
}
