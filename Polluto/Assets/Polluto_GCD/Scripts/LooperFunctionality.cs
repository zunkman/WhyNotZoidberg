﻿using UnityEngine;
using System.Collections;

public class LooperFunctionality : MonoBehaviour
{
    //Basic looper stats
    private float looperHorSpeed;
    private float looperHorAcceleration;
    private float looperJumpSpeed;
    private float looperGravity;
    private float looperWidth;
    private float looperHeight;
    private int looperRaycast;
    private float looperTapGravity;
    private float looperMaxSlope;
    private float looperBaseHealth;
    [SerializeField]public float looperHealth;
    private float looperDamage;

    private float looperFullHealth;

    //Gamehandler object
    private GameObject gameHandlerObject;

    /* 
    Looper Functionality variables
    */
    /* is true if the shadow is spawned somewhere */
    private bool shadowAlive;

    private bool canSpawnShadow;

    /* Shadow object */
    public GameObject looperShadow;
    public GameObject looperShadowInstance;

    [SerializeField]private GameObject parentObject;

    [SerializeField]private BoxCollider attackBox;
    [SerializeField]private SpriteRenderer attackSprite;

    private int buttonTaps;
    private int playerNumber;
    private int dir;

    [SerializeField]private float markerPlacementTimer;
    [SerializeField]private float specialActiveTimer;
    [SerializeField]private float shadowCoolDown;
    [SerializeField]private float shadownTimer;

    [SerializeField]private float chargeAttackTime;
    [SerializeField]private float attackCharged;

    //Javalin variables
    [SerializeField]private GameObject javelin;

    public float javelinSpeed;
    public int javelinDirection;
    public float javelinDuration;

    void Start()
    {
        /*
        Setting up loopers movement stats at runtime
        */
        looperFullHealth = 100.0f;

        looperHorSpeed = 11.0f; 
        looperHorAcceleration = 50.0f;
        looperJumpSpeed = 27.0f; 
        looperGravity = 35.0f;
        looperWidth = 0.95f;
        looperHeight = 1.9f;
        looperRaycast = 20;
        looperTapGravity = 40.0f;
        looperMaxSlope = 47.0f;
        looperBaseHealth = 100;
        looperHealth = 100;
        looperDamage = 25.0f;
        

        transform.parent.GetComponent<Player>().horspeed = looperHorSpeed;
        transform.parent.GetComponent<Player>().horaccel = looperHorAcceleration;
        transform.parent.GetComponent<Player>().jumpspeed = looperJumpSpeed;
        transform.parent.GetComponent<Player>().gravity = looperGravity;
        transform.parent.GetComponent<Player>().width = looperWidth;
        transform.parent.GetComponent<Player>().height = looperHeight;
        transform.parent.GetComponent<Player>().raycasts = looperRaycast;
        transform.parent.GetComponent<Player>().tapGrav = looperTapGravity;
        transform.parent.GetComponent<Player>().maxSlope = looperMaxSlope;
        transform.parent.GetComponent<Player>().baseHealth = looperBaseHealth;
        transform.parent.GetComponent<Player>().health = looperHealth;
        playerNumber = this.GetComponentInParent<Player>().playerNumber;
        
        //transform.parent.GetComponent<Player>().health = looperHealth;
        //transform.parent.GetComponent<Player>().damage = looperDamage;

        //Getting the gamehandler object

        shadownTimer = 5.0f;

       /*
       Javalin stats set
       */

       javelinSpeed = 15.0f;
       javelinDuration = 2.0f;

    /*
    Looper Stats setup
    */
        attackBox = this.gameObject.GetComponent<BoxCollider>() as BoxCollider;
        attackSprite = this.gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
       
        attackBox.enabled = false;
        attackSprite.enabled = false;

        attackCharged = 3.0f;

        shadowAlive = false;
        canSpawnShadow = true;
    }

