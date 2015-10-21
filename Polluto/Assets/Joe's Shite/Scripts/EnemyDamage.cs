using UnityEngine;
using System.Collections;

/*
This script will handle enemies taking damage.
*/

public class EnemyDamage : MonoBehaviour
{
    public float startHealth, health;
    public bool gotHit, hasHitAction;

	// Use this for initialization
	void Start ()
    {
        health = startHealth;
        gotHit = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    // This function will be called by a player if they damage the enemy.
    public void takeDamage (float damage)
    {
        if (hasHitAction)
        {
            gotHit = true;
        }

        health -= damage;
    }
}
