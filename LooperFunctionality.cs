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
    private int dir;

    [SerializeField]private float markerPlacementTimer;
    [SerializeField]private float specialActiveTimer;



    void Start()
    {
        /*
        Setting up loopers movement stats at runtime
        */
        looperHorSpeed = 9.5f;
        looperHorAcceleration = 65.0f;
        looperJumpSpeed = 19.0f;
        looperGravity = 35.0f;
        looperWidth = 1.0f;
        looperHeight = 1.0f;
        looperRaycast = 20;
        looperTapGravity = 25.0f;
        looperMaxSlope = 47.0f;

        transform.parent.GetComponent<Player>().horspeed = looperHorSpeed;
        transform.parent.GetComponent<Player>().horaccel = looperHorAcceleration;
        transform.parent.GetComponent<Player>().jumpspeed = looperJumpSpeed;
        transform.parent.GetComponent<Player>().gravity = looperGravity;
        transform.parent.GetComponent<Player>().width = looperWidth;
        transform.parent.GetComponent<Player>().height = looperHeight;
        transform.parent.GetComponent<Player>().raycasts = looperRaycast;
        transform.parent.GetComponent<Player>().tapGrav = looperTapGravity;
        transform.parent.GetComponent<Player>().maxSlope = looperMaxSlope;

        /*
        Looper Stats setup
        */

        shadowArray = new Vector3[4];

        
        parentObject = this.gameObject.transform.parent.gameObject;
        attackBox = this.gameObject.GetComponent<BoxCollider>() as BoxCollider;
       

    }

    // Update is called once per frame
    void Update()
    {
        /*
        Currently the swap mechanic will work as such. The player will go to change positions but will place a marker which will
        change the current shadow position to the next in line then the player will swap places witht he next shadow
        making this a super duper hard mechanic to use, but will be really fun a rewarding when you get it right
        */
        
        //Make a timer and put the placeing marker function on a timer for like four seconds
        if (Input.GetKeyDown(KeyCode.P) /*&& markerPlacementTimer == 0.0f*/)
        {
            if (specialActiveTimer > 0.0f && buttonTaps == 1)
            {
                swapWithShadow();
            }
            else
            {
                specialActiveTimer = 1.0f;
                buttonTaps = 1;

                looperFunctionality();
                markerPlacementTimer = 4.0f;
            }
        }
        doubleTap();
        resetTimer();
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

    void swapWithShadow()
    {
        if (shadow == true)
        {
            Vector3 newShadowPosition = transform.parent.transform.position;
            transform.parent.transform.position = shadowArray[0];

            shadowArray[0] = newShadowPosition;
            shadowObj.transform.position = shadowArray[0];
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
        }


        
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
        if (Input.GetButtonDown("Attack"))
        {
            Vector3 dir = parentObject.GetComponent<Player>().speed;

            // 1 is right 0 is left
            if (dir.x > 0)
            {
                //parentObject.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                attackBox.enabled = true;
            }
            else
            {
                //parentObject.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
                attackBox.enabled = true;
            }

            attackBox.enabled = false;
        }
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
}

