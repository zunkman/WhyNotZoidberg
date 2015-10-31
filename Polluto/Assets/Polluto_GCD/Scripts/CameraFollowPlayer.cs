using UnityEngine;
using System.Collections;

public class CameraFollowPlayer : MonoBehaviour
{
    private GameObject playerTransform;
    private GameObject cameraComponent;
    private Vector3 newPosition;

	// Use this for initialization
	void Start ()
    {
        cameraComponent = this.gameObject;
	}

    // Update is called once per frame
    void Update()
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player");
        }

        newPosition = playerTransform.transform.position;
        newPosition.z += -15.0f;
        cameraComponent.transform.position = newPosition;

    }
}
