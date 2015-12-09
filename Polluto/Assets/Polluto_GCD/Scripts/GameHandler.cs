using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    private static bool loadOnceGameHandler = false;
    [SerializeField]private bool isOriginal = false;//used with static stuff...
    [SerializeField]public static string selectedCharacterNamePlayerOne = "";
    [SerializeField]public static string selectedCharacterNamePlayerTwo = "";
    [SerializeField]protected static GameObject[] playerCharType = { null, null };

    public static string selectedLevelName = "";
    
    [SerializeField]public static int numPlayers = 0;

    [SerializeField]public int totalLives = 3;

    [SerializeField]private GameObject playerOne;
    [SerializeField]private GameObject playerTwo;

    public GameObject playerSpawnOne;
    //[SerializeField]private GameObject playerSpawnTwo;

    /* All of the player gameobjects */

    [SerializeField]protected GameObject[] characterPrefabs = { null, null, null, null, null };
    [SerializeField]protected static GameObject[] playableCharacters = { null, null, null, null, null };

    //--ref for checking offscreen players--//
    [SerializeField]private Camera sceneCamera;
    //
    [SerializeField]private GameObject pauseMenu;
    [SerializeField]private float offScreenTime;

    private bool isPaused;
    public bool stopGame;
    private bool buttonUp;

    /* Possible remove this later */
    public GameObject targetObject;
    
    [SerializeField] private Button ResumeButton;


    void Awake()
    {
		if(loadOnceGameHandler == false) {
			//Debug.Log ("GH_wasFalse");
            //Assigning non-static data to a static variable for easier access in scripts
            //can't do this in the editor it seems
            for(int i = 0; i < playableCharacters.Length; i++)
            {
                playableCharacters[i] = characterPrefabs[i];
            }
			loadOnceGameHandler = true;
            isOriginal = true;
			DontDestroyOnLoad (this.gameObject);
			DontDestroyOnLoad (this.transform.parent.gameObject);
		} else {
			Debug.Log ("GH_wasTrue");
            //destroy the duplicate if one was already there
            if (this.transform.parent)
            {
                Destroy (this.transform.parent.gameObject);
            }
			Destroy (this.gameObject);
		}
	}
    // Use this for initialization
    void Start()
    {
        //Will make a game handler script that script will be placed in each scene and will 
        //hold the character string and the player selected

        buttonUp = false;

        //for debugging purposes, will load default characters here when running from the editor
        if(playerCharType[0] == null)
        {
            selectCharacter(1, 0);//should set looper as default for player one
            numPlayers = 1;
        }
        OnLevelWasLoaded(0);
    }

    // Update is called once per frame
    void Update () 
    {
        pauseFunction();
        //--offscreen p2 relocate--//
        if(playerTwo != null && playerOne != null) {
            if(sceneCamera != null) {
                Vector3 distVect = playerTwo.transform.position - sceneCamera.transform.position;
                distVect.z = 0;//camera's z might be way off
                if(distVect.magnitude > sceneCamera.orthographicSize * 1.6f) {
                    //give a half second before teleporting, so looper's teleport has time to work first
                    offScreenTime += Time.deltaTime;
                    if(offScreenTime > 0.5f) playerTwo.transform.position = playerOne.transform.position;
                } else {
                    offScreenTime = 0.0f;
                }
            } else {
                sceneCamera = FindObjectOfType<Camera>();
            }
        }
    }

    

    /* Called each frame to check if a player wanted to pause the game */
    void pauseFunction()
    {
        if (Input.GetAxis("pause") >= 0.1f && buttonUp == true && selectedLevelName != null || selectedLevelName == "")
        {
            if (stopGame == false && buttonUp == true)
            {
                stopGame = true;
                isPaused = true;
                pauseGame();
            }
            buttonUp = false;
        }
        else
        {
            buttonUp = true;
        }
    }

    public void respawnPlayer(GameObject playerToRespawn)
    {
        if (totalLives <= 0.0f)
        {
            Application.LoadLevel("MainMenu");
        }
        else
        {
            if (numPlayers == 1)
            {
                playerToRespawn.transform.position = playerSpawnOne.transform.position;
                totalLives -= 1;
                Debug.Log(totalLives);
            }

            if (numPlayers == 2)
            {
                if (playerToRespawn.transform.GetComponent<Player>().playerNumber == 1)
                {
                    playerToRespawn.transform.position = playerTwo.transform.position;
                    totalLives -= 1;
                }

                if (playerToRespawn.transform.GetComponent<Player>().playerNumber == 2)
                {
                    playerToRespawn.transform.position = playerOne.transform.position;
                    totalLives -= 1;
                }
            }
            playerToRespawn.GetComponentInParent<Player>().health = playerToRespawn.GetComponentInParent<Player>().baseHealth;
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

    //This function was too iffy. It has been shortened.
    void spawnPlayers(string characterOneName, string characterTwoName, int numOfChars)
    {
        GameObject spawned;
        for (int i = 0;i< numOfChars; i++) {
            Vector3 offset = new Vector3(i - (numOfChars / 2), 0, 0);
            //checking length of the array before trying to read from it to prevent a potential crash
            if(playerCharType.Length > i) { 
                if(playerCharType[i] != null)
                {
                    spawned = Instantiate(playerCharType[i], playerSpawnOne.transform.position + offset, Quaternion.identity) as GameObject;
                    spawned.GetComponent<Player>().playerNumber = i + 1;//may have issues with inconsistent character design...
                    if(i==0) playerOne = spawned;
                    else playerTwo = spawned;
                }
            }
        }
    }
    
    //--Function added by CT to work with modified Main Menu--//
    public static void selectCharacter(int playerNum, int charID)
    {
        string charName = "";//may be better just fetch the name of the instantiated player object...
        //Looper, Higgs, JunkToss, TankGirl, GlassCannonMan
        switch (charID)
        {
            case 0: charName = "Looper";    break;
            case 1: charName = "Higgs";    break;
            case 2: charName = "JunkToss";    break;
            case 3: charName = "TankGirl";    break;
            case 4: charName = "GlassCannonMan";    break;
        }
        if (charID < 0 || charID >= playableCharacters.Length) charID = 0;
        //playerNum should be 1 or 2
        if(playerNum == 1)selectedCharacterNamePlayerOne = charName;//may be obsolete
        if(playerNum == 2)selectedCharacterNamePlayerTwo = charName;//may be obsolete
        if(playerNum >= 1 && playerNum <= 2)
        {
            int i = playerNum - 1;
            if (playableCharacters[charID] != null) {
                //Debug.Log("Set Player " + i + " to class#" + charID + ".");
                playerCharType[i] = playableCharacters[charID]; }
                else { playerCharType[i] = playableCharacters[0]; }
        }
        //there is no check to make sure the input was valid
        //if an invalid character name was used then the default character should be spawned
    }
    //apparently called when a level was loaded...
    void OnLevelWasLoaded(int level)
    {
        if (isOriginal)//only runs if it's the first GameHandler, since it's supposed to be static.. ish
        {
            //this may break if more than one camera is in the scene
            sceneCamera = FindObjectOfType<Camera>();
            //for debugging purposes, will load default characters here when running from the editor
            if (playerCharType[0] == null)
            {
                selectCharacter(1, 0);//should set looper as default for player one
                numPlayers = 1;
            }

            if (pauseMenu == null)
            {
                pauseMenu = GameObject.FindGameObjectWithTag("pauseMenu");
            }

            if (targetObject == null)
            {
                targetObject = GameObject.FindGameObjectWithTag("target");
            }

            if (pauseMenu != null)
            {
                pauseMenu.SetActive(false);
                isPaused = false;
                stopGame = false;
            }

            playerSpawnOne = GameObject.FindGameObjectWithTag("firstPlayerSpawn");
            if (playerSpawnOne != null)
            {
                if(selectedLevelName == null || selectedLevelName == "") selectedLevelName = "Debug";
                Debug.Log("Calling SpawnPlayers for level: " + level);
                spawnPlayers(selectedCharacterNamePlayerOne, selectedCharacterNamePlayerTwo, numPlayers);
            }

            Polluto_SFX.playSound ("levelload");
        }
    }

    public void pauseGame()
    {
        if (isPaused == true)
        {
            pauseMenu.SetActive(true);
            ResumeButton.Select();
            Time.timeScale = 0.0f;  
        }
    }

    public void resume()
    {
        isPaused = false;
        stopGame = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void backToMainMenu()
    {
        Time.timeScale = 1.0f;
        isPaused = false;
        pauseMenu.SetActive(false);
        selectedLevelName = null;
        Application.LoadLevel("MainMenu");
    }
}
