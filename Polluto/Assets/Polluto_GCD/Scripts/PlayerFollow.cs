using UnityEngine;
using System.Collections;

public class PlayerFollow : MonoBehaviour {

    GameObject player;
    Vector3 offset;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        offset = Vector3.Lerp(offset,Vector3.zero, 20f * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, player.transform.position, 1.0f * Time.deltaTime);
            transform.position = new Vector3(transform.position.x,transform.position.y,-20f)+offset;
            GetComponent<Camera>().orthographicSize = Screen.height / 128f;
	}

    public void setOffset(Vector3 newOffset) {
        offset = newOffset;
    }
}
