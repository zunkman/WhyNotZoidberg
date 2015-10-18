using UnityEngine;
using System.Collections;

public class DestructableBlocks : MonoBehaviour
{
    private bool isVisible;
    [SerializeField]private float timeToKeep;
	// Use this for initialization
	void Start ()
    {
        isVisible = true;	
	}
	
	// Update is called once per frame
	void Update ()
    {
        regenTimer();
	}

    void regenTimer()
    {
        if (isVisible == false) 
        {
            countDown();
        }
        else
        {
            regenerateBlock();
        }

    }

    void countDown()
    {
        float regenTime = 5.0f;
        timeToKeep += Time.deltaTime * 1.0f;

        if(timeToKeep >= regenTime)
        {
            isVisible = true;
            timeToKeep = 0.0f;
        }

    }

    void regenerateBlock()
    {
        //play animation here when one is made
        this.gameObject.GetComponent<BoxCollider>().enabled = true;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        this.gameObject.transform.parent.GetComponent<BoxCollider>().enabled = true;
    }



    void OnTriggerEnter(Collider triggerEnter)
    {
        if (triggerEnter.gameObject.tag == "Attack")
        {
            isVisible = false;
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            this.gameObject.transform.parent.GetComponent<BoxCollider>().enabled = false;
        }
    }

    void OnTriggerStay(Collider triggerEnter)
    {
        if (triggerEnter.gameObject.tag == "Player")
        {
            timeToKeep = 0.0f;
        }
    }

}
