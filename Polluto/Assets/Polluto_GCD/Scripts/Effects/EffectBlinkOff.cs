using UnityEngine;
using System.Collections;

public class EffectBlinkOff : MonoBehaviour {

	[SerializeField] private float duration = 0.15f;
    [SerializeField] private float timer = 0.0f;

	
	// Use this for initialization
	void Start () {
		timer = 0;
	}

    void Update() {
        	    timer += Time.deltaTime;
        if(timer >= duration) {
            timer = 0;
            this.gameObject.SetActive(false);
        }
    }



}
