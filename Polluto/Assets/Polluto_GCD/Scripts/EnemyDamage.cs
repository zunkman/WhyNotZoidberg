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
        //--edit by CT to destroy objects with no health left when hit--//
        if(health < 0) {
            //modify later to call a function if more than simply destroying is needed
            //if (this.transform.parent) { Destroy(this.transform.parent.gameObject); } else { Destroy(this); }
            
        }
        //----//
    }
}
