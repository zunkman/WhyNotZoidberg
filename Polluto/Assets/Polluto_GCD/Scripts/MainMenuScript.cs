using UnityEngine;
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


    [SerializeField] private Button StartButton;
    [SerializeField] private Button CreditsBackButton;
    [SerializeField] private Button OptionsBackButton;
    [SerializeField] private Button OnePlayerButton;
    [SerializeField] private Button LevelOneButton;
    [SerializeField] private Button CharacterDefaultButton;

    void Start()
    {
        StartButton.Select();
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
        cameraMoveSpeed = 500.0f;//Changed 40 to 4000. It was the easiest way to fix waits between menu screens.

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
        Polluto_SFX.playSound("Button");
            moveCameraTierTwo = true;
        creditsCanvas.SetActive(true);
        CreditsBackButton.Select();
    }

    public void OnClickButtonOptions()
    {
        Polluto_SFX.playSound("Button");
        moveCameraTierTwo = true;
        optionsCanvas.SetActive(true);
        OptionsBackButton.Select();
    }

    public void OnClickButtonCharacter()
    {
        Polluto_SFX.playSound("Button");
        moveCameraTierTwo = true;
        characterCanvas.SetActive(true);
        OnePlayerButton.Select();
    }

    public void OnClickBackToMainMenu()
    {
        Polluto_SFX.playSound("Button");
        creditsCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
        characterSelectCanvas.SetActive(false);
        characterCanvas.SetActive(false);
        OnePlayerButton.Select();
        moveCameraTierOne = true;
    }

    public void OnClickBackToCharacterSelect()
    {
        Polluto_SFX.playSound("Button");
        characterSelectCanvas.SetActive(true);
        levelSelectCanvas.SetActive(false);
        CharacterDefaultButton.Select();
        moveCameraTierTwoNeg = true;
    }


    /* Sets the camera tier to move to two if clicked */
    public void OnClickButtonSinglePlayer()
    {
        Polluto_SFX.playSound("Button");
        characterSelectTitle.GetComponent<Text>().text = "Select your character player one";
        characterSelectCanvas.SetActive(true);
        characterCanvas.SetActive(false);
        CharacterDefaultButton.Select();
        GameHandler.numPlayers = 1;
        currentSelectingPlayer = 1;
    }

    public void OnClickButtonTwoPlayer()
    {
        Polluto_SFX.playSound("Button");
        characterSelectTitle.GetComponent<Text>().text = "Select your character player one";
        characterSelectCanvas.SetActive(true);
        characterCanvas.SetActive(false);
        CharacterDefaultButton.Select();
        GameHandler.numPlayers = 2;
        currentSelectingPlayer = 1;
    }

    public void backToCharacterCanvas()
    {
        Polluto_SFX.playSound("Button");
        characterSelectCanvas.SetActive(false);
        characterCanvas.SetActive(true);
        OnePlayerButton.Select();
        playerOneOk.SetActive(false);
        playerTwoOk.SetActive(false);
        GameHandler.numPlayers = 0;
    }

    public void playerOneOkButton()
    {
        Polluto_SFX.playSound("Button");
        if (GameHandler.numPlayers == 1)
        {
            levelSelectCanvas.SetActive(true);
            LevelOneButton.Select();
            moveCameraTierThree = true;
        }
        else if (GameHandler.numPlayers == 2)
        {
            characterSelectTitle.GetComponent<Text>().text = "Select your character player two";
            currentSelectingPlayer = 2;
            CharacterDefaultButton.Select();
            if (currentSelectingPlayer == 2)
            {
                playerOneOk.SetActive(false);
                //playerTwoOk.SetActive(true);
            }
            //playerTwoOk.SetActive(true);
            //make player one ok button disappear last
            //make player two ok button true
        }
    }

    public void playerTwoOkButton()
    {
        Polluto_SFX.playSound("Button");
        playerOneOk.SetActive(false);
        playerTwoOk.SetActive(false);
        levelSelectCanvas.SetActive(true);
        LevelOneButton.Select();
        moveCameraTierThree = true;
    }

    public void clickOnCharacter(int charID)
    {
        Polluto_SFX.playSound("Button");
        //Looper, Higgs, JunkToss, TankGirl, GlassCannonMan
        if(currentSelectingPlayer >= 0)
        {
            GameHandler.selectCharacter(currentSelectingPlayer, charID);
            if (currentSelectingPlayer == 1) playerOneOk.SetActive(true);
            if (currentSelectingPlayer == 2) playerTwoOk.SetActive(true);
        }
    }

    public void onClickSubterfuge()
    {
        Polluto_SFX.playSound("Button");
        GameHandler.selectedLevelName = "Subterfuge";
        Application.LoadLevel("Level4");
    }

    public void onClickMountainGoat()
    {
        Polluto_SFX.playSound("Button");
        GameHandler.selectedLevelName = "  ";
        Application.LoadLevel("MountainBox");
    }

    public void onClickFortKnox()
    {
        Polluto_SFX.playSound("Button");
        GameHandler.selectedLevelName = "Fort Knox";
        Application.LoadLevel("Fort Knox");
    }

    public void onClickVolcanoParadise()
    {
        Polluto_SFX.playSound("Button");
        GameHandler.selectedLevelName = "VolcanoParadise";
        Application.LoadLevel("VolcanoParadise");
    }

    public void onClickTutorial()
    {
        Polluto_SFX.playSound("Button");
		GameHandler.selectedLevelName = "VolcanoParadise";
		Application.LoadLevel("VolcanoParadise");
    }


}
