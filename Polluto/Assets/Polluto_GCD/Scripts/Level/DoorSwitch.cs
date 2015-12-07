using UnityEngine;
using System.Collections;

public class DoorSwitch : MonoBehaviour
{
    public bool active;

	// Use this for initialization
	void Start ()
    {
        active = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerEnter (Collider other)
    {
        if (!active && other.gameObject.tag == "Player")
        {
            activate();
        }
    }
    void activate ()
    {
        active = true;

        this.GetComponentInChildren<Renderer>().material.color = new Color(0, 256, 0);
    }
}
