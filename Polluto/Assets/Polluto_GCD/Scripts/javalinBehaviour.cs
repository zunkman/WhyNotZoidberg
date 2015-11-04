using UnityEngine;
using System.Collections;

public class javalinBehaviour : MonoBehaviour
{
    private Vector3 newPosition;
    [SerializeField]private float currentTime;
    public float speed;
    public int direction;
    public float aliveTimer;

    public float javalinDamage;
    // Use this for initialization
    void Start ()
    {
        newPosition = this.gameObject.transform.position;

        GameObject player = GameObject.FindGameObjectWithTag("Attack");
        direction = player.GetComponent<LooperFunctionality>().javalinDirection;
        speed = player.GetComponent<LooperFunctionality>().javalinSpeed;
        aliveTimer = player.GetComponent<LooperFunctionality>().javalinDuration;

        javalinDamage = 50.0f;
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
            colliderEnter.gameObject.GetComponent<EnemyDamage>().takeDamage(javalinDamage);
        }
    }

}
