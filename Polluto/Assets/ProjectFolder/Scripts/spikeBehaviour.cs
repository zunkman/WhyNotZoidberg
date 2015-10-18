using UnityEngine;
using System.Collections;

public class spikeBehaviour : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.gameObject.transform.position, -Vector3.up, out hit))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                StartCoroutine(destroyAfter(3.0f));
            }
        }
	}

    void OnCollisionEnter(Collision CollisionEnter)
    {
        if (CollisionEnter.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }


    IEnumerator destroyAfter(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
        
    }
}
