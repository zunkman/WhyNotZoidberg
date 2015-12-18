using UnityEngine;
using System.Collections;

public class AVD_RenderLayer : MonoBehaviour {

	[SerializeField] private string newLayer = "Foreground";
	
	// Use this for initialization
	void Start () {
		this.GetComponent<Renderer>().sortingLayerName = newLayer;//"Foreground";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
