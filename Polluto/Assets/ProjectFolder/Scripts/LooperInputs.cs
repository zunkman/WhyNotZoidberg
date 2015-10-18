using UnityEngine;
using System.Collections;

public class LooperInputs : MonoBehaviour
{
    public float looperSpeed;
    public float looperJumpHeight;
    public float loopeJumpForce;
    public bool JumpForwards;

    private string joystickOne;

    // Use this for initialization
    void Start ()
    {
        looperSpeed = 3.0f;
        looperJumpHeight = 75.0f;
        loopeJumpForce = 0.3f;

        this.gameObject.GetComponent<DefaultPlayerMovement>().p_MoveSpeed = looperSpeed;
        this.gameObject.GetComponent<DefaultPlayerMovement>().p_JumpHeight = looperJumpHeight;
        this.gameObject.GetComponent<DefaultPlayerMovement>().p_JumpForwardForce = loopeJumpForce;
        this.gameObject.GetComponent<DefaultPlayerMovement>().jumpForwardForce = true;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
      

        if (this.gameObject.GetComponent<DefaultPlayerMovement>().isJumping == false)
        {
            //looperSpeed = 4.0f;
            this.gameObject.GetComponent<DefaultPlayerMovement>().p_MoveSpeed = looperSpeed;
        }


	}
}
