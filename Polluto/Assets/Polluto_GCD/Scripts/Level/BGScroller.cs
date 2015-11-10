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
        Vector3 newTransform = sceneCamera.transform.position * (1.0f - 0.01f * bgScrollSpeed);
        newTransform.z = 0;
	    if(sceneCamera != null) transform.position = newTransform;
	}
}
