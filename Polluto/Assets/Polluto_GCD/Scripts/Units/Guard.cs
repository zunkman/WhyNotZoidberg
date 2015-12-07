using UnityEngine;
using System.Collections;

public class Guard : Turret
{
    [SerializeField] private GameObject leftBound, rightBound;
    [SerializeField] private bool facingLeft, facingRight, cameraAlert;
    [SerializeField] private float moveSpeed;

    private Vector3 leftDirection, rightDirection;
    private Rigidbody myRigid;

	// Use this for initialization
	public override void childStart ()
    {
        myRigid = this.gameObject.GetComponentInChildren<Rigidbody>();
        
        

        if (facingLeft)
        {
            defaultFocus = leftBound.transform.position;
            focus = defaultFocus;
            leftDirection = leftBound.transform.position - this.transform.position;
        }

        else
        {
            defaultFocus = rightBound.transform.position;
            focus = defaultFocus;
            rightDirection = rightBound.transform.position - this.transform.position;
        }
	}
	
	// Update is called once per frame
	public override void childUpdate ()
    {
        if (!cameraAlert)
        {
            updateFocus();
        }

        if (!haveTarget)
        {
            patrolVision();
            patrol();
        }

        else
        {
            myRigid.velocity = new Vector3(0, 0, 0);
        }
    }

    // This function updates the focus as we patrol.
    void updateFocus ()
    {
        if (facingLeft)
        {
            defaultFocus = leftBound.transform.position;
            focus = defaultFocus;
        }

        else
        {
            defaultFocus = rightBound.transform.position;
            focus = defaultFocus;
        }
    }

    // This function will cause the Guard to Patrol.
    void patrol ()
    {
        if (facingLeft)
        {
            myRigid.velocity = leftDirection.normalized * moveSpeed;

            if ((leftBound.transform.position - this.transform.position).magnitude < 2)
            {
                facingLeft = false;
                facingRight = true;
                rightDirection = rightBound.transform.position - this.transform.position;
            }
        }

        if (facingRight)
        {
            myRigid.velocity = rightDirection.normalized * moveSpeed;

            if ((rightBound.transform.position - this.transform.position).magnitude < 2)
            {
                facingLeft = true;
                facingRight = false;
                leftDirection = leftBound.transform.position - this.transform.position;
            }
        }
    }

    // This function will have the guard always looking left and right.
    void patrolVision ()
    {
        RaycastHit hit;
        if (!haveTarget && facingLeft && Physics.Raycast(this.transform.position, leftDirection, out hit, 100) && hit.transform.gameObject.tag == "Player")
        {
            player = hit.transform.gameObject;
            haveTarget = true;
        }

        if (!haveTarget && facingRight && Physics.Raycast(this.transform.position, rightDirection, out hit, 100) && hit.transform.gameObject.tag == "Player")
        {
            player = hit.transform.gameObject;
            haveTarget = true;
        }
    }

    public void alert (Vector3 inputFocus)
    {
        defaultFocus = inputFocus;
        focus = defaultFocus;
        cameraAlert = true;
    }
}
