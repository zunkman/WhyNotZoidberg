using UnityEngine;
using System.Collections;

public class BGScroller : MonoBehaviour {
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private float bgScrollSpeed = 10.0f;
	// Use this for initialization
	void Start () {
	    sceneCamera = FindObjectOfType<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(sceneCamera != null) transform.position = sceneCamera.transform.position * (1.0f - 0.01f * bgScrollSpeed);
	}
}
