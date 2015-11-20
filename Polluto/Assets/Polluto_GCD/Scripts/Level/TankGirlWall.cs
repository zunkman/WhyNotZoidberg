using UnityEngine;
using System.Collections;

public class TankGirlWall : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerEnter (Collider other)
    {
        Debug.Log("Something's here.");
        if (other.gameObject.GetComponentInParent<TankGirl>() != null)
        {
            Debug.Log("Die now.");
            Destroy(this.transform.parent.gameObject);
        }
    }
}
