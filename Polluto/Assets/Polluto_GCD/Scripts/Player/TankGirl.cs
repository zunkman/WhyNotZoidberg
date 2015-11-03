using UnityEngine;
using System.Collections;

public class TankGirl : Player
{

    [SerializeField]
    float JumpBonusSpeed;
    [SerializeField]
    float jumpDelay;
    [SerializeField]
    float fallDelay;
    float jumptimer;
    bool aboutToJump;
    float stun;
    PlayerFollow camera;
    public float attackDamage;


    protected override void Start()
    {
        speed = Vector3.zero;
        jumptimer = jumpDelay;
        camera = GameObject.Find("Main Camera").GetComponent<PlayerFollow>();
    }

    protected override void Update()
    {
        if (playerNumber == 1) inputDirection.x = Input.GetAxis("Horizontal");
        if (playerNumber == 2) inputDirection.x = Input.GetAxis("Horizontal 2");
        if (playerNumber == 1) inputDirection.y = Input.GetAxis("Vertical");
        if (playerNumber == 2) inputDirection.y = Input.GetAxis("Vertical 2");
        if (inputDirection != Vector2.zero) inputDirection.Normalize();

        if (stun >= 0f) { stun -= Time.deltaTime; }
        if (aboutToJump)
        {
            jumptimer -= Time.deltaTime;
            if (jumptimer < 0)
            {
                jumptimer = jumpDelay;
                speed.y = jumpspeed;
                speed.x = (horspeed + JumpBonusSpeed) * Input.GetAxis("Horizontal");
                aboutToJump = false;
                isJumping = true;
            }
        }

        else
        {
            if (!isJumping && stun < 0f)
            {
                HorMove();
            }
        }

        Gravity();

        bool canjump = FloorCollide();

        if (playerNumber == 1)
        {
            if (Input.GetButton("Jump") && canjump && stun < 0f)
            {
                //Again, a single line of code for now, but you can put animation-y stuff here.
                Jump();
            }
        }

        if (playerNumber == 2)
        {
            if (Input.GetButton("Jump 2") && canjump && stun < 0f)
            {
                //Again, a single line of code for now, but you can put animation-y stuff here.
                Jump();
            }
        }


        WallCollide();

        CeilCollide();
        transform.localPosition += speed * Time.deltaTime;
    }

    protected override void Jump()
    {
        aboutToJump = true;
        speed.x = 0;
    }


    

    void Attack() { 
    
    }

}