using UnityEngine;
using System.Collections;

public class SurveillanceCamera : MonoBehaviour
{
    [SerializeField] private bool pan, panningLeft, panningRight;
    [SerializeField] private float panSpeed, panRest;
    [SerializeField] private int [] turrets, guards;
    [SerializeField] private GameObject leftBound, rightBound, leftPan, rightPan, lens;

    private float visionArc, range;
    private GameObject respawner, handler, playerOne, playerTwo, visiblePlayer;
    private Rigidbody myRigid;
    private bool seenPlayer, twoPlayers;

    // Use this for initialization
    void Start ()
    {
        respawner = GameObject.Find("Respawning Handler");
        handler = GameObject.FindGameObjectWithTag("gameHandler");
        myRigid = this.gameObject.GetComponentInChildren<Rigidbody>();

        playerOne = handler.GetComponentInParent<GameHandler>().getPlayerOne();
        if (handler.GetComponentInParent<GameHandler>().getNumPlayers() > 1)
        {
            twoPlayers = true;
            playerTwo = handler.GetComponentInParent<GameHandler>().getPlayerTwo();
        }

        adjustVision();
    }
	
	// Update is called once per frame
	void Update ()
    {
	
        if (pan)
        {
            panCamera();
        }

        if (!seenPlayer)
        {
            visionCheck();
        }

        if (seenPlayer)
        {
            visiblePlayerCheck();
        }
	}

    // This will set up the camera's vision, and set the left and right bound objects to the child objects.
    void adjustVision ()
    {
        visionArc = this.gameObject.GetComponentInChildren<Light>().spotAngle / 2;
        range = this.gameObject.GetComponentInChildren<Light>().range;

        leftBound.transform.Rotate(0, 0, -visionArc / 2);
        rightBound.transform.Rotate(0, 0, visionArc / 2);

        leftBound = leftBound.transform.FindChild("Left Vision Bound").gameObject;
        rightBound = rightBound.transform.FindChild("Right Vision Bound").gameObject;

    }

    // This will cause the camera to pan.
    void panCamera ()
    {
        if (panningLeft)
        {
            myRigid.angularVelocity = new Vector3(0, 0, -panSpeed);

            // Check if we're at left bound.
            if ((lens.transform.position - leftPan.transform.position).magnitude < 0.1)
            {
                StartCoroutine(panResting(true));
            }
        }

        if (panningRight)
        {
            myRigid.angularVelocity = new Vector3(0, 0, panSpeed);

            // Check if we're at left bound.
            if ((lens.transform.position - rightPan.transform.position).magnitude < 0.1)
            {
                StartCoroutine(panResting(false));
            }
        }
    }

    // This will check if we see the player.
    void visionCheck ()
    {
        Vector3 toPlayer = playerOne.transform.position - this.transform.position, toLeft = this.transform.InverseTransformPoint(leftBound.transform.position) * range, toRight = this.transform.InverseTransformPoint(rightBound.transform.position) * range;
        // If the player is within the range of the camera, we check if we have direct line of sight to it.
        if ((toPlayer.normalized * range).x > toLeft.x && (toPlayer.normalized * range).x < toRight.x && toPlayer.y < 0 && Mathf.Abs(toPlayer.y) < range)
        {
            RaycastHit hit;
            if (Physics.Raycast(lens.transform.position, toPlayer, out hit, range) && hit.transform.gameObject.tag == "Player")
            {
                visiblePlayer = hit.transform.gameObject;
                seenPlayer = true;
            }
        }

        if (twoPlayers && !seenPlayer)
        {
            toPlayer = playerTwo.transform.position - this.transform.position;
            if ((toPlayer.normalized * range).x > toLeft.x && (toPlayer.normalized * range).x < toRight.x && toPlayer.y < 0 && Mathf.Abs(toPlayer.y) < range)
            {
                RaycastHit hit;
                if (Physics.Raycast(lens.transform.position, toPlayer, out hit, range) && hit.transform.gameObject.tag == "Player")
                {
                    visiblePlayer = hit.transform.gameObject;
                    seenPlayer = true;
                }
            }
        }

        
    }

    // This will be called when we see the player, to check when we stop seeing the player.
    void visiblePlayerCheck ()
    {
        RaycastHit hit;
        if (Physics.Raycast(lens.transform.position, visiblePlayer.transform.position, out hit, range) && hit.transform.gameObject.tag != "Player")
        {
            seenPlayer = false;
        }
        alert(visiblePlayer);
    }

    // This will be called when we stop seeing the character. We will pass the information to the various guards and turrets.
    void alert (GameObject thePlayer)
    {
        // Alert the turrets.
        for (int x = 0; x < turrets.Length; x++)
        {
            if (respawner.GetComponent<RespawnHandler>().respawnObjects[turrets[x]].gameObject.activeSelf)
            {
                respawner.GetComponent<RespawnHandler>().respawnObjects[turrets[x]].GetComponentInParent<Turret>().defaultFocus = thePlayer.transform.position;
                respawner.GetComponent<RespawnHandler>().respawnObjects[turrets[x]].GetComponentInParent<Turret>().focus = thePlayer.transform.position;
            }
            
        }

        // Alert the guards.
        for (int x = 0; x < guards.Length; x++)
        {
            if (respawner.GetComponent<RespawnHandler>().respawnObjects[guards[x]].gameObject.activeSelf)
            {
                respawner.GetComponent<RespawnHandler>().respawnObjects[guards[x]].GetComponentInParent<Guard>().alert(thePlayer.transform.position);
            }
            
        }
    }

    // This will tell the camera to hold still for a few seconds.
    IEnumerator panResting (bool panLeft)
    {
        float timePassed = 0;
        panningLeft = false;
        panningRight = false;
        myRigid.angularVelocity = new Vector3(0, 0, 0);
        do
        {
            timePassed += Time.deltaTime;
            yield return null;
        } while (timePassed < panRest);

        if (panLeft)
        {
            panningRight = true;
        }

        else
        {
            panningLeft = true;
        }
    }
}
