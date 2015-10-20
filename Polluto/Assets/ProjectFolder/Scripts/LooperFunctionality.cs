using UnityEngine;
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
    [SerializeField]public float looperHealth;
    private float looperDamage;

    private float looperFullHealth;

    //Gamehandler object
    private GameObject gameHandlerObject;

    /* 
    Looper Functionality variables
    */
    [SerializeField]private Vector3[] shadowArray;

    Vector3 emptyVector;

    [SerializeField]private bool duplicateData;
    [SerializeField]private bool arrayFull;
    private bool shadow;
    private bool spawnShadow;

    public GameObject looperShadow;
    private GameObject shadowObj;
    [SerializeField]private GameObject parentObject;

    [SerializeField]private BoxCollider attackBox;

    [SerializeField]private int arraySlotsFull;
    private int buttonTaps;
    [SerializeField]private int dir;

    [SerializeField]private float markerPlacementTimer;
    [SerializeField]private float specialActiveTimer;

    [SerializeField]private float chargeAttackTime;
    [SerializeField]private float attackCharged;

    //Javalin variables
    [SerializeField]private GameObject javalin;

    public float javalinSpeed;
    public int javalinDirection;
    public float javalinDuration;

    void Start()
    {
        /*
        Setting up loopers movement stats at runtime
        */
        looperFullHealth = 100.0f;

        looperHorSpeed = 11.0f; 
        looperHorAcceleration = 50.0f;
        looperJumpSpeed = 26.0f; 
        looperGravity = 55.0f;
        looperWidth = 0.95f;
        looperHeight = 1.0f;
        looperRaycast = 20;
        looperTapGravity = 100.0f;
        looperMaxSlope = 47.0f;
        looperHealth = looperFullHealth;
        looperDamage = 10.0f;

        transform.parent.GetComponent<Player>().horspeed = looperHorSpeed;
        transform.parent.GetComponent<Player>().horaccel = looperHorAcceleration;
        transform.parent.GetComponent<Player>().jumpspeed = looperJumpSpeed;
        transform.parent.GetComponent<Player>().gravity = looperGravity;
        transform.parent.GetComponent<Player>().width = looperWidth;
        transform.parent.GetComponent<Player>().height = looperHeight;
        transform.parent.GetComponent<Player>().raycasts = looperRaycast;
        transform.parent.GetComponent<Player>().tapGrav = looperTapGravity;
        transform.parent.GetComponent<Player>().maxSlope = looperMaxSlope;
        //transform.parent.GetComponent<Player>().health = looperHealth;
        //transform.parent.GetComponent<Player>().damage = looperDamage;

        //Getting the gamehandler object
        gameHandlerObject = GameObject.FindGameObjectWithTag("gameHandler");

       /*
       Javalin stats set
       */

       javalinSpeed = 15.0f;
       javalinDuration = 2.0f;

    /*
    Looper Stats setup
    */

        shadowArray = new Vector3[4];
        
        parentObject = this.gameObject.transform.parent.gameObject;
        attackBox = this.gameObject.GetComponent<BoxCollider>() as BoxCollider;
       
        attackBox.enabled = false;
        attackCharged = 3.0f;
        //find a way to get the javalin object and spawn it

    }

    // Update is called once per frame
    void Update()
    {
        /*
        Currently the swap mechanic will work as such. The player will go to change positions but will place a marker which will
        change the current shadow position to the next in line then the player will swap places witht he next shadow
        making this a super duper hard mechanic to use, but will be really fun a rewarding when you get it right
        */

        //looperHealth = this.gameObject.GetComponentInParent<Player>().health;
        
        //Make a timer and put the placeing marker function on a timer for like four seconds
        if (Input.GetKeyDown(KeyCode.P) /*&& markerPlacementTimer == 0.0f*/)
        {
            if (specialActiveTimer > 0.0f && buttonTaps == 1 && arrayFull == true)
            {
                checkArrayFull();
                swapWithShadow();
            }
            else
            {
                specialActiveTimer = 4.0f;
                buttonTaps = 1;

                looperFunctionality();
                markerPlacementTimer = 4.0f; 
            }
        }

        /*Testing*/
        Vector3 directionFacing = parentObject.GetComponent<Player>().speed;
        if (directionFacing.x > 1)
        {
            javalinDirection = 1;
        }
        else if(directionFacing.x < 0)
        {
            javalinDirection = 0;
        }
    

        doubleTap();
        resetTimer();
        Attack();
        checkPlayerHealth(this.looperHealth);
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

    void swapWithShadow()
    {
        if (shadow == true)
        {
            //Vector3 newShadowPosition = transform.parent.transform.position;
            transform.parent.transform.position = shadowArray[0];//You teleport to shadow
            shadowArray[0] = emptyVector;

            //shadowObj.transform.position = shadowArray[3];
            specialActiveTimer = 0.0f;
        }
    }



    //Done
    void looperFunctionality()
    {
        emptyVector = new Vector3(0.0f, 0.0f, 0.0f);
        
        //stores the position of the stopping point
        Vector3 tempVector = transform.parent.transform.position;

        //This will check for duplicate data and store non duplicate data
        for (int i = 0; i < shadowArray.Length; i++)
        {
            if (shadowArray[i] != emptyVector)
            {
                if (shadowArray[i] == transform.parent.transform.position)
                {
                    duplicateData = true;
                    break;
                }
                else
                {
                    duplicateData = false;
                }
            }
        }
        
        //This places data that is not duplicated into the array
        if (duplicateData == false)
        {
            for (int i = 0; i < shadowArray.Length; i++)
            {
                if (shadowArray[i] == emptyVector && duplicateData != true)
                {
                    shadowArray[i] = tempVector;
                    break;
                }
            }
        }

        checkArrayFull();


        
        //If the array is full then delete the oldest data in the array which would be zero
        if (arrayFull == true && duplicateData != true)
        {
            shadowArray[0] = shadowArray[1];
            shadowArray[1] = shadowArray[2];
            shadowArray[2] = shadowArray[3];
            shadowArray[3] = tempVector;

            canSpawnShadow();
        } 
    }

    void checkArrayFull()
    {
        //This whill check if the array is full
        for (int i = 0; i < shadowArray.Length; i++)
        {
            if (shadowArray[i] != emptyVector)
            {
                arraySlotsFull = i + 1;
            }

            if (arraySlotsFull >= 4)
            {
                arrayFull = true;
            }
            else
            {
                arraySlotsFull = 0;
                arrayFull = false;
            }

        }
    }

    /* Checks the current health and if it's lower than 0 or is zero 
    respawn the player */
    void checkPlayerHealth(float healthToCheck)
    {
        if (healthToCheck < 0.0f)
        {
            
            //Debug.Log(this.parentObject);
            gameHandlerObject.GetComponent<GameHandler>().respawnPlayer(this.gameObject);
            this.looperHealth = looperFullHealth;
            //this.looperHealth = this.looperFullHealth;
            //this.gameObject.GetComponentInParent<Player>().health = looperFullHealth;
        }

    }

    
    void canSpawnShadow()
    {
        for (int i = 0; i < shadowArray.Length; i++)
        {
            if (shadowArray[i] == emptyVector)
            {
                spawnShadow = false;
                break;
            }
            else
            {
                spawnShadow = true;
            }

        }

        if(spawnShadow == true)
        {
            if (shadow == false)
            {
                shadow = true;
                shadowObj = Instantiate(looperShadow, shadowArray[0], Quaternion.identity) as GameObject;
            }
            else
            {
                shadowObj.transform.position = shadowArray[0];
            }
        }
    }

    void Attack()
    {
        

        if (Input.GetKeyUp(KeyCode.O) && chargeAttackTime >= attackCharged)
        {
            ChargeAttack();
        }

        if (Input.GetKey(KeyCode.W))
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
        }


        if (Input.GetKeyDown(KeyCode.O))
        {
            Vector3 dir = parentObject.GetComponent<Player>().speed;

            // 1 is right 0 is left
            if (dir.x > 0)
            {
                attackBox.enabled = true;
            }
            else
            {
                attackBox.enabled = true;
            }
            chargeAttackTime = 0.0f;
            StartCoroutine(attackReset(0.2f));
        }

        if (Input.GetKey(KeyCode.O))
        {
            chargeAttackTime += 1 * Time.deltaTime;
        }

        
    }

    void ChargeAttack()
    {
        Vector3 javalinStart = this.gameObject.transform.position;
        javalinStart.y += 0.5f;

        Instantiate(javalin, javalinStart, Quaternion.identity);
    }


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
        yield return new WaitForSeconds(time);
        attackBox.enabled = false;
    }


    void OnTriggerExit(Collider colliderExit)
    {
        if (colliderExit.gameObject.tag == "Attack")
        {
            Debug.Log("HIt");
            //gameHandler.GetComponent<GameHandler>().switchesOn += 1;
            //gameHandler.GetComponent<GameHandler>().subterfugeMissionUpdater();
            //this.gameObject.GetComponent<switchBehaviour>().enabled = false;
        }
    }

}

