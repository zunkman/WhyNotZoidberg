using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverText : MonoBehaviour {

     Text theText;
    

	// Use this for initialization
    void Start () {
		Destroy (this.gameObject, Random.Range(3, 12));
        theText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
	    this.transform.position += new Vector3(Random.Range(-16, 17), Random.Range(-16, 17), 0);
        theText.color = new Color32((byte)Random.Range(0, 256), (byte)Random.Range(0, 256), (byte)Random.Range(0, 256), (byte)Random.Range(128, 256));
	}
}
