﻿using UnityEngine;
using System.Collections;

public class javalinBehaviour : MonoBehaviour
{
    private Vector3 newPosition;
    [SerializeField]private float currentTime;
    public float speed;
    public int direction;
    public float aliveTimer;

    public float javelinDamage;
    // Use this for initialization
    void Start ()
    {
        newPosition = this.gameObject.transform.position;

        GameObject player = GameObject.FindGameObjectWithTag("Attack");
        direction = player.GetComponent<LooperFunctionality>().javelinDirection;
        speed = player.GetComponent<LooperFunctionality>().javelinSpeed;
        aliveTimer = player.GetComponent<LooperFunctionality>().javelinDuration;

        javelinDamage = 50.0f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        moveJavalin(speed, direction);
        destoryTimer(aliveTimer);
	}

    public void moveJavalin(float speed, int direction)
    {
        if (direction == 1) //move right
        {
            newPosition.x += speed * Time.deltaTime;
        }
        else
        {
            newPosition.x -= speed * Time.deltaTime;
        }

        this.gameObject.transform.position = newPosition;
    }

    void destoryTimer(float aliveTimer)
    {
        if (currentTime < aliveTimer)
        {
            currentTime += 1 * Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider colliderEnter)
    {
        if (colliderEnter.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy");
            if (colliderEnter.gameObject.GetComponentInParent<EnemyDamage>() == null)
            {
                Debug.Log("damage is in child.");
                colliderEnter.gameObject.GetComponentInChildren<EnemyDamage>().takeDamage(javelinDamage);
            }
            else
            {
                colliderEnter.gameObject.GetComponentInParent<EnemyDamage>().takeDamage(javelinDamage);
            }
        }
    }

}