    // Update is called once per frame
    void Update()
    {        
        //Make a timer and put the placeing marker function on a timer for like four seconds

        Vector3 directionFacing = this.GetComponentInParent<Player>().speed;
        if (directionFacing.x > 1)
        {
            javelinDirection = 1;
        }
        else if(directionFacing.x < 0)
        {
            javelinDirection = 0;
        }
    

        doubleTap();
        resetTimer();
        abilities();
        looperSpecialActive();
    }

    void looperSpecialActive()
    {
        if (playerNumber == 1)
        {
            if (Input.GetAxis("Special Attack") >= 0.1f)
            {
                spawnShadow();
            }
        }

        if (playerNumber == 2)
        {
            if (Input.GetAxis("Special Attack 2") >= 0.1f)
            {
                spawnShadow();
            }
        }

        shadowCoolDownTimer();

    }

    /* Spawns loopers shadow. If loopers shadow is already spawned
    then teleport to the shadow and destroy it. */
    void spawnShadow()
    {
        if (shadowAlive == false && canSpawnShadow == true)
        {
            looperShadowInstance = Instantiate(looperShadow, this.gameObject.transform.position, Quaternion.identity) as GameObject;
            shadowAlive = true;
            shadowCoolDown = shadownTimer;
            if(this.GetComponentInParent<Player>().UIScript) this.GetComponentInParent<Player>().UIScript.startSpecialCooldown(playerNumber, shadowCoolDown);
            canSpawnShadow = false;
        }
        else if(shadowAlive == true && canSpawnShadow == true)
        {
            this.gameObject.transform.parent.transform.position = looperShadowInstance.transform.position;
            GameHandler gameHandlerScript = FindObjectOfType<GameHandler>();
            if(gameHandlerScript) {
                if(gameHandlerScript.getPlayerOne() != null) gameHandlerScript.getPlayerOne().transform.position = looperShadowInstance.transform.position;
                if(gameHandlerScript.getPlayerTwo() != null) gameHandlerScript.getPlayerTwo().transform.position = looperShadowInstance.transform.position;
            }

            Destroy(looperShadowInstance); 
            shadowAlive = false;
            shadowCoolDown = shadownTimer;
            if(this.GetComponentInParent<Player>().UIScript) this.GetComponentInParent<Player>().UIScript.startSpecialCooldown(playerNumber, shadowCoolDown);
            canSpawnShadow = false;
        }
    }

    /* Cooldown for the placing/swapping with a shadow */
    void shadowCoolDownTimer()
    {
        if (shadowCoolDown > 0.0f)
        {
            shadowCoolDown -= 1 * Time.deltaTime;
        }

        if (shadowCoolDown < 0.0f)
        {
            shadowCoolDown = 0.0f;
            canSpawnShadow = true;
        }
    }

    void abilities()
    {
        Attack();
    }


    void doubleTap()
    {
        if (specialActiveTimer > 0.0f)
        {
            specialActiveTimer -= 1 * Time.deltaTime;
        }
        else
        {
            buttonTaps = 0;
        }
    }
   
