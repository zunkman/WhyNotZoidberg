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


    protected override bool FloorCollide()
    {
        bool canjump = false;
        bool floor = false;
        float oldyspeed = speed.y;
        RaycastHit hit;


        for (int i = 0; i <= 2; i++)
        {
            float xoff = (width / 2) * (i - 1);

            if (Physics.Raycast(transform.position + new Vector3(xoff, magicNumber, 0f), Vector3.down, out hit, Mathf.Abs(speed.y * Time.deltaTime) + magicNumber) && speed.y <= 0)
            {
                //snap to floor, and enable jumping.
                transform.position += new Vector3(0f, -hit.distance + magicNumber, 0f);
                speed.y = 0f;
                if (isJumping) {
                    stun = fallDelay;
                    //camera.setOffset(new Vector3(0f, -1f, 0f));
                    speed.x = 0f;
                }
                canjump = true;
                isJumping = false;
                floor = true;
            }
        }

        if (floor)
        {
            RaycastHit h1, h2;
            if (Physics.Raycast(transform.position + new Vector3(-width / 2, height - magicNumber, 0f), Vector3.down, out h1) &&
                Physics.Raycast(transform.position + new Vector3(width / 2, height - magicNumber, 0f), Vector3.down, out h2))
            {

                float slope = Vector2.Angle(h2.point - h1.point, Vector2.right);
                Debug.Log(h1.point.ToString() + " " + h2.point.ToString());
                Debug.Log(slope);
                if (slope > maxSlope)
                {
                    canjump = false;
                    if (h1.distance < h2.distance) { transform.Translate(-oldyspeed * Mathf.Atan(slope * Mathf.Deg2Rad) * Time.deltaTime + magicNumber, 0f, 0f); speed.x = Mathf.Max(0f, speed.x); speed.y = oldyspeed; }
                    else { transform.Translate(oldyspeed * Mathf.Atan(slope * Mathf.Deg2Rad) * Time.deltaTime - magicNumber, 0f, 0f); speed.x = Mathf.Min(0f, speed.x); speed.y = oldyspeed; }

                }


            }


        }

        return canjump;
    }

    void Attack() { 
    
    }

}