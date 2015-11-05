using UnityEngine;
using System.Collections;

public class DefaultPlayerCollision : MonoBehaviour
{
    private float initialLavaDamage;
    private float spikeDamage;
    private float lavaDamageOverTime;
    private float pushPower;

    void Start()
    {
        initialLavaDamage = 15.0f;
        lavaDamageOverTime = 0.05f;
        spikeDamage = 10.0f;
        pushPower = 500.0f;
    }

    void OnTriggerEnter(Collider collisionEnter)
    {
        if (collisionEnter.gameObject.tag == "Lava")
        {
            this.gameObject.GetComponentInChildren<Player>().health -= initialLavaDamage;
            Vector3 dir = this.gameObject.GetComponentInParent<Player>().speed;
            damageKnockBack(dir.x, pushPower);
        }

        if (collisionEnter.gameObject.tag == "smallBoostBox")
        {
            this.gameObject.GetComponent<Player>().speed.y = 16.5f;
        }

        if (collisionEnter.gameObject.tag == "mediumBoostBox")
        {
            this.gameObject.GetComponent<Player>().speed.y = 30.0f;
        }

        if (collisionEnter.gameObject.tag == "bigBoostBox")
        {
            this.gameObject.GetComponent<Player>().speed.y = 70.0f;
        }
    }


    void OnTriggerStay(Collider collisionStay)
    {
        if(collisionStay.gameObject.tag == "Lava")
        {
            this.gameObject.GetComponent<Player>().health -= lavaDamageOverTime;
            //Debug.Log(this.gameObject.GetComponentInParent<Player>().health);
        }
    }

    void damageKnockBack(float direction, float knockBackPower)
    {
        if (direction > 0.0f)
        {
            this.gameObject.GetComponent<Player>().speed.x += -5.0f;
        }
        else
        {
            this.gameObject.GetComponent<Player>().speed.x += 5.0f;
        }
    }
}
