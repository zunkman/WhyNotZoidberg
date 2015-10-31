using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	
	[SerializeField] private Transform cameraTarget = null;
	private Vector3 setSpot;
	
	
	// Use this for initialization
	void Awake () {
		setSpot = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(cameraTarget != null) {
			setSpot.x = cameraTarget.position.x;
			setSpot.y = cameraTarget.position.y;
			this.transform.position = setSpot;
		}
	}
}
