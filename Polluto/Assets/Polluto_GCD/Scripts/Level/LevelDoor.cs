using UnityEngine;
using System.Collections;

public class LevelDoor : MonoBehaviour {

    [SerializeField] public string gameOverScene = "GameOver";
	[SerializeField] public string levelToLoad = "";

    bool closing = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //LevelDoor test = FindObjectOfType<LevelDoor>();
        //GameObject doorObject = test.transform.parent.gameObject;
        //for game handler...
        //if(endLevelDoor != null) endLevelDoor.SetActive(false);
	}
	void OnTriggerEnter2D (Collider2D other) {
		if(other.gameObject.tag == "Player") {
		if(levelToLoad != "") {
                StartCoroutine(EnderFunction());
    		}
		}
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (levelToLoad != "")
            {
                StartCoroutine(EnderFunction());
            }
        }
    }

    IEnumerator EnderFunction() {
        Application.LoadLevelAdditive(gameOverScene);
        yield return new WaitForSeconds(1.25f);
        Application.LoadLevelAdditive(gameOverScene);
        yield return new WaitForSeconds(1.00f);
        Application.LoadLevelAdditive(gameOverScene);
        yield return new WaitForSeconds(0.75f);
        Application.LoadLevelAdditive(gameOverScene);
        yield return new WaitForSeconds(0.50f);
        Application.LoadLevelAdditive(gameOverScene);
        yield return new WaitForSeconds(0.25f);
        Application.LoadLevelAdditive(gameOverScene);
        yield return new WaitForSeconds(0.25f);
        Application.LoadLevelAdditive(gameOverScene);
        yield return new WaitForSeconds(0.25f);
        Application.LoadLevelAdditive(gameOverScene);
        yield return new WaitForSeconds(0.25f);
        Application.LoadLevelAdditive(gameOverScene);
        yield return new WaitForSeconds(0.25f);
        Application.LoadLevelAdditive(gameOverScene);
        yield return new WaitForSeconds(0.25f);
        Application.LoadLevelAdditive(gameOverScene);
        yield return new WaitForSeconds(0.25f);
        Application.LoadLevelAdditive(gameOverScene);
        yield return new WaitForSeconds(0.25f);
        Application.LoadLevelAdditive(gameOverScene);
        yield return new WaitForSeconds(2.50f);
        Application.LoadLevel (levelToLoad);
    }
}
