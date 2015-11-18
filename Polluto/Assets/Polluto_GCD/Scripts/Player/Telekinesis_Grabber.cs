using UnityEngine;
using System.Collections;

//This script should grab any objects with <TossedGarbage> and lift them into the air.
//When the player moves to the left or right, throw the garbage.

public class Telekinesis_Grabber : MonoBehaviour {

    [SerializeField] private TossedGarbage[] CollectedTrash = null;
    [SerializeField] private TossedGarbage targetSelected = null;
    [SerializeField] public bool TKActive = false;
    [SerializeField]    public Vector3 inputDirection = new Vector2(0, 0);

    //used to track units within targeting range
	//[SerializeField] private TossedGarbage targetSelected = null;
	
	// Use this for initialization
	void Start () {
	
	}

    void Update () {
        if(CollectedTrash != null && TKActive) {
            
            if(CollectedTrash.Length > 0) {
                if(inputDirection != Vector3.zero) {
                    for(int i = 0; i < CollectedTrash.Length;i++){
                        if(CollectedTrash[i] == null) continue;//nulls will be cleaned up on their own later
                        //Vector3 offset = this.transform.position - CollectedTrash[i].transform.position;
				        CollectedTrash[i].GetComponent<Rigidbody>().velocity = CollectedTrash[i].GetComponent<Rigidbody>().velocity + inputDirection * Time.deltaTime * 10.0f;
                        CollectedTrash[i].isWeapon = true;
			        }
                } else {
                    //if there's no movement simply collect
                    for(int i = 0; i < CollectedTrash.Length;i++){
                        if(CollectedTrash[i] == null) continue;//nulls will be cleaned up on their own later
                        CollectedTrash[i].GetComponent<Rigidbody>().velocity = CollectedTrash[i].GetComponent<Rigidbody>().velocity * Mathf.Pow(0.5f, Time.deltaTime);
                        Vector3 offset = this.transform.position - CollectedTrash[i].transform.position;
				        CollectedTrash[i].transform.position += offset * 0.1f;
			        }
                }
            }
        }
    }

	//--The next 150 lines or so were ripped from another project by C T to use for managing an array inside a trigger area.--//
    // A list might have been simpler... or just a function to return all overlapping actors.
	public GameObject getTargetSelected(int forTeam){
		clearVoidsFromArray();
		if(CollectedTrash == null) return null;
		targetSelected = null;
		for(int i = 0; i < CollectedTrash.Length;i++){
			//if(CollectedTrash[i].teamNumber != forTeam){ 
				//if(CollectedTrash[i].checkTargetable(targeting)){
					targetSelected = CollectedTrash[i];
				//}
			//}
		}
		if(targetSelected == null) {
			return null;
		} else {
			return targetSelected.transform.parent.gameObject;
		}
	}

	//on Trigger enter, if unit not in array, add unit to array, if no current target set target from array
	void OnTriggerEnter2D (Collider2D other) {
        //Debug.Log ("OTE2D"  + other.tag);
		TossedGarbage othersScript = other.gameObject.GetComponent<TossedGarbage>();
		if(othersScript != null) {
			addUnitToArray(othersScript);//other.transform.parent.gameObject);
			//Debug.Log ("Entering");
		}
	}
	void OnTriggerEnter (Collider other) {
        //Debug.Log ("OTE"  + other.tag);
		TossedGarbage othersScript = other.gameObject.GetComponent<TossedGarbage>();
		if(othersScript != null) {
			addUnitToArray(othersScript);//other.transform.parent.gameObject);
			//Debug.Log ("Entering");
		}
	}

	//on Trigger exit, remove unit from array, if current target exits get a new target from array
	void OnTriggerExit2D (Collider2D other) {
        //Debug.Log ("OTX2D"  + other.tag);
		TossedGarbage othersScript = other.gameObject.GetComponent<TossedGarbage>();
		if(othersScript != null) {
			//Debug.Log ("Exiting");
			//Debug.Log ("Exit:" + other.name);
			subUnitFromArray(othersScript);//other.transform.parent.gameObject);
		}
	}
	void OnTriggerExit (Collider other) {
        //Debug.Log ("OTX"  + other.tag);
		TossedGarbage othersScript = other.gameObject.GetComponent<TossedGarbage>();
		if(othersScript != null) {
			//Debug.Log ("Exiting");
			//Debug.Log ("Exit:" + other.name);
			subUnitFromArray(othersScript);//other.transform.parent.gameObject);
		}
	}

