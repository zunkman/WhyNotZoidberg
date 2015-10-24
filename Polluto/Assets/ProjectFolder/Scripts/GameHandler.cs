using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{

    [SerializeField]public static string selectedCharacterNamePlayerOne = "";
    [SerializeField]public static string selectedCharacterNamePlayerTwo = "";
    public static string selectedLevelName = "";
    
    [SerializeField]public static int numPlayers = 0;

    [SerializeField]public int totalLives = 3;

    [SerializeField]private GameObject playerOne;
    [SerializeField]private GameObject playerTwo;

    [SerializeField]private GameObject playerSpawnOne;
    [SerializeField]private GameObject playerSpawnTwo;

    /* All of the player gameobjects */
    [SerializeField]private GameObject looperObject;
    [SerializeField]private GameObject junkTossObject;
    [SerializeField]private GameObject tankGirlObject;
    [SerializeField]private GameObject higgsObject;

    [SerializeField]private GameObject playerOneInstatiatedObject;
    [SerializeField]private GameObject playerTwoInstatiatedObject;
    [SerializeField]private GameObject looperInstantiatedObject;

  
    //Something wrong with spawning switches fiX!!!!

    /* Subterfuges variables */

    private string bossName = "";

    private int switchSelected;
    private int tempHolderVal;

    [SerializeField]private GameObject newSwitchObject;

    [SerializeField]private GameObject gameCanvas;
    [SerializeField]private GameObject missionText;

    private GameObject switchMissionStartHitBox;

    public Vector3[] switchArray;

    public int switchesOn;

    public bool switchMission;
    public bool bossMission;
    public bool endMission;

    private bool isBoss;
    private bool alreadyGenerated;


    // Use this for initialization
    void Start()
    {
        //Will make a game handler script that script will be placed in each scene and will 
        //hold the character string and the player selected

        //this line is where I will get the character string and the player.
        //Remove the lines below when the second player is being implemented
        playerSpawnOne = GameObject.FindGameObjectWithTag("firstPlayerSpawn");
        playerSpawnTwo = GameObject.FindGameObjectWithTag("secondPlayerSpawn");

        /* Subterfuge level specific objects */
        
        gameCanvas = GameObject.FindGameObjectWithTag("UI");
        missionText = GameObject.FindGameObjectWithTag("MissionText");
        newSwitchObject = GameObject.FindGameObjectWithTag("Switch");
        switchMissionStartHitBox = GameObject.FindGameObjectWithTag("beginSwitchMission");

        //numPlayers = 2;
        //selectedCharacterNamePlayerOne = "Looper";
        //selectedCharacterNamePlayerTwo = "Looper";
        //selectedLevelName = "Subterfuge";

        if (numPlayers == 1)
        {
            characterSelect(selectedCharacterNamePlayerOne, selectedCharacterNamePlayerTwo, numPlayers);
        }
        else if (numPlayers == 2)
        {
            characterSelect(selectedCharacterNamePlayerOne, selectedCharacterNamePlayerTwo, numPlayers);
        }

        /* subterfuge start speficiers */
        if (selectedLevelName == "Subterfuge")
        {
            switchMission = false;
            switchArray = new Vector3[7];
            for (int i = 0; i < switchArray.Length; i++)
            {
                setSwitchLocation(i);
            }

            subterfugeMissionStart();
            addSpecialCollisionScript();
        }

    }

    // Update is called once per frame
    void Update () 
    {
	}

    void LateUpdate()
    {
       
    }

    public void respawnPlayer(GameObject playerToRespawn)
    {
        if(numPlayers == 2)
        {
            if (playerToRespawn.transform.parent.GetComponent<Player>().playerNumber == 1 /*get the player two health an dif the health is above 0*/)
            { }

        }

        if (totalLives <= 0.0f)
        {
             Debug.Log("LOSERS!");
            Destroy(playerToRespawn.transform.parent);
        }
        else
        {
            if (numPlayers == 1)
            {
                playerToRespawn.transform.parent.transform.position = playerSpawnOne.transform.position;
                totalLives -= 1;
                Debug.Log(totalLives);
            }

            if (numPlayers == 2)
            {
                if (playerToRespawn.transform.parent.GetComponent<Player>().playerNumber == 1)
                {
                    Debug.Log("player one respawn");
                    playerToRespawn.transform.parent.transform.position = playerSpawnOne.transform.position;
                    totalLives -= 1;
                }

                if (playerToRespawn.transform.parent.GetComponent<Player>().playerNumber == 2)
                {
                    Debug.Log("player two respawn");
                    playerToRespawn.transform.parent.transform.position = playerSpawnTwo.transform.position;
                    totalLives -= 1;
                }
            }
        }
    }


    void addSpecialCollisionScript()
    {
        if(numPlayers == 1)
        {
            playerOneInstatiatedObject.AddComponent<subterfugeSpecialCollision>();
        }
        else if (numPlayers == 2)
        {
            playerOneInstatiatedObject.AddComponent<subterfugeSpecialCollision>();
            playerTwoInstatiatedObject.AddComponent<subterfugeSpecialCollision>();
        }
    }

    void subterfugeMissionStart()
    {
        string newMission = "Find control room";
        setMissionText(newMission);
        StartCoroutine(missionTextClear(3.0f));
        //add a special collision component to each character for only this level
        setCompas();
    }
    
    public void subterfugeMissionUpdater()
    {
        if (switchMission == true)
        {
            if (alreadyGenerated == false)
            {
                subterfugeNewMission();
                playerSpawnOne.transform.position = new Vector3(25.0f, -148.5f, 0.0f);
                playerSpawnTwo.transform.position = new Vector3(35.0f, -148.5f, 0.0f);
            }


            if (switchesOn == 2)
            {
                bossMission = true;
                switchMissionStartHitBox.SetActive(false);
            }
            else
            {
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
            setMissionText(newMission);
            StartCoroutine(missionTextClear(3.0f));
        }
    }

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
                    Instantiate(newSwitchObject, switchArray[switchToMake], Quaternion.identity);
                    switchCount += 1;
                    Debug.Log(switchCount);
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


    void setMissionText(string textToShow)
    {
        missionText.GetComponent<Text>().text = textToShow;
    }


    void setCompas()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            //Debug.Log("Show path to mission");
        }

    }


    public GameObject getPlayerOne()
    {
        return playerOne;
    }

    public GameObject getPlayerTwo()
    {
        return playerTwo;
    }

    public int getNumPlayers()
    {
        return numPlayers;
    }


    void characterSelect(string characterOneName, string characterTwoName, int numOfChars)
    {
        if (numOfChars == 1)
        {
            this.playerOneInstatiatedObject = Instantiate(this.playerOne, playerSpawnOne.transform.position, Quaternion.identity) as GameObject;

            if (characterOneName == "Looper")
            {
                looperObject = Instantiate(this.looperObject, playerSpawnOne.transform.position, Quaternion.identity) as GameObject;
                looperObject.transform.parent = this.playerOneInstatiatedObject.transform;
            }
            else if (characterOneName == "JunkToss")
            {
                junkTossObject = Instantiate(junkTossObject, playerSpawnOne.transform.position, Quaternion.identity) as GameObject;
                junkTossObject.transform.parent = this.playerOneInstatiatedObject.transform;
            }
            else if (characterOneName == "Higgs")
            {
                higgsObject = Instantiate(higgsObject, playerSpawnOne.transform.position, Quaternion.identity) as GameObject;
                higgsObject.transform.parent = this.playerOneInstatiatedObject.transform;
            }
            else if (characterOneName == "TankGirl")
            {
                tankGirlObject = Instantiate(tankGirlObject, playerSpawnOne.transform.position, Quaternion.identity) as GameObject;
                tankGirlObject.transform.parent = this.playerOneInstatiatedObject.transform;
            }

            this.playerOneInstatiatedObject.GetComponent<Player>().playerNumber = 1;

        }

        if (numOfChars == 2)
        {
            this.playerOneInstatiatedObject = Instantiate(this.playerOne, playerSpawnOne.transform.position, Quaternion.identity) as GameObject;

            if (characterOneName == "Looper")
            {
                looperObject = Instantiate(looperObject, playerSpawnOne.transform.position, Quaternion.identity) as GameObject;
                looperObject.transform.parent = this.playerOneInstatiatedObject.transform;
            }
            else if (characterOneName == "JunkToss")
            {
                junkTossObject = Instantiate(junkTossObject, playerSpawnOne.transform.position, Quaternion.identity) as GameObject;
                junkTossObject.transform.parent = this.playerOneInstatiatedObject.transform;
            }
            else if (characterOneName == "Higgs")
            {
                higgsObject = Instantiate(higgsObject, playerSpawnOne.transform.position, Quaternion.identity) as GameObject;
                higgsObject.transform.parent = this.playerOneInstatiatedObject.transform;
            }
            else if (characterOneName == "TankGirl")
            {
                tankGirlObject = Instantiate(tankGirlObject, playerSpawnOne.transform.position, Quaternion.identity) as GameObject;
                tankGirlObject.transform.parent = this.playerOneInstatiatedObject.transform;
            }
            else
            {

            }

            this.playerOneInstatiatedObject.GetComponent<Player>().playerNumber = 1;

            this.playerTwoInstatiatedObject = Instantiate(this.playerTwo, playerSpawnTwo.transform.position, Quaternion.identity) as GameObject;

            if (characterTwoName == "Looper")
            {
                looperObject = Instantiate(this.looperObject, playerSpawnTwo.transform.position, Quaternion.identity) as GameObject;
                looperObject.transform.parent = this.playerTwoInstatiatedObject.transform;
            }
            else if (characterTwoName == "JunkToss")
            {
                junkTossObject = Instantiate(junkTossObject, playerSpawnTwo.transform.position, Quaternion.identity) as GameObject;
                junkTossObject.transform.parent = this.playerTwoInstatiatedObject.transform;
            }
            else if (characterTwoName == "Higgs")
            {
                higgsObject = Instantiate(higgsObject, playerSpawnTwo.transform.position, Quaternion.identity) as GameObject;
                higgsObject.transform.parent = this.playerTwoInstatiatedObject.transform;
            }
            else if (characterTwoName == "TankGirl")
            {
                tankGirlObject = Instantiate(tankGirlObject, playerSpawnTwo.transform.position, Quaternion.identity) as GameObject;
                tankGirlObject.transform.parent = this.playerTwoInstatiatedObject.transform;
            }
            else
            {

            }

            this.playerTwoInstatiatedObject.GetComponent<Player>().playerNumber = 2;
        }
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


    IEnumerator missionTextClear(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        setMissionText(" ");
    }

}
