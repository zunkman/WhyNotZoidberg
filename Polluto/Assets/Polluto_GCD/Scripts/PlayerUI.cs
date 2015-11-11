using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject playerOne, playerTwo, gameHandler;
    private bool twoPlayers;
    [SerializeField] private Text playerOneName, playerTwoName, playerOneHealth, playerTwoHealth;
    [SerializeField] private Image playerOneBasic, playerTwoBasic, playerOneSpecial, playerTwoSpecial;

    
    // Use this for initialization
    void Start ()
    {
        gameHandler = GameObject.FindGameObjectWithTag("gameHandler");

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
        }
    }
	
	// Update is called once per frame
	void Update ()
    {

	}

    // This function will allow other scripts to update various text fields
    public void updateObjective (string inputText)
    {

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
}
