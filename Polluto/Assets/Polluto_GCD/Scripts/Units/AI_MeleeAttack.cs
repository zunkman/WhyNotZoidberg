using UnityEngine;
using System.Collections;

public class AI_MeleeAttack : MonoBehaviour {

    [SerializeField] float attackDamage = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Polluto_SFX.playSound ("zap");
            other.gameObject.GetComponentInParent<Player>().health -= attackDamage;
        }

        //damage destructibles?
        if (other.gameObject.tag != "Enemy")
        {
        EnemyDamage impactedDamageScript = null;
            if(other.gameObject.GetComponentInChildren<EnemyDamage>())
            {
                //Debug.Log("Has Script.");
                impactedDamageScript = other.gameObject.GetComponentInChildren<EnemyDamage>();
            
            } else if(other.transform.parent)
            {
                //Debug.Log("Has Parent.");
                if (other.transform.parent.GetComponentInChildren<EnemyDamage>())
                {
                    //Debug.Log("Has Child Script.");
                    impactedDamageScript = other.transform.parent.GetComponentInChildren<EnemyDamage>();
                } else
                {
                    //Debug.Log("No Child script.");
                }
            }
            if (impactedDamageScript)
            {
                impactedDamageScript.takeDamage(attackDamage);//deals more damage if thrown faster
            }
        }
    }
}
