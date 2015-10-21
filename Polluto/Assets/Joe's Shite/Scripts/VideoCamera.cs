using UnityEngine;
using System.Collections;

/*
This script will handle camera behaviour, including seeing players and alerting enemies to player locations.
*/
public class VideoCamera : MonoBehaviour
{
    [SerializeField] private GameObject [] enemies;
    [SerializeField] private GameObject visionLeft, visionRight;
    [SerializeField] private int rayNumber;
    [SerializeField] private float turn;
    private Vector3 playerLocation;


	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    // This function will handle the camera scanning the area.
}
