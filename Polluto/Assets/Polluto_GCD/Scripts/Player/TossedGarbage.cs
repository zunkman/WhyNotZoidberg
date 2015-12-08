using UnityEngine;
using System.Collections;

public class TossedGarbage : MonoBehaviour {

    [SerializeField] public bool isWeapon = false;
    [SerializeField] private float baseDamage = 10.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate() {
        this.GetComponent<Rigidbody>().velocity += Physics.gravity * Time.deltaTime * 0.1f;
    }

    void hit(Collider other) {
        
        //only do this if it is considered a weapon.
        if(isWeapon || GetComponent<Rigidbody>().velocity.magnitude > 1.0f) {
            //Debug.Log("Trigger Overlap.");
            EnemyDamage impactedDamageScript = null;
            if(other.GetComponentInChildren<EnemyDamage>())
            {
                //Debug.Log("Has Script.");
                impactedDamageScript = other.GetComponentInChildren<EnemyDamage>();
            
            } else if(other.transform.parent)
            {
                //Debug.Log("Has Parent.");
                if (other.GetComponentInParent<EnemyDamage>())
                {
                    //Debug.Log("Has Child Script.");
                    impactedDamageScript = other.GetComponentInParent<EnemyDamage>();
                } else
                {
                    Debug.Log("No script.");
                }
            }
            if (impactedDamageScript)
            {
                impactedDamageScript.takeDamage(baseDamage + GetComponentInChildren<Rigidbody>().velocity.magnitude);//deals more damage if thrown faster
                isWeapon = false;
                baseDamage *= 0.5f; if(baseDamage < 1.0f) baseDamage = 1.0f;
                //Destroy(this);
                Debug.Log("Hit " + other.gameObject.tag);
            }
            if (other.gameObject.tag == "Ground")
            {
                    Destroy(this.gameObject);
            }
        } else {
            if(!isWeapon)Debug.Log("Not Weapon");
            if(GetComponent<Rigidbody>().velocity.magnitude > 1.0f)Debug.Log("Low Velocity");//:" + GetComponent<Rigidbody>().velocity.magnitude); }
        }
    }
    void OnTriggerEnter (Collider other)
    {
        hit(other);
        
    }
    
    void OnTriggerStay(Collider other)
    {
        //Debug.Log("Trigger Stay.");
    }
    void OnCollisionEnter(Collision other)
    {
        hit(other.collider);
        //Debug.Log("Collider Overlap.");
    }
}
