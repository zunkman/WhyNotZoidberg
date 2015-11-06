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

    float chargeMult = 1.5f;

    float attackWindUp = 0.1f;
    float attackWindDown = 0.65f;
    float chargeWindDown = 0.5f;
    float chargeAttackWindDown = 1.2f;
    float attackDuration = 0.25f;

    float attackPrepare;

    bool isAttacking = false;
    public bool charging = false;
    int chargeDir = 0;

    TankgirlAttack attack;


    protected override void Start()
    {
        attack = GetComponentInChildren<TankgirlAttack>();
        speed = Vector3.zero;
        jumptimer = jumpDelay;
        camera = GameObject.Find("Main Camera").GetComponent<PlayerFollow>();
    }

    protected override void Update()
    {
        if (playerNumber == 1)
        {
            inputDirection.x = Input.GetAxis("Horizontal");
            inputDirection.y = Input.GetAxis("Vertical");
            if(Input.GetButton("Basic Attack") && stun<0f){
                Attack();
            }
            if (Input.GetButton("Special Attack") && stun < 0f) {
                Charge();
            }

        }
        if (playerNumber == 2)
        {
            inputDirection.x = Input.GetAxis("Horizontal 2");
            inputDirection.y = Input.GetAxis("Vertical 2");
            if (Input.GetButton("Basic Attack 2") && stun < 0f)
            {
                Attack();
            }

        } 
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
                if (charging) {
                    speed.x *= chargeMult;
                }
                aboutToJump = false;
                isJumping = true;
                charging = false;
            }
        }

        else
        {
            if (!isJumping && stun < 0f)
            {
                HorMove();
                if (charging)
                {
                    speed.x *= chargeMult;
                    Charging();
                }
                
            }
            if (stun > 0f) {
                if (speed.x > 0) { speed.x = Mathf.Max(speed.x - horaccel/4f * Time.deltaTime, 0); }
                else { speed.x = Mathf.Min(speed.x + horaccel/4f * Time.deltaTime, 0); }
            }
        }

        Gravity();

        bool canjump = FloorCollide();
        if (canjump == true && isJumping) { 
        
        }

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

        if (isAttacking) {
            attackPrepare -= Time.deltaTime;
            if (attackPrepare < 0) {
                if (attack.isActive)
                {
                    attack.isActive = false;
                    isAttacking = false;
                }
                else {
                    attack.isActive = true;
                    attackPrepare = attackDuration;
                }
            }
        }

    }

    protected override void Jump()
    {
        aboutToJump = true;
        speed.x = 0;
        
    }


    

    void Attack() {
        if (!charging)
        {
            stun = attackWindUp + attackWindDown;
            isAttacking = true;
            attackPrepare = attackWindUp;
        }
        else {
            stun = chargeAttackWindDown;
            attack.isActive = true;
            attackPrepare = attackDuration;
            isAttacking = true;
            charging = false;
        }
        //speed.x = 0;
    }
    void Charge() {
        if (speed.x != 0) {
            charging = true;
            chargeDir = (int)Mathf.Sign(speed.x);
        }
    }
    void Charging() {
        if (playerNumber == 1) {
            if (Mathf.Sign(Input.GetAxis("Horizontal")) != chargeDir) {
                charging = false;
                stun = chargeWindDown;
            }
        }

        if (playerNumber == 2)
        {
            if (Mathf.Sign(Input.GetAxis("Horizontal 2")) != chargeDir)
            {
                charging = false;
                stun = chargeWindDown;
            }
        }
    }
}