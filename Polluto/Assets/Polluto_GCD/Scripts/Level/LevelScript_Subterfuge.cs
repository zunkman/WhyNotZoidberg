﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelScript_Subterfuge : MonoBehaviour {
    /* Subterfuges variables */

    private string bossName = "";

    private int switchSelected;
    private int tempHolderVal;

    [SerializeField]private GameObject newSwitchObject;

    [SerializeField]private GameObject gameCanvas;
    [SerializeField]private GameObject missionText;
    private GameObject elevator;

    private GameObject switchMissionStartHitBox;

    public Vector3[] switchArray;
    public GameObject[] switchCompasTargets;

    public int switchesOn;

    public bool switchMission;
    public bool bossMission;
    public bool endMission;

    private bool isBoss;
    private bool alreadyGenerated;
    //--moved from GameHandler--//
    [SerializeField]private GameObject endLevelDoor;
    [SerializeField]private GameObject playerOneInstatiatedObject;
    [SerializeField]private GameObject playerTwoInstatiatedObject;
    [SerializeField]private GameObject looperInstantiatedObject;
    //--duplicate from GameHandler--//
    [SerializeField]private GameObject playerSpawnOne;
    //--Reference to GameHandler--//
    [SerializeField]private GameHandler gameHandlerScript;

	// Use this for initialization
	void Start () {
        gameHandlerScript = FindObjectOfType<GameHandler>();
        
        playerSpawnOne = GameObject.FindGameObjectWithTag("firstPlayerSpawn");
	    gameCanvas = GameObject.FindGameObjectWithTag("UI");
        missionText = GameObject.FindGameObjectWithTag("MissionText");
        newSwitchObject = GameObject.FindGameObjectWithTag("Switch");
        switchMissionStartHitBox = GameObject.FindGameObjectWithTag("beginSwitchMission");
        elevator = GameObject.FindGameObjectWithTag("Elevator");

        if(endLevelDoor == null)
        {
            endLevelDoor = GameObject.FindGameObjectWithTag("endLevel");
        }

        if(endLevelDoor != null)
        {
            endLevelDoor.SetActive(false);
        }

        switchCompasTargets = new GameObject[2];

        /* Move to subterfuge mission script later */
        /* subterfuge start speficiers */
        //if (selectedLevelName == "Subterfuge")
        {
            switchMission = false;
            switchArray = new Vector3[7];
            for (int i = 0; i < switchArray.Length; i++)
            {
                setSwitchLocation(i);
            }

            subterfugeMissionStart();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /* Move to subterfuge mission script later */
    //void addSpecialCollisionScript()
    //{
    //    if(numPlayers == 1)
    //    {
    //        playerOneInstatiatedObject.AddComponent<subterfugeSpecialCollision>();
    //    }
    //    else if (numPlayers == 2)
    //    {
    //        playerOneInstatiatedObject.AddComponent<subterfugeSpecialCollision>();
    //        playerTwoInstatiatedObject.AddComponent<subterfugeSpecialCollision>();
    //    }
    //}

    /* Move to subterfuge mission script later */
    void subterfugeMissionStart()
    {
        string newMission = "Find control room";
        setMissionText(newMission);
        StartCoroutine(missionTextClear(3.0f));
        //add a special collision component to each character for only this level
        
    }

    /* Move to subterfuge mission script later */
    public void subterfugeMissionUpdater()
    {
        if (switchMission == true)
        {
            if (alreadyGenerated == false)
            {
                subterfugeNewMission();
                playerSpawnOne.transform.position = new Vector3(25.0f, -148.5f, 0.0f);
            }

            if (switchesOn == 2)
            {
                bossMission = true;
                switchMissionStartHitBox.SetActive(false);
            }
            else
            {
                if(switchesOn == 0)
                {
                    if(gameHandlerScript != null)gameHandlerScript.setNewCompassTarget(switchCompasTargets[0]);
                }
                else
                {
                    if(gameHandlerScript != null)gameHandlerScript.setNewCompassTarget(switchCompasTargets[1]);
                }

                missionText.GetComponent<Text>().text = "Find power breakers and turn them on [" + switchesOn + "/2]";
                StartCoroutine(missionTextClear(3.0f));
            }
        }

        if (bossMission == true)
        {
            if (isBoss == false)
            {
                endMission = true;
            }
            else
            {
                string newMission = "Kill " + bossName;
                setMissionText(newMission);
                subterfugeNewMission();
                StartCoroutine(missionTextClear(3.0f));

            }
        }

        if (endMission == true)
        {
            string newMission = "Escape!";
            endLevelDoor.SetActive(true);
            if(gameHandlerScript != null)gameHandlerScript.setNewCompassTarget(endLevelDoor);
            setMissionText(newMission);
            StartCoroutine(missionTextClear(3.0f));
        }
    }

    /* Move to subterfuge mission script later */
    public void subterfugeNewMission()
    {
        //possibly use this as compas setter function
        if (switchMission == true)
        {
            checkForRepeatData();
        }
        else if (bossMission == true)
        {
            //in here dont need to do anything I think
        }
        else if (endMission == true)
        {
            
            //in here I think there will be an escape timer if not then whatever
        }
    }

    /* Move to subterfuge mission script later */
    void checkForRepeatData()
    {
        if (alreadyGenerated == false)
        {
            bool generateSwitches = true;
            int switchCount = 0;
            tempHolderVal = 7;
            while (generateSwitches == true)
            {
                int switchToMake = Random.Range(0, 6);

                if (switchToMake == tempHolderVal)
                {
                    Debug.Log("Duplicated data");
                }
                else
                {
                    GameObject switchInstance = Instantiate(newSwitchObject, switchArray[switchToMake], Quaternion.identity) as GameObject;
                    switchCount += 1;
                    switchCompasTargets[switchCount - 1] = switchInstance;
                    tempHolderVal = switchToMake;
                }

                if (switchCount >= 2)
                {
                    generateSwitches = false;
                }
            }
        }
        alreadyGenerated = true;
    }

    /* Move to subterfuge mission script later */
    void setMissionText(string textToShow)
    {
        missionText.GetComponent<Text>().text = textToShow;
    }

    IEnumerator missionTextClear(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        setMissionText(" ");
    }

        void setSwitchLocation(int powerSwitchNum)
    {
        switch (powerSwitchNum)
        {
            case 0:
                switchArray[powerSwitchNum] = new Vector3(80.0f, -57.5f, 0.0f);
                break;
            case 1:
                switchArray[powerSwitchNum] = new Vector3(5.5f, -86.0f, 0.0f);
                break;
            case 2:
                switchArray[powerSwitchNum] = new Vector3(43.0f, -108.0f, 0.0f);
                break;
            case 3:
                switchArray[powerSwitchNum] = new Vector3(58.5f, -210.5f, 0.0f);
                break;
            case 4:
                switchArray[powerSwitchNum] = new Vector3(44.5f, -267.5f, 0.0f);
                break;
            case 5:
                switchArray[powerSwitchNum] = new Vector3(-71.0f, -252.0f, 0.0f);
                break;
            case 6:
                switchArray[powerSwitchNum] = new Vector3(-138.0f, -98.0f, 0.0f);
                break;
        }

    }
}
