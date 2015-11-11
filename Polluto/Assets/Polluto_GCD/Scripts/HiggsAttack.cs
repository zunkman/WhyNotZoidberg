using UnityEngine;
using System.Collections;

public class HiggsAttack : MonoBehaviour
{
    private float damage;
	// Use this for initialization
	void Start ()
    {
        damage = this.GetComponentInParent<HiggsBombinController>().attackDamage;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerEnter (Collider other)
    {
        //if (other.gameObject.tag == "Enemy")
        {
            EnemyDamage hitHP = other.gameObject.GetComponentInParent<EnemyDamage>();
            if(hitHP != null) hitHP.takeDamage(damage);
        }
    }
}
