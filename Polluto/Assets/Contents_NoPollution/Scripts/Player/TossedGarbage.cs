using UnityEngine;
using System.Collections;

public class TossedGarbage : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter (Collider other)
    {
        //Debug.Log("Trigger Overlap.");
        EnemyDamage impactedDamageScript = null;
        if(other.GetComponent<EnemyDamage>())
        {
            //Debug.Log("Has Script.");
            impactedDamageScript = other.GetComponent<EnemyDamage>();
            
        } else if(other.transform.parent)
        {
            //Debug.Log("Has Parent.");
            if (other.transform.parent.GetComponent<EnemyDamage>())
            {
                //Debug.Log("Has Child Script.");
                impactedDamageScript = other.transform.parent.GetComponent<EnemyDamage>();
            } else
            {
                //Debug.Log("No Child script.");
            }
        }
        if (impactedDamageScript)
        {
            impactedDamageScript.takeDamage(1.0f);
            Destroy(this);
        }
        if (other.gameObject.tag == "Ground")
        {
            Destroy(this.gameObject);
            
        }
    }
    
    void OnTriggerStay(Collider other)
    {
        //Debug.Log("Trigger Stay.");
    }
    void OnCollisionEnter(Collision other)
    {
        //Debug.Log("Collider Overlap.");
    }
}