	void AddToArray(TossedGarbage toAdd) {
		TossedGarbage[] CollectedTrashBuffer;
	}
	public void addUnitToArray(TossedGarbage triggerUnit) {
		clearVoidsFromArray();
		bool duplicate = false;
		TossedGarbage[] CollectedTrashBuffer;
		if(CollectedTrash == null) {
			CollectedTrash = new TossedGarbage[1];
			CollectedTrash[0] = triggerUnit;
			//Debug.Log ("LeadUnit[" + triggerUnit.GetComponent<minimapDotScript>().team + "] " + triggerUnit.name);
		} else {
			int i = 0;
			CollectedTrashBuffer = null;
			CollectedTrashBuffer = new TossedGarbage[CollectedTrash.Length];
			for(i = 0; i < CollectedTrash.Length;i++){
				CollectedTrashBuffer[i] = CollectedTrash[i];
				if(CollectedTrash[i] == triggerUnit) { duplicate = true;
					Debug.Log ("Duplicate Unit!!"); }
			}
			if(duplicate == false) {
				CollectedTrash = null;
				CollectedTrash = new TossedGarbage[CollectedTrashBuffer.Length + 1];
				//--  --//
				for(i = 0; i < CollectedTrashBuffer.Length;i++){
					CollectedTrash[i] = CollectedTrashBuffer[i];
					//Debug.Log ("Unit " + i + " " + CollectedTrash[i].name);
				}
				CollectedTrash[i] = triggerUnit;
				//Debug.Log ("AddedUnit[" + triggerUnit.GetComponent<minimapDotScript>().team + "] " + triggerUnit.name);
				//Debug.Log ("Unit " + i + " " + CollectedTrash[i].name);
			}
			//Debug.Log ("Duplicate Unit!! Can not add to array!");
		}
	}
	private void clearVoidsFromArray() {
		if(CollectedTrash != null) {
			TossedGarbage[] CollectedTrashBuffer;
			int i = 0; int offset = 0; int nulls = 0;
			CollectedTrashBuffer = null;
			CollectedTrashBuffer = new TossedGarbage[CollectedTrash.Length];
			for(i = 0; i < CollectedTrash.Length;i++){
				if(CollectedTrash[i] == null){
					nulls += 1;
				} else {
					CollectedTrashBuffer[i-nulls] = CollectedTrash[i];
				}
			}
			CollectedTrash = null;
			CollectedTrash = new TossedGarbage[i-nulls];
			for(i = 0; i < CollectedTrashBuffer.Length-nulls;i++){
				CollectedTrash[i] = CollectedTrashBuffer[i];
			}
		}
	}
	public void subUnitFromArray(TossedGarbage triggerUnit) {
		if(CollectedTrash != null) {
			TossedGarbage[] CollectedTrashBuffer;
			if(CollectedTrash.Length == 1) {
				CollectedTrash = null;
			} else {
				int i = 0; int offset = 0; int nulls = 0;
				CollectedTrashBuffer = null;
				CollectedTrashBuffer = new TossedGarbage[CollectedTrash.Length];
				for(i = 0; i < CollectedTrash.Length;i++){
					//modified to exclude the specified unit as well as any null values
					if(CollectedTrash[i] == null || CollectedTrash[i] == triggerUnit){
						nulls += 1;
					} else {
						CollectedTrashBuffer[i-nulls] = CollectedTrash[i];
					}
				}
				CollectedTrash = null;
				CollectedTrash = new TossedGarbage[i-nulls];
				for(i = 0; i < CollectedTrashBuffer.Length-nulls;i++){
					if(CollectedTrashBuffer[i] == triggerUnit) { //this should ALWAYS return false due to the previous for loop's conditions
						offset -= 1;
					} else {
						CollectedTrash[i+offset] = CollectedTrashBuffer[i];
					}
				}
			}
		}
	}
}
