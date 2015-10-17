using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour {
    //Script assumes that this.transform.parent.gameObject is the root object that this is a part of.
    [SerializeField]    private string ownerTag = "untagged";
    [SerializeField]    private GameObject explosionEffect = null;//effect to create when this is destroyed

    void Start()
    {

    }
    void Update()
    {
        //put health regeneration here?
    }

    void selfDestruct()
    {
        //Platformus_SFX.playSound ("bombDeath");
        //Bomb explosion effect... need prefab reference to particles
        if (explosionEffect != null) Instantiate(explosionEffect, this.transform.position, Quaternion.identity);
        Destroy(this.transform.parent.gameObject);
    }
    //2D collision //disabled
    //3D collision
    void ActiveCollision(Collider other)
    {
        if (other.gameObject.tag != ownerTag)
        {
            switch (other.gameObject.tag)
            {
                case "Bullet":
                    //Debug.Log("Destructible Collided: " + other.gameObject.tag + " " + other.transform.position + " " + other.gameObject.transform.position);
                    selfDestruct();
                    break;
                case "Enemy":
                    selfDestruct();
                    break;
                case "Wall":
                case "ground":
                    //don't want ground destroying itself yet
                    //selfDestruct();
                    break;
                default:
                    //selfDestruct();
                    break;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("!1");
        ActiveCollision(other);
    }
    void OnTriggerStay(Collider other)
    {
        //Debug.Log("!2");
        ActiveCollision(other);
    }
    void OnCollisionEnter(Collision other)
    {
        //Debug.Log("!3");
        ActiveCollision(other.collider);
    }
    void OnCollisionStay(Collision other)
    {
        //Debug.Log("!4");
        ActiveCollision(other.collider);
    }

}
