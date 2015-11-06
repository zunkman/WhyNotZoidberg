using UnityEngine;
using System.Collections;

public class TankgirlAttack : MonoBehaviour {

    private float damage;
    public bool isActive;
    ArrayList hitObjects;
    int chargebonus;
    // Use this for initialization
    void Start()
    {
        hitObjects = new ArrayList();
        damage = this.GetComponentInParent<TankGirl>().attackDamage;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<MeshRenderer>().enabled = isActive;
        if (!isActive) {
            hitObjects.Clear();
        }
        if (this.GetComponentInParent<TankGirl>().charging) { chargebonus = 2; } else { chargebonus = 1; }
    }

    void OnTriggerStay(Collider other)
    {
        
        if (other.gameObject.tag == "Enemy" && isActive)
        {

            if (!hitObjects.Contains(other.gameObject)) { 

                other.gameObject.GetComponent<EnemyDamage>().health -= damage*chargebonus;
                hitObjects.Add(other.gameObject);
            }
            

        }
    }

}
