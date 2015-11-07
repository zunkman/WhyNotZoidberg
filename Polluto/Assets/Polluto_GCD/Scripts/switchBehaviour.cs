using UnityEngine;
using System.Collections;

public class switchBehaviour : MonoBehaviour
{
    [SerializeField]private GameObject gameHandler;
    [SerializeField]private LevelScript_Subterfuge subterfugeScript;
	// Use this for initialization
	void Start ()
    {
        
        if (subterfugeScript == null)
        {
            subterfugeScript = FindObjectOfType<LevelScript_Subterfuge>();
        }
	}

    void OnTriggerEnter(Collider colliderExit)
    {
        if (colliderExit.gameObject.tag == "Attack")
        {
            subterfugeScript.switchesOn += 1;
            subterfugeScript.subterfugeMissionUpdater();
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }


}
