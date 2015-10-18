using UnityEngine;
using System.Collections;

public class subterfugeSpecialCollision : MonoBehaviour
{
    private GameObject gameHandler;
    private GameObject switchMissionHitBox;
    void Start()
    {
        gameHandler = GameObject.FindGameObjectWithTag("gameHandler");
        switchMissionHitBox = GameObject.FindGameObjectWithTag("beginSwitchMission");
    }

    void OnTriggerEnter(Collider colliderEnter)
    {
        if(colliderEnter.gameObject.tag == "beginSwitchMission")
        {
            switchMissionHitBox.SetActive(false);
            gameHandler.GetComponent<GameHandler>().switchMission = true;
            gameHandler.GetComponent<GameHandler>().subterfugeMissionUpdater();
        }
    }

    

}
