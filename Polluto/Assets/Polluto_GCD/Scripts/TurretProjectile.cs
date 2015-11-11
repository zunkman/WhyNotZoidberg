using UnityEngine;
using System.Collections;

/*
This script will handle turret projectiles, specifically dealing damage.
*/
public class TurretProjectile : MonoBehaviour
{
    [SerializeField] private float ourDamage;

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    // This function will be called by the Turret script when it's time to fire,
    // giving us our direction, speed and damage.
    public void fire(Vector3 direction, float speed, float damage)
    {
        Vector3 travelVelocity;

        direction.Normalize();
        travelVelocity = direction * speed;
        ourDamage = damage;

        this.gameObject.GetComponent<Rigidbody>().velocity = travelVelocity;
    }

    // This will trigger when we collide with something. If it's a player, we'll call the damage function
    // on the player script. If it's a floor, wall or roof, it is destroyed.
    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponentInParent<Player>().health -= ourDamage;
            
            Destroy(this.gameObject);

        }

        else if (other.gameObject.tag == "Ground")
        {
            Destroy(this.gameObject);
            
        }

        else
        {

        }
    }

}
