using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject playerOne, playerTwo, gameHandler;
    [SerializeField] private Text playerOneName, playerTwoName, lives, missionObjective;
    [SerializeField] private Image playerOneBasic, playerTwoBasic, playerOneSpecial, playerTwoSpecial, playerOneHealth, playerTwoHealth, playerOneEmpty, playerTwoEmpty;
    [SerializeField] private float objectiveShowTime;

    private bool twoPlayers, pauseOn;
    

    // Use this for initialization
    void Start ()
    {
        gameHandler = GameObject.FindGameObjectWithTag("gameHandler");
        pauseOn = false;

        if (gameHandler.GetComponent<GameHandler>().getNumPlayers() == 2)
        {
            twoPlayers = true;
        }

        playerOne = gameHandler.GetComponent<GameHandler>().getPlayerOne();

        if (twoPlayers)
        {
            playerTwo = gameHandler.GetComponent<GameHandler>().getPlayerTwo();
        }

        playerOneBasic.type = Image.Type.Filled;
        playerOneBasic.fillMethod = Image.FillMethod.Vertical;
        playerTwoBasic.type = Image.Type.Filled;
        playerTwoBasic.fillMethod = Image.FillMethod.Vertical;

        playerOneHealth.type = Image.Type.Filled;
        playerOneHealth.fillMethod = Image.FillMethod.Horizontal;
        playerOneHealth.fillOrigin = (int) Image.OriginHorizontal.Left;
        playerTwoHealth.type = Image.Type.Filled;
        playerTwoHealth.fillMethod = Image.FillMethod.Horizontal;
        playerTwoHealth.fillOrigin = (int)Image.OriginHorizontal.Right;

        playerOneEmpty.type = Image.Type.Filled;
        playerOneEmpty.fillMethod = Image.FillMethod.Horizontal;
        playerOneEmpty.fillOrigin = (int)Image.OriginHorizontal.Left;
        playerTwoEmpty.type = Image.Type.Filled;
        playerTwoEmpty.fillMethod = Image.FillMethod.Horizontal;
        playerTwoEmpty.fillOrigin = (int)Image.OriginHorizontal.Right;
        playerOneEmpty.fillAmount = 1;
        playerTwoEmpty.fillAmount = 1; 

        playerOneSpecial.type = Image.Type.Filled;
        playerOneSpecial.fillMethod = Image.FillMethod.Vertical;
        playerTwoSpecial.type = Image.Type.Filled;
        playerTwoSpecial.fillMethod = Image.FillMethod.Vertical;

        if (!twoPlayers)
        {
            playerTwoName.gameObject.SetActive(false);
            playerTwoHealth.gameObject.SetActive(false);
            playerTwoBasic.gameObject.SetActive(false);
            playerTwoSpecial.gameObject.SetActive(false);
            playerTwoEmpty.gameObject.SetActive(false);
        }

        /*Turn this back on possibly*/
        missionObjective.gameObject.SetActive(false);

        setCharacters();
    }
	
	// Update is called once per frame
	void Update ()
    {
        updateHealth();
        updateLives();
        pauseObjective();

        if (playerOne.GetComponentInParent<Player>().health <= 0 && gameHandler.GetComponent<GameHandler>().totalLives <= 0)
        {
            killCharacter(1);
        }

        if (twoPlayers && playerTwo.GetComponentInParent<Player>().health <= 0 && gameHandler.GetComponent<GameHandler>().totalLives <= 0)
        {
            killCharacter(2);
        }
    }

    void setCharacters ()
    {
        playerOneName.text = GameHandler.selectedCharacterNamePlayerOne;

        if (twoPlayers)
        {
            playerTwoName.text = GameHandler.selectedCharacterNamePlayerTwo;
        }
    }

    void killCharacter (int playerToDelete)
    {
        if (playerToDelete == 1)
        {
            playerOneName.gameObject.SetActive(false);
            playerOneHealth.gameObject.SetActive(false);
            playerOneBasic.gameObject.SetActive(false);
            playerOneSpecial.gameObject.SetActive(false);
            playerOneEmpty.gameObject.SetActive(false);
        }

        else
        {
            playerTwoName.gameObject.SetActive(false);
            playerTwoHealth.gameObject.SetActive(false);
            playerTwoBasic.gameObject.SetActive(false);
            playerTwoSpecial.gameObject.SetActive(false);
            playerTwoEmpty.gameObject.SetActive(false);
        }
    }

    void updateHealth ()
    {
        playerOneHealth.fillAmount = playerOne.GetComponentInParent<Player>().health / playerOne.GetComponentInParent<Player>().baseHealth;
        if (twoPlayers)
        {
            playerTwoHealth.fillAmount = playerTwo.GetComponentInParent<Player>().health / playerTwo.GetComponentInParent<Player>().baseHealth;
        }
    }
    void updateLives ()
    {
        lives.text = "Lives: " + gameHandler.GetComponent<GameHandler>().totalLives;
    }

    void pauseObjective ()
    {
        if (gameHandler.GetComponent<GameHandler>().stopGame && !missionObjective.gameObject.activeSelf)
        {
            missionObjective.gameObject.SetActive(true);
            pauseOn = true;
        }

        if (pauseOn && !gameHandler.GetComponent<GameHandler>().stopGame)
        {
            pauseOn = false;
            missionObjective.gameObject.SetActive(false);
        }
    }
    // This function will allow other scripts to update various text fields
    public void updateObjective (string inputText)
    {
        StartCoroutine(objectiveTimer());
        missionObjective.text = inputText;
    }

    public void startBasicCooldown(int playerNumber, float inputCoolDown)
    {
        if (playerNumber == 1)
        {
            StartCoroutine(cooldown(inputCoolDown, playerOneBasic));
        }

        else
        {
            StartCoroutine(cooldown(inputCoolDown, playerTwoBasic));
        }
    }

    public void startSpecialCooldown(int playerNumber, float inputCoolDown)
    {
        if (playerNumber == 1)
        {
            StartCoroutine(cooldown(inputCoolDown, playerOneSpecial));
        }

        else
        {
            StartCoroutine(cooldown(inputCoolDown, playerTwoSpecial));
        }
    }

    IEnumerator cooldown (float cooldown, Image icon)
    {
        float ourCooldown = 0.0f;

        do
        {
            ourCooldown += Time.deltaTime;

            if (ourCooldown/cooldown > 1.0f)
            {
                ourCooldown = cooldown;
            }

            icon.fillAmount = ourCooldown / cooldown;

            yield return null;
        } while (ourCooldown / cooldown != 1.0f);
    }

    IEnumerator objectiveTimer ()
    {
        float timePassed = 0;
        do
        {
            if (!missionObjective.gameObject.activeSelf)
            {
                missionObjective.gameObject.SetActive(true);
                Debug.Log("On");
            }

            timePassed += Time.deltaTime;
            yield return null;
        } while (timePassed < objectiveShowTime);
        Debug.Log("off");
        missionObjective.gameObject.SetActive(false);
    }
}
