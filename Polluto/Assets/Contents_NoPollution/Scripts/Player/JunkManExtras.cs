using UnityEngine;
using System.Collections;

public class JunkManExtras : MonoBehaviour {

    [SerializeField]    protected float[] attacking = { 0.0f, 0.0f };
    [SerializeField]    protected float[] attackTimers = { 0.0f, 0.0f };
    [SerializeField]    protected float[] attackCooldowns = { 0.50f, 0.50f };

    [SerializeField]    protected GameObject spawnable;


    void Awake()
    {
    }
	void Start ()
    {
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(Input.GetAxis("Fire1"));
        attacking[0] = Mathf.Abs(Input.GetAxis("Basic Attack"));
        attacking[1] = Mathf.Abs(Input.GetAxis("Special Attack"));
        int weapons = attacking.Length;//number of attacks
        for(int i = 0; i < attacking.Length; i++)
        {
            attackTimers[i] += Time.deltaTime;
            if (attacking[i] >= 0.25f && attackTimers[i] >= attackCooldowns[i])
            {
                attackTimers[i] = 0.0f;//reset timer since we're firing... probably
                if (!spawnable) continue;
                GameObject spawned = Instantiate(spawnable, this.transform.position, this.transform.rotation) as GameObject;
                spawned.GetComponent<Rigidbody>().velocity = spawned.transform.right * 3.0f;
                Debug.Log(spawned.transform.forward);
            }
        }

	}
}
