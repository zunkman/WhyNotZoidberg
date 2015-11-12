using UnityEngine;
using System.Collections;

/*
This script will be used for controlling the Higgs Bombin' character, and handling damage.
*/


public class HiggsBombinController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject UI;
    [SerializeField] private GameObject bomb;
    [SerializeField] private GameObject attackRange;
    public float attackDamage;
    [SerializeField] private float attackCooldown, specialCooldown;
    private GameObject playerContainer;
    private float attackTimer, specialTimer;

    [SerializeField] private Vector3 bombLaunch;

    private bool canAttack, canSpecial;
    public bool facingRight;
    public int playerNumber;

	// Use this for initialization
	void Start ()
    {
        
        player = this.GetComponentInParent<Player>().gameObject;
        attackRange = this.GetComponentInChildren<HiggsAttack>().gameObject;

        attackRange.SetActive(false);
        // This is used for setting the stats in player.
        playerNumber = player.GetComponent<Player>().playerNumber;
        player.GetComponent<Player>().horspeed = 10;
        player.GetComponent<Player>().horaccel = 30;
        player.GetComponent<Player>().jumpspeed = 12;
        player.GetComponent<Player>().gravity = 25;
        player.GetComponent<Player>().width = 1;
        player.GetComponent<Player>().height = 2;
        player.GetComponent<Player>().raycasts = 20;
        player.GetComponent<Player>().tapGrav = 25;
        player.GetComponent<Player>().maxSlope = 47;
        player.GetComponent<Player>().baseHealth = 100;
        player.GetComponent<Player>().health = 100;


        this.transform.position = player.transform.position;
        if (attackDamage == 0)
        {
            attackDamage = 1f;
        }

        if (attackCooldown == 0)
        {
            attackCooldown = 1f;
        }

        if (specialCooldown == 0)
        {
            specialCooldown = 5f;
        }

        if (bombLaunch == new Vector3())
        {
            bombLaunch = new Vector3(10, 10, 0);
        }

        canAttack = true;
        canSpecial = true;
        facingRight = true;

        attackTimer = attackCooldown;
        specialTimer = specialCooldown;

        UI = GameObject.FindGameObjectWithTag("UI");
	}
	
	// Update is called once per frame
	void Update ()
    {
        keepUp();
        playerInput();
        cooldownHandler();
	}

    // This keeps the Higgs Controller game object at the same location as the player object, and gathers information off of what the player script is doing.
    void keepUp()
    {
        this.gameObject.transform.position = player.transform.position;

        // Checks if we're facing left or right.
        if (!facingRight & player.GetComponent<Player>().speed.x >= 0.1f)
        {
            facingRight = true;
        }

        else if (facingRight & player.GetComponent<Player>().speed.x <= -0.1f)
        {
            facingRight = false;
        }
    }

    // This handles the cooldowns of things like basic attacks and special moves
    void cooldownHandler ()
    {
        if (!canAttack)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= 0.5)
            {
                attackRange.SetActive(false);
            }

            if (attackTimer >= attackCooldown)
            {
                attackTimer = attackCooldown;
                canAttack = true;
            }
        }

        if (!canSpecial)
        {
            specialTimer += Time.deltaTime;
            if (specialTimer >= specialCooldown)
            {
                specialTimer = specialCooldown;
                canSpecial = true;
            }
        }

        
    }
    // Gets the input from the player, concerning attacks.
    void playerInput()
    {
        if (playerNumber == 1)
        {
            if (canAttack & Input.GetAxis("Basic Attack") >= 0.1)
            {
                canAttack = false;
                attack();
                if(player.GetComponent<Player>().UIScript) player.GetComponent<Player>().UIScript.startBasicCooldown(playerNumber, attackCooldown);
                //UI.GetComponent<PlayerUI>().startBasicCooldown(playerNumber, attackCooldown);
                attackTimer = 0;
            }

            if (canSpecial & Input.GetAxis("Special Attack") >= 0.1)
            {
                canSpecial = false;
                specialMove();
                if(player.GetComponent<Player>().UIScript) player.GetComponent<Player>().UIScript.startSpecialCooldown(playerNumber, specialCooldown);
                //UI.GetComponent<PlayerUI>().startSpecialCooldown(playerNumber, specialCooldown);
                specialTimer = 0;
            }
        }

        if (playerNumber == 2)
        {
            if (canAttack & Input.GetAxis("Basic Attack 2") >= 0.1)
            {
                canAttack = false;
                attack();
                if(player.GetComponent<Player>().UIScript) player.GetComponent<Player>().UIScript.startBasicCooldown(playerNumber, attackCooldown);
                attackTimer = 0;
            }

            if (canSpecial & Input.GetAxis("Special Attack 2") >= 0.1)
            {
                canSpecial = false;
                specialMove();
                if(player.GetComponent<Player>().UIScript) player.GetComponent<Player>().UIScript.startSpecialCooldown(playerNumber, specialCooldown);
                specialTimer = 0;
            }
        }
        
    }

    // Launch a bomb
    void specialMove ()
    {
        canSpecial = false;
        GameObject spawnedBomb;

       
        // If we're facing right.
        if (facingRight)
        {
            spawnedBomb = Instantiate(bomb, new Vector3(this.gameObject.transform.position.x + 1, this.gameObject.transform.position.y + 1, 0), Quaternion.identity) as GameObject;
            spawnedBomb.GetComponent<Rigidbody>().velocity = bombLaunch;
        }
        
        else
        {
            spawnedBomb = Instantiate(bomb, new Vector3(this.gameObject.transform.position.x - 1, this.gameObject.transform.position.y + 1, 0), Quaternion.identity) as GameObject;
            spawnedBomb.GetComponent<Rigidbody>().velocity = new Vector3(-bombLaunch.x, bombLaunch.y, 0);
        }
    }

    // Handles basic attacks.
    void attack ()
    {
        canAttack = false;
        attackRange.SetActive(true);
    }
}
