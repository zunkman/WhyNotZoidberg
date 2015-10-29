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


    protected override void Start()
    {
        speed = Vector3.zero;
        jumptimer = jumpDelay;
        camera = GameObject.Find("Main Camera").GetComponent<PlayerFollow>();
    }

    protected override void Update()
    {
        Debug.Log(stun);
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
        if (Input.GetButton("Jump") && canjump && stun < 0f)
        {

            Jump();
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