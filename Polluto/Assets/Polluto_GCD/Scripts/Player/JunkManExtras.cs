using UnityEngine;
using System.Collections;

public class JunkManExtras : MonoBehaviour
{

    [SerializeField]
    protected float[] attacking = { 0.0f, 0.0f };
    [SerializeField]
    protected float[] attackTimers = { 0.0f, 0.0f };
    [SerializeField]
    protected float[] attackCooldowns = { 0.50f, 0.50f };

    [SerializeField]
    protected GameObject spawnable;
    [SerializeField]
    protected float[] spawnableSpeed = { 6.0f, 6.0f };
    [SerializeField]
    protected Player playerScript;


    void Awake()
    {
    }
    void Start()
    {
        if (playerScript == null)
        {
            if (this.transform.parent)
            {
                playerScript = this.transform.parent.gameObject.GetComponent<Player>();
            }
        }
        if(!playerScript) {
            Debug.Log("Could not find player script for this actor! " + this.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!playerScript) return;//may crash without this variable, so skipping the function if it's not set
        if(playerScript.playerNumber == 1) {
            attacking[0] = Mathf.Abs(Input.GetAxis("Basic Attack"));
            attacking[1] = Mathf.Abs(Input.GetAxis("Special Attack"));
        } else {
            attacking[0] = Mathf.Abs(Input.GetAxis("Basic Attack 2"));
            attacking[1] = Mathf.Abs(Input.GetAxis("Special Attack 2"));
        }
        int weapons = attacking.Length;//number of attacks
        for (int i = 0; i < attacking.Length; i++)
        {
            attackTimers[i] += Time.deltaTime;
            if (attacking[i] >= 0.25f && attackTimers[i] >= attackCooldowns[i])
            {
                attackTimers[i] = 0.0f;//reset timer since we're firing... probably
                if (!spawnable) continue;
                GameObject spawned = Instantiate(spawnable, this.transform.position, this.transform.rotation) as GameObject;
                if(playerScript.inputDirection != Vector2.zero) spawned.GetComponent<Rigidbody>().velocity = playerScript.inputDirection * spawnableSpeed[i];
                else spawned.GetComponent<Rigidbody>().velocity = spawned.transform.right * spawnableSpeed[i];//shoot facing if no input vector
                Debug.Log(spawned.transform.forward);
            }
        }

    }
}
