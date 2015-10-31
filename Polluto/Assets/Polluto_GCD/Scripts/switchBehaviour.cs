using UnityEngine;
using System.Collections;

public class switchBehaviour : MonoBehaviour
{
    [SerializeField]private GameObject gameHandler;
	// Use this for initialization
	void Start ()
    {
        if (gameHandler == null)
        {
            gameHandler = GameObject.FindGameObjectWithTag("gameHandler");
        }
	}

    void OnTriggerEnter(Collider colliderExit)
    {
        if (colliderExit.gameObject.tag == "Attack")
        {
            gameHandler.GetComponent<GameHandler>().switchesOn += 1;
            gameHandler.GetComponent<GameHandler>().subterfugeMissionUpdater();
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }


}
