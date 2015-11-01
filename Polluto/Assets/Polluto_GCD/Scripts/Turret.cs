using UnityEngine;
using System.Collections;

/*
This will handle turret behaviour, including aiming, shooting.
*/
public class Turret : MonoBehaviour
{
    [SerializeField] public float damage, initialFireRate, fireRate, projectileSpeed;
    [SerializeField] public Vector3 focus, defaultFocus;

    private float fireCooldown;
    [SerializeField] private GameObject gun, gunBarrel, projectile, player;
    private bool haveTarget, canFire;

	// Use this for initialization
	void Start ()
    {
        haveTarget = false;
        canFire = false;
        fireCooldown = initialFireRate;
        focus = defaultFocus;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (haveTarget)
        {
            focus = player.transform.position + new Vector3(0, 0f, 0);

            if (canFire)
            {
                fire();
                canFire = false;
                fireCooldown = fireRate;
            }

            else
            {
                fireCooldown -= Time.deltaTime;

                if (fireCooldown <= 0)
                {
                    canFire = true;
                }
            }
        }

        if (this.GetComponentInChildren<EnemyDamage>().health <= 0)
        {
            this.gameObject.SetActive(false);
        }

        aim();
        lineOfSight();
	}

    // This function aims at a point.
    void aim ()
    {
        Quaternion lookRotation;
        Vector3 lookDirection;

        lookDirection = focus - this.transform.position;

        lookRotation = Quaternion.LookRotation(lookDirection);

        gun.transform.rotation = lookRotation;
    }

    // This function will check if we see a player, or if we already see a player, check if we stopped seeing him.
    void lineOfSight ()
    {
        RaycastHit hit;
        if (!haveTarget && Physics.Raycast(gunBarrel.transform.position, gunBarrel.transform.up, out hit, 100) && hit.transform.gameObject.tag == "Player")
        {
            player = hit.transform.gameObject;
            haveTarget = true;
        }

        else if (haveTarget && Physics.Raycast(gunBarrel.transform.position, gunBarrel.transform.up, out hit, 100) && hit.transform.gameObject.tag != "Player" && hit.transform.gameObject.tag == "Ground")
        {
            haveTarget = false;
            focus = defaultFocus;
            canFire = false;
            fireCooldown = initialFireRate;
        }
    }

    // This function will handle firing the gun.
    void fire ()
    {
        Vector3 direction = focus - gunBarrel.transform.position;
        GameObject shot;

        shot = Instantiate(projectile, gunBarrel.transform.position, Quaternion.identity) as GameObject;
        shot.GetComponent<TurretProjectile>().fire(direction, projectileSpeed, damage);
    }
}
