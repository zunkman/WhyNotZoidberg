using UnityEngine;
using System.Collections;

public class AI_Slime : MonoBehaviour {

    [SerializeField] private bool haveTarget;
    [SerializeField] GameObject theTarget;
    [SerializeField] GameObject attackSubObject;


    [SerializeField] Vector3 groundNormal;
    [SerializeField] float[] AITimer = {0, 1.5f};

	// Use this for initialization
	void Start () {
	    //attackSubObject = GetComponentInChildren<AI_MeleeAttack>().gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	    AITimer[0] += Time.deltaTime;
        if(AITimer[0] >= AITimer[1]) {
            lookForNearbyPlayer();
            AITimer[0] = 0;
        }
	}

    void lookForNearbyPlayer ()
    {
        RaycastHit hit;
        RaycastHit[] hits;

        Vector3[] dirChk = {new Vector3(1f, 0, 0), new Vector3(-1f, 0, 0) };//, new Vector3(0, 1f, 0), new Vector3(0, -1f, 0)};
        Vector3 rayOffset = new Vector3(0, 0, 0);
        Vector3 hitNormal = new Vector3(0, 0, 0);
        float[] dirOffsets = { 0, 0, 0, 0 };

        bool wasHit; int hitCount = 0;
        Vector3 speedAdjust = new Vector3(0, 0, 0);

        for(int i=0;i<dirChk.Length;i++){
            Vector3 tempPos = transform.position;
            if(i == 3) groundNormal = new Vector3(0, 1, 0);
            //for(int rc = -1; rc <= 1;rc++)
            int rc = 0;
            {
                Vector3 nxOffset = Vector3.zero;
			    //ray = new Ray(tempPos, dirChk[i]);
                float rayDist = 12.0f;//Mathf.Abs(dirChk[i].x) * ((RayDist * 0.5f)) + Mathf.Abs(dirChk[i].y) * ((RayDist * 0.5f));
			    hits = null;
                //rayOffset = new Vector3(dirChk[i][1]* ((RayDist * 0.45f) * rc), dirChk[i][0]* ((RayDist * 0.25f) * rc), 0);//offset on other axis
			    hits = Physics.RaycastAll (tempPos + rayOffset, dirChk[i], rayDist);//ray, 1.0f);//
			    Debug.DrawRay (tempPos + rayOffset, dirChk[i]*rayDist, Color.yellow, 1.0f);
			    wasHit = false; int j = 0;
			    while (j < hits.Length) {
				    hit = hits[j];
				    j++;
                    //if(hit.collider.isTrigger == false)
                    {                        
                        if(hit.collider.gameObject.tag.Contains("Player") || hit.collider.GetComponentInParent<Player>())
                        {
                          //  Debug.Log ("RCA hit player:[" + hit.collider.gameObject.name + ", " + hit.collider.gameObject.tag + "],pos:["+ hit.transform.position + "],player:" + transform.position
                          //+ ",dist:" + hit.distance + ",offset:" + nxOffset + ",dirchk:" + dirChk[i]);
						    hitCount++;
                            if(Mathf.Abs(rayDist - hit.distance) > nxOffset.magnitude) { 
                                wasHit = true;
                                nxOffset = dirChk[i] * 1.0f * (hit.distance - rayDist);
                                hitNormal = hit.normal;
                                theTarget = hit.collider.gameObject;
                            }
					    }
				    }
			    }
			    if(wasHit == true) {
                    GetComponent<Rigidbody>().velocity = (theTarget.transform.position.x < transform.position.x)? new Vector3(-1, 0, 0) : new Vector3(1, 0, 0);
			    }
            }
		}
        if(hitCount == 0) {
            theTarget = null;
        } else {
            if(attackSubObject)attackSubObject.SetActive(true);
        }
    }
}
