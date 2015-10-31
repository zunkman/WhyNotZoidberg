using UnityEngine;
using System.Collections;

public class MovingPlatformScript : MonoBehaviour {
	[SerializeField] private float moveSpeedH = 1f;
	[SerializeField] private float moveSpeedV = 0f;
	[SerializeField] private bool standby = false;
	[SerializeField] private bool destructible = false;
	[SerializeField] private GameObject explosionEffect = null;
	
	private Vector3 startPosition = Vector3.zero;
	
	
	private Vector2 currentVelocity = Vector2.zero;
	
	void Awake() {
		startPosition = this.transform.parent.position;
	}
	
	// Use this for initialization
	void Start () {
		currentVelocity.x = moveSpeedH;
		currentVelocity.y = moveSpeedV;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void FixedUpdate () {
		//this.rigidbody2D.velocity = Vector2.zero;
		if(moveSpeedH == 0 && Mathf.Abs (this.transform.parent.position.x - startPosition.x) > 0.01f) {
			currentVelocity.x = 0.08f * Mathf.Sign (startPosition.x - this.transform.parent.position.x);
		}
		if(moveSpeedV == 0 && Mathf.Abs (this.transform.parent.position.y - startPosition.y) > 0.01f) {
			currentVelocity.y = 0.08f * Mathf.Sign (startPosition.y - this.transform.parent.position.y);
		}
		if(standby == true) { currentVelocity = Vector2.zero; }
		this.transform.parent.GetComponent<Rigidbody2D>().velocity = currentVelocity;
		
	}
	
	void selfDestruct() {
		//Platformus_SFX.playSound ("bombDeath");
		//Bomb explosion effect... need prefab reference to particles
		if(explosionEffect != null) Instantiate (explosionEffect, this.transform.position, Quaternion.identity);
		Destroy (this.transform.parent.gameObject);
	}
	
	void ActiveCollision ( Collision2D other ) {
		if(other.gameObject.tag == "Wall" || other.gameObject.tag == "Ground" || (standby == true && other.gameObject.tag == "Player")) {
			standby = false;
			//Debug.Log ("Platform Colliding: " + other.transform.position + " " + other.gameObject.transform.position);
			if(other.transform.position.x < this.transform.position.x) {
				currentVelocity.x = 1f * Mathf.Abs (moveSpeedH);
			}
			if(other.transform.position.x > this.transform.position.x) {
				currentVelocity.x = -1f * Mathf.Abs (moveSpeedH);
			}
			if(other.transform.position.y < this.transform.position.y) {
				currentVelocity.y = 1f * Mathf.Abs (moveSpeedV);
			}
			if(other.transform.position.y > this.transform.position.y) {
				currentVelocity.y = -1f * Mathf.Abs (moveSpeedV);
			}
		}
		//Platformus_SFX.playSound ("default");
	}
	void ActiveCollision ( Collider2D other ) {
		if(destructible == true && other.gameObject.name.Contains("Fire")) {
			selfDestruct ();
		}
		if(other.gameObject.tag == "Wall" || other.gameObject.tag == "Ground" || (standby == true && other.gameObject.tag == "Player")) {
			standby = false;
			//Debug.Log ("Platform Colliding: " + other.transform.position + " " + other.gameObject.transform.position);
			if(other.transform.position.x < this.transform.position.x) {
				currentVelocity.x = 1f * Mathf.Abs (moveSpeedH);
			}
			if(other.transform.position.x > this.transform.position.x) {
				currentVelocity.x = -1f * Mathf.Abs (moveSpeedH);
			}
			if(other.transform.position.y < this.transform.position.y) {
				currentVelocity.y = 1f * Mathf.Abs (moveSpeedV);
			}
			if(other.transform.position.y > this.transform.position.y) {
				currentVelocity.y = -1f * Mathf.Abs (moveSpeedV);
			}
		}
		//Platformus_SFX.playSound ("bump");
	}
	
	
	void OnCollisionEnter2D (Collision2D other) {
		//Debug.Log ("Platform Collided: " + other.gameObject.tag + " " + other.transform.position + " " + other.gameObject.transform.position);
		//this.rigidbody2D.velocity = Vector2.zero;
		ActiveCollision(other);
	}
	void OnCollisionStay2D (Collision2D other) {
		//Debug.Log ("Platform Colliding: " + other.gameObject.tag + " " + other.transform.position + " " + other.gameObject.transform.position);
		//this.rigidbody2D.velocity = Vector2.zero;
		ActiveCollision(other);
	}
	void OnTriggerEnter2D (Collider2D other) {
		//Debug.Log ("Platform Collided: " + other.gameObject.tag + " " + other.transform.position + " " + other.gameObject.transform.position);
		//this.rigidbody2D.velocity = Vector2.zero;
		ActiveCollision(other);
	}
	void OnTriggerStay2D (Collider2D other) {
		//Debug.Log ("Platform Colliding: " + other.gameObject.tag + " " + other.transform.position + " " + other.gameObject.transform.position);
		//this.rigidbody2D.velocity = Vector2.zero;
		ActiveCollision(other);
	}
}
