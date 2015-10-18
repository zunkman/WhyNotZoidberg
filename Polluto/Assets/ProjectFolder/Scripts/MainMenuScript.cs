﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    private GameObject mainCamera;
    private GameObject creditsCanvas;
    private GameObject optionsCanvas;
    private GameObject characterSelectCanvas;
    private GameObject characterCanvas;
    private GameObject levelSelectCanvas;
    private GameObject characterSelectTitle;
    [SerializeField]private GameObject playerOneOk;
    [SerializeField]private GameObject playerTwoOk;

    private Vector3 newCameraPosition;
    private Vector3 menuTierOne;
    private Vector3 menuTierTwo;
    private Vector3 menuTierThree;

    private bool moveCameraTierOne;
    private bool moveCameraTierTwo;
    private bool moveCameraTierTwoNeg;
    private bool moveCameraTierThree;
    private bool characterSelection;

    [SerializeField]private int currentSelectingPlayer;

    private float cameraMoveSpeed;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        creditsCanvas = GameObject.FindGameObjectWithTag("creditCanvas");
        optionsCanvas = GameObject.FindGameObjectWithTag("optionsCanvas");
        characterCanvas = GameObject.FindGameObjectWithTag("characterCanvas");
        levelSelectCanvas = GameObject.FindGameObjectWithTag("levelSelectCanvas");
        characterSelectCanvas = GameObject.FindGameObjectWithTag("characterSelectCanvas");
        characterSelectTitle = GameObject.FindGameObjectWithTag("characterSelectTitle");
        playerOneOk = GameObject.FindGameObjectWithTag("playerOneOk");
        playerTwoOk = GameObject.FindGameObjectWithTag("playerTwoOk");

        creditsCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
        characterSelectCanvas.SetActive(false);
        characterCanvas.SetActive(false);
        levelSelectCanvas.SetActive(false);
        playerOneOk.SetActive(false);
        playerTwoOk.SetActive(false);

        newCameraPosition = mainCamera.transform.position;
        cameraMoveSpeed = 40.0f;

        menuTierOne = new Vector3(295.0f, 0.0f, -40.0f);
        menuTierTwo = new Vector3(295.0f, 0.0f, 160.0f);
        menuTierThree = new Vector3(295.0f, 0.0f, 360.0f);

        currentSelectingPlayer = 0;
    }

    void Update()
    {
        checkIfCameraIsMoving();



    }

    /* Constantly checks what tier the camera should be on */
    void checkIfCameraIsMoving()
    {
        if (moveCameraTierOne == true)
        {
           moveCameraTierOne = changeMenuNeg(menuTierOne, moveCameraTierOne);
        }
        else if (moveCameraTierTwo == true)
        {
           moveCameraTierTwo = changeMenuPos(menuTierTwo, moveCameraTierTwo);
        }
        else if (moveCameraTierTwoNeg == true)
        {
            moveCameraTierTwoNeg = changeMenuNeg(menuTierTwo, moveCameraTierTwoNeg);
        }
        else if(moveCameraTierThree == true)
        {
           moveCameraTierThree = changeMenuPos(menuTierThree, moveCameraTierThree);
        }


    }

    /* Moves the camera to its respective tier*/
    bool changeMenuPos(Vector3 cameraStopVector, bool cameraTierToPass)
    {
        if (newCameraPosition.z < cameraStopVector.z)
        {
            newCameraPosition.z += Time.deltaTime * cameraMoveSpeed;
        }

        if (newCameraPosition.z > cameraStopVector.z)
        {
            newCameraPosition.z = cameraStopVector.z;
            return cameraTierToPass = false;
        }
        
        mainCamera.transform.position = newCameraPosition;
        return true;
    }

    bool changeMenuNeg(Vector3 cameraStopVector, bool cameraTierToPass)
    {
        if (newCameraPosition.z > cameraStopVector.z)
        {
            newCameraPosition.z -= Time.deltaTime * cameraMoveSpeed;
        }

        if(newCameraPosition.z < cameraStopVector.z)
        {
            newCameraPosition.z = cameraStopVector.z;
            return cameraTierToPass = false;
        }

        mainCamera.transform.position = newCameraPosition;
        return true;
    }

    /* Sets the camera tier to move to two if clicked */
    public void OnClickButtonCredits()
    {
        moveCameraTierTwo = true;
        creditsCanvas.SetActive(true);
    }

    public void OnClickButtonOptions()
    {
        moveCameraTierTwo = true;
        optionsCanvas.SetActive(true);
    }

    public void OnClickButtonCharacter()
    {
        moveCameraTierTwo = true;
        characterCanvas.SetActive(true);
    }

    public void OnClickBackToMainMenu()
    {
        creditsCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
        characterSelectCanvas.SetActive(false);
        characterCanvas.SetActive(false);
        moveCameraTierOne = true;
    }

    public void OnClickBackToCharacterSelect()
    {
        characterSelectCanvas.SetActive(true);
        levelSelectCanvas.SetActive(false);
        moveCameraTierTwoNeg = true;
    }


    /* Sets the camera tier to move to two if clicked */
    public void OnClickButtonSinglePlayer()
    {
        characterSelectTitle.GetComponent<Text>().text = "Select your character player one";
        characterSelectCanvas.SetActive(true);
        characterCanvas.SetActive(false);
        GameHandler.numPlayers = 1;
        currentSelectingPlayer = 1;
    }

    public void OnClickButtonTwoPlayer()
    {
        characterSelectTitle.GetComponent<Text>().text = "Select your character player one";
        characterSelectCanvas.SetActive(true);
        characterCanvas.SetActive(false);
        GameHandler.numPlayers = 2;
        currentSelectingPlayer = 1;
    }

    public void backToCharacterCanvas()
    {
        characterSelectCanvas.SetActive(false);
        characterCanvas.SetActive(true);
        playerOneOk.SetActive(false);
        playerTwoOk.SetActive(false);
        GameHandler.numPlayers = 0;
    }

    public void playerOneOkButton()
    {
        if (GameHandler.numPlayers == 1)
        {
            levelSelectCanvas.SetActive(true);
            moveCameraTierThree = true;
        }
        else if (GameHandler.numPlayers == 2)
        {
            characterSelectTitle.GetComponent<Text>().text = "Select your character player two";
            playerOneOk.SetActive(false);
            playerTwoOk.SetActive(true);
            //make player one ok button disappear last
            //make player two ok button true
        }
    }

    public void playerTwoOkButton()
    {
        playerTwoOk.SetActive(false);
        levelSelectCanvas.SetActive(true);
        moveCameraTierThree = true;
    }

    public void clickOnLooper()
    {
        if (currentSelectingPlayer == 1)
        {
            GameHandler.selectedCharacterNamePlayerOne = "Looper";
            playerOneOk.SetActive(true);
        }
        else
        {
            GameHandler.selectedCharacterNamePlayerTwo = "Looper";
        }

    }

    public void clickOnHiggs()
    {
        if (currentSelectingPlayer == 1)
        {
            GameHandler.selectedCharacterNamePlayerOne = "Higgs";
            playerOneOk.SetActive(true);
        }
        else
        {
            GameHandler.selectedCharacterNamePlayerTwo = "Higgs";
        }
    }

    public void clickOnJunkToss()
    {
        if (currentSelectingPlayer == 1)
        {
            GameHandler.selectedCharacterNamePlayerOne = "JunkToss";
            playerOneOk.SetActive(true);
        }
        else
        {
            GameHandler.selectedCharacterNamePlayerTwo = "JunkToss";
        }
    }

    public void clickOnTankGirl()
    {
        if (currentSelectingPlayer == 1)
        {
            GameHandler.selectedCharacterNamePlayerOne = "TankGirl";
            playerOneOk.SetActive(true);
        }
        else
        {
            GameHandler.selectedCharacterNamePlayerTwo = "TankGirl";
        }
    }

    public void clickOnGlassCanonMan()
    {
        if (currentSelectingPlayer == 1)
        {
            GameHandler.selectedCharacterNamePlayerOne = "GlassCanonMan";
            playerOneOk.SetActive(true);
        }
        else
        {
            GameHandler.selectedCharacterNamePlayerTwo = "GlassCanonMan";
        }
    }

    public void onClickSubterfuge()
    {
        Application.LoadLevel("Level4");
    }


}
