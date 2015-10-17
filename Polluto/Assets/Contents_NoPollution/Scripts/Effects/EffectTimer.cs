using UnityEngine;
using System.Collections;

public class EffectTimer : MonoBehaviour {
	
	[SerializeField] private float duration = 1f;
	
	// Use this for initialization
	void Start () {
		Destroy (this.gameObject, duration);
	}
}
