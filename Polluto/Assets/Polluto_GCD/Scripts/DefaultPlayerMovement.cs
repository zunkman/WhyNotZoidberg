using UnityEngine;
using System.Collections;

public class DefaultPlayerMovement : MonoBehaviour
{
    public float p_MoveSpeed;
    public float p_JumpHeight;
    public float p_JumpForwardForce;

    private float p_MoveDir;

    public bool jumpForwardForce;
    public bool isJumping;
    public bool gamePad = false;
    private bool p_JumpDir;

    private Rigidbody2D newRigid2D;

    private int joystickArrayLength;

    void Start()
    {
        joystickArrayLength = Input.GetJoystickNames().GetLength(0);
        newRigid2D = this.gameObject.GetComponent<Rigidbody2D>();

        if (joystickArrayLength == 0)
        {
            //Debug.Log("No Joystick found");
        }
        else
        {
            gamePad = true;
        }
    }

    /*
    Gets player direction and jump boolean
    */
    void Update()
    {
        if (gamePad == true)
        {
            p_MoveDir = Input.GetAxis("GamePad1Horizontal");

            p_Movement(p_MoveDir, p_MoveSpeed);

            p_JumpDir = Input.GetButton("GamePad1Jump");

            p_Jump(p_JumpDir, p_MoveDir, p_JumpHeight, p_JumpForwardForce);
        }
        else
        {
            p_MoveDir = Input.GetAxis("Horizontal");

            p_Movement(p_MoveDir, p_MoveSpeed);

            p_JumpDir = Input.GetButton("Jump");

            p_Jump(p_JumpDir, p_MoveDir, p_JumpHeight, p_JumpForwardForce);
        }
    }

    /*
    Made for 2d use only
    Changes the direction that the sprite is faceing
    */
    void p_CharacterDirection(float Direction,  GameObject sprite)
    {
        Vector2 p_facingDir;
        float left = 180;

        p_facingDir = new Vector2(0.0f, 0.0f);

        if (Direction > 0)
        {
            p_facingDir.x = left;
            sprite.gameObject.transform.eulerAngles = p_facingDir;
        }
        else if(Direction < 0)
        {
            p_facingDir.x = 0.0f;
            sprite.gameObject.transform.eulerAngles = p_facingDir;
        }
    }

    /*
    Movement Function
    */
    void p_Movement(float moveDirection, float moveSpeed)
    {
        if (moveDirection > 0.0f)
        {
            newRigid2D.AddForce(new Vector2(moveSpeed, 0.0f));
            p_CharacterDirection(moveDirection, this.gameObject);
        }
        else if (moveDirection < 0.0f)
        {
            newRigid2D.AddForce(new Vector2(-moveSpeed, 0.0f));
            p_CharacterDirection(moveDirection, this.gameObject);
        }
    }

    /*
    Jump Function 
    */
    void p_Jump(bool jumpDirY, float jumpDirX, float p_yJumpForce, float p_xJumpForce)
    {
        if (jumpForwardForce == true)
        {
            if (jumpDirY == true && jumpDirX > 0 && isJumping == false)
            {
                isJumping = true;
                newRigid2D.AddForce(new Vector2(p_xJumpForce, p_yJumpForce));
            }
            else if (jumpDirY == true && jumpDirX < 0 && isJumping == false)
            {
                isJumping = true;
                newRigid2D.AddForce(new Vector2(-p_xJumpForce, p_yJumpForce));
            }
            else if (jumpDirY == true && isJumping == false)
            {
                isJumping = true;
                newRigid2D.AddForce(new Vector2(0.0f, p_yJumpForce));
            }
        }
        else
        {
            if (jumpDirY == true && isJumping == false)
            {
                isJumping = true;
                newRigid2D.AddForce(new Vector2(0.0f, p_yJumpForce));
            }
        }
    }
}
