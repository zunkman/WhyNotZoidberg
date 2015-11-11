using UnityEngine;
using System.Collections;

/*
This script will handle bomb behaviour, which consists of bouncing around a bit, blowing shit up, and that's it.
*/

public class Bomb : MonoBehaviour
{
    public GameObject player;


    [SerializeField] private float detonationTimer, damage;
    [SerializeField] private Vector3 leftBlast, rightBlast, upBlast;
    private GameObject[] blastTargets;
    private Rigidbody bombRigid;
    [SerializeField] private GameObject physicalHitbox, explosionHitbox, blastedPlayer;
    [SerializeField] private float additionalGravity;

    // Use this for initialization
    void Start ()
    {
        physicalHitbox = this.transform.FindChild("Bomb Physical Hitbox").gameObject;
        explosionHitbox = this.transform.FindChild("Bomb Blast Radius").gameObject;
        bombRigid = this.gameObject.GetComponent<Rigidbody>();

        explosionHitbox.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        detectPhysicalCollisions();

        handleGravity();

        detonationTimer -= Time.deltaTime;

        if (!explosionHitbox.activeSelf & detonationTimer <= 0.2)
        {
            explosionHitbox.SetActive(true);
        }

        if (detonationTimer <= 0)
        {
            if (explosionHitbox.GetComponent<Blast>().haveTargets)
            {
                blastTargets = new GameObject[explosionHitbox.GetComponent<Blast>().targetNumber];
                blastTargets = explosionHitbox.GetComponent<Blast>().bombTargets;
                explode(explosionHitbox.GetComponent<Blast>().targetNumber);
            }
            
            Destroy(this.gameObject);
        }
	}

    // Handle additional gravity.
    void handleGravity ()
    {
        bombRigid.AddForce(new Vector3(0, -additionalGravity, 0));
    }

    // This will check if we hit a wall or roof.
    void detectPhysicalCollisions()
    {
        RaycastHit hit;
        bool left = false, right = false, above = false;
        bombRigid = this.gameObject.GetComponent<Rigidbody>();
        Vector3 currentVelocity = bombRigid.velocity;

        // Detects if we're hitting a wall or slope to the right.
        if (Physics.Raycast(transform.position, Vector3.right + new Vector3(physicalHitbox.GetComponent<SphereCollider>().radius, 0f, 0f), out hit, 0.5f) && hit.transform.gameObject.tag == "Ground")
        {
            right = true;
        }

        // Detects if we're hitting a wall or slope to the left.
        if (Physics.Raycast(transform.position, Vector3.left - new Vector3(physicalHitbox.GetComponent<SphereCollider>().radius, 0f, 0f), out hit, 0.5f) && hit.transform.gameObject.tag == "Ground")
        {
            left = true;
        }

        // Detects if we're hitting the roof.
        if (Physics.Raycast(transform.position, Vector3.up + new Vector3(0f, physicalHitbox.GetComponent<SphereCollider>().radius, 0f), out hit, 0.5f) && hit.transform.gameObject.tag == "Ground")
        {
            above = true;
        }

        // Calls the appropriate behaviour method, based on what's around us.

       if (above)
        {
            // If we're between two walls or slopes.
            if (right & left)
            {
                bounceRoof(currentVelocity);
            }

            // If we're next to a slope or wall to the right.
            else if (right)
            {
                bounceRoof(currentVelocity);
                bounceLeft(currentVelocity);
            }

            // If we're next to a slope or wall to the left.
            else if (left)
            {
                bounceRoof(currentVelocity);
                bounceRight(currentVelocity);
            }

            // If we just hit the roof
            else
            {
                bounceRoof(currentVelocity);
            }
        }

        // If there's a wall.
        else if (right || left)
        {
            if (right)
            {
                bounceLeft(currentVelocity);
            }

            else
            {
                bounceRight(currentVelocity);
            }
        }

        else
        {

        }
    }

    // This will handle when we hit the roof.
    void bounceRoof(Vector3 currentVelocity)
    {
        currentVelocity.y = - Mathf.Abs(currentVelocity.y);
        bombRigid.velocity = currentVelocity;
    }

    // This will handle when we hit a wall.
    void bounceLeft (Vector3 currentVelocity)
    {
        currentVelocity.x = -Mathf.Abs(currentVelocity.x);
        bombRigid.velocity = currentVelocity;
    }

    void bounceRight(Vector3 currentVelocity)
    {
        currentVelocity.x = +Mathf.Abs(currentVelocity.x);
        bombRigid.velocity = currentVelocity;
    }

    // This will blast all the objects in our range.
    void explode (int targets)
    {
        GameObject zeePlayer;
        for (int x = 0; x < targets; x++)
        {
            if (blastTargets[x].tag == "Enemy" || blastTargets[x].tag == "Ground")
            {
                EnemyDamage hitHP = blastTargets[x].GetComponentInParent<EnemyDamage>();
                if(hitHP != null) hitHP.takeDamage(damage);
            }

            // If it's a player, we do some crazy ass shit. God it's late.
            else
            {
                zeePlayer = Instantiate(blastedPlayer, blastTargets[x].transform.position, Quaternion.identity) as GameObject;
                zeePlayer.GetComponent<BlastedPlayer>().player = blastTargets[x].GetComponentInParent<Player>().gameObject;

                // If the target is to the right.
                if (blastTargets[x].transform.position.x - this.transform.position.x >= explosionHitbox.GetComponent<SphereCollider>().radius / 2)
                {
                    zeePlayer.GetComponent<Rigidbody>().velocity = rightBlast;
                    zeePlayer.GetComponent<BlastedPlayer>().begin();
                }

                // If the target is to the left.
                else if (blastTargets[x].transform.position.x - this.transform.position.x <= -explosionHitbox.GetComponent<SphereCollider>().radius / 2)
                {
                    zeePlayer.GetComponent<Rigidbody>().velocity = leftBlast;
                    zeePlayer.GetComponent<BlastedPlayer>().begin();
                }

                // If the target is on top of the bomb.
                else
                {
                    zeePlayer.GetComponent<Rigidbody>().velocity = upBlast;
                    zeePlayer.GetComponent<BlastedPlayer>().begin();
                }
            }
        }
    }
}
