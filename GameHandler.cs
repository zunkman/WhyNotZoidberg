using UnityEngine;
using System.Collections;

public class GameHandler : MonoBehaviour {

    [SerializeField] public string selectedCharacterNamePlayerOne = "";
    [SerializeField] public string selectedCharacterNamePlayerTwo = "";

    [SerializeField] private int numPlayers = 0;

    ///static private bool isPlayerOne = false;

    [SerializeField]private GameObject playerOne;
    [SerializeField]private GameObject playerTwo;

    [SerializeField] private GameObject playerSpawnOne;
    [SerializeField]private GameObject playerSpawnTwo;

    [SerializeField]private GameObject looperObject;

    [SerializeField]private GameObject playerOneInstatiatedObject;
    [SerializeField]private GameObject playerTwoInstatiatedObject;
    [SerializeField]private GameObject looperInstantiatedObject;

    // Use this for initialization
    void Start()
    {
        //Will make a game handler script that script will be placed in each scene and will 
        //hold the character string and the player selected

        //this line is where I will get the character string and the player.
        //Remove the lines below when the second player is being implemented
        playerSpawnOne = GameObject.FindGameObjectWithTag("firstPlayerSpawn");
        playerSpawnTwo = GameObject.FindGameObjectWithTag("secondPlayerSpawn");

        numPlayers = 1;
        selectedCharacterNamePlayerOne = "Looper";
        selectedCharacterNamePlayerTwo = "Looper";

        if (numPlayers == 1)
        {
            characterSelect(selectedCharacterNamePlayerOne, selectedCharacterNamePlayerTwo, numPlayers);
        }
        else if (numPlayers == 2)
        {
            characterSelect(selectedCharacterNamePlayerOne, selectedCharacterNamePlayerTwo, numPlayers);
        }
    }

    // Update is called once per frame
    void Update () 
    {
	
	}


    void characterSelect(string characterOneName, string characterTwoName, int numOfChars)
    {
        if (numOfChars == 1)
        {
            playerOneInstatiatedObject = Instantiate(playerOne, playerSpawnOne.transform.position, Quaternion.identity) as GameObject;

            if (characterOneName == "Looper")
            {
                looperObject = Instantiate(looperObject, playerSpawnOne.transform.position, Quaternion.identity) as GameObject;
                looperObject.transform.parent = this.playerOneInstatiatedObject.transform;
            }
        }
        else if (numOfChars == 2)
        {
            playerOneInstatiatedObject = Instantiate(playerOne, playerSpawnOne.transform.position, Quaternion.identity) as GameObject;

            if (characterOneName == "Looper")
            {
                looperObject = Instantiate(looperObject, playerSpawnOne.transform.position, Quaternion.identity) as GameObject;
                looperObject.transform.parent = this.playerOneInstatiatedObject.transform;
            }
            else
            {

            }

            playerTwoInstatiatedObject = Instantiate(playerTwo, playerSpawnTwo.transform.position, Quaternion.identity) as GameObject;

            if (characterTwoName == "Looper")
            {
                looperObject = Instantiate(looperObject, playerSpawnTwo.transform.position, Quaternion.identity) as GameObject;
                looperObject.transform.parent = this.playerTwoInstatiatedObject.transform;
            }
            else
            {

            }
        }
    }
}
