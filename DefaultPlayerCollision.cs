using UnityEngine;
using System.Collections;

public class DefaultPlayerCollision : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collisionEnter)
    {
        if(collisionEnter.gameObject.tag == "Floor")
        {
            this.gameObject.GetComponent<DefaultPlayerMovement>().isJumping = false;
        }

    }
}
