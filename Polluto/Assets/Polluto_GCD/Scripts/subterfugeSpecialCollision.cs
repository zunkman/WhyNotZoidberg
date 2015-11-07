using UnityEngine;
using System.Collections;

//--Consider placing subterfuge specific events in the subterfuge level.--//
//--Script modified to work with subterfuge level script instead of gamehandler--//
public class subterfugeSpecialCollision : MonoBehaviour
{
    //private GameObject gameHandler;
    private GameObject switchMissionHitBox;
    [SerializeField]private LevelScript_Subterfuge subterfugeScript;
    void Start()
    {
        subterfugeScript = FindObjectOfType<LevelScript_Subterfuge>();
        //gameHandler = GameObject.FindGameObjectWithTag("gameHandler");
        switchMissionHitBox = GameObject.FindGameObjectWithTag("beginSwitchMission");
    }

    void OnTriggerEnter(Collider colliderEnter)
    {
        if(colliderEnter.gameObject.tag == "beginSwitchMission")
        {
            switchMissionHitBox.SetActive(false);
            subterfugeScript.switchMission = true;
            subterfugeScript.subterfugeMissionUpdater();
        }
    }
}
