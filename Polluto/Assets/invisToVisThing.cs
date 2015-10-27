using UnityEngine;
using System.Collections;

public class invisToVisThing : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerEnter(Collider colliderEnter)
    {
        if (colliderEnter.gameObject.tag == "Player")
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .5f);
        }
    }

    void OnTriggerExit(Collider colliderExit)
    {
        if (colliderExit.gameObject.tag == "Player")
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1.0f);
        }
    }

}