    /* Loopers basic attack and charge attack */
    void Attack()
    {
        if (this.GetComponentInParent<Player>().playerNumber == 1)
        {
            if (Input.GetAxis("Basic Attack") <= 0.1f && chargeAttackTime >= attackCharged)//this is key up
            {
                Vector3 dir = parentObject.GetComponent<Player>().speed;

                // 1 is right 0 is left

                chargeAttackTime = 0.0f;
                StartCoroutine(attackReset(0.2f));

                ChargeAttack();
            }

            /*if (Input.GetKey(KeyCode.W))
            {
                this.gameObject.transform.localPosition = new Vector3(0.0f, 0.5f, 0.0f);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                this.gameObject.transform.localPosition = new Vector3(0.0f, -0.5f, 0.0f);
            }
            else
            {
                this.gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            }*/


            if (chargeAttackTime > 0.0f && Input.GetAxis("Basic Attack") < 0.1f)
            {
                if (chargeAttackTime < attackCharged)
                {
                    Vector3 dir = parentObject.GetComponent<Player>().speed;

                    //1 is right 0 is left
                    if (dir.x > 0)
                    {
                        attackBox.enabled = true;
                        attackSprite.enabled = true;
                    }
                    else
                    {
                        attackBox.enabled = true;
                        attackSprite.enabled = true;
                    }  
                    StartCoroutine(attackReset(0.2f));
                    chargeAttackTime = 0;
                }
                
            }

            if (Input.GetAxis("Basic Attack") > 0.1f)
            {
                chargeAttackTime += 1 * Time.deltaTime;
            }
        }

        
        if(this.GetComponentInParent<Player>().playerNumber == 2)
        { 
            if (Input.GetAxis("Basic Attack 2") <= 0.1f && chargeAttackTime >= attackCharged)//this is key up
            {
                Vector3 dir = parentObject.GetComponent<Player>().speed;

                // 1 is right 0 is left

                chargeAttackTime = 0.0f;
                StartCoroutine(attackReset(0.2f));

                ChargeAttack();
            }

            /*if (Input.GetKey(KeyCode.W))
            {
                this.gameObject.transform.localPosition = new Vector3(0.0f, 0.5f, 0.0f);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                this.gameObject.transform.localPosition = new Vector3(0.0f, -0.5f, 0.0f);
            }
            else
            {
                this.gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            }*/

            // On key up, and the key was down first
            if (chargeAttackTime > 0.0f && Input.GetAxis("Basic Attack 2") < 0.1f)
            {
                if (chargeAttackTime < attackCharged)
                {
                    Vector3 dir = parentObject.GetComponent<Player>().speed;

                    //1 is right 0 is left
                    if (dir.x > 0)
                    {
                        attackBox.enabled = true;
                        attackSprite.enabled = true;
                    }
                    else
                    {
                        attackBox.enabled = true;
                        attackSprite.enabled = true;
                    }  
                    StartCoroutine(attackReset(0.2f));
                    chargeAttackTime = 0;
                }
                
            }
            

            if (Input.GetAxis("Basic Attack 2") > 0.1f)
            {
                chargeAttackTime += 1 * Time.deltaTime;

                
            }
        } 
    }

    /* Spawn the javalin */
    void ChargeAttack()
    {
        Vector3 javelinStart = this.gameObject.transform.position;
        javelinStart.y += 0.5f;

        Instantiate(javelin, javelinStart, Quaternion.identity);
    }

    /* Cooldown timer for loopers shadow */
    void resetTimer()
    {
        if (markerPlacementTimer > 0.0f)
        {
            markerPlacementTimer -= 1 * Time.deltaTime;
        }

        if (markerPlacementTimer < 0.0f)
        {
            markerPlacementTimer = 0.0f;
        }
    }

    IEnumerator attackReset(float time)
    {
        Debug.Log("AReset:" + time);
        if(this.GetComponentInParent<Player>().UIScript) this.GetComponentInParent<Player>().UIScript.startBasicCooldown(playerNumber, time);
        yield return new WaitForSeconds(time);
        attackBox.enabled = false;
        attackSprite.enabled = false;
    }


    void OnTriggerEnter(Collider colliderEnter)
    {
        if (colliderEnter.gameObject.tag == "Enemy")
        {
           // EnemyDamage hitHP = colliderEnter.gameObject.GetComponentInParent<EnemyDamage>();
           // if(hitHP != null) colliderEnter.gameObject.GetComponent<EnemyDamage>().takeDamage(looperDamage);


            if (colliderEnter.gameObject.GetComponentInParent<EnemyDamage>() == null)
            {
                Debug.Log("damage is in child.");
                colliderEnter.gameObject.GetComponentInChildren<EnemyDamage>().takeDamage(looperDamage);
            }
            else
            {
                colliderEnter.gameObject.GetComponentInParent<EnemyDamage>().takeDamage(looperDamage);
            }
        }
    }

}

