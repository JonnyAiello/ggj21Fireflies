using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class LevelGoal : MonoBehaviour {
    
    // Variables
    public string nextLevelID; 
	private bool triggerLock;

	void OnTriggerEnter2D(Collider2D col){
		if( !triggerLock ){
			if( col.gameObject.tag == "Player" ){
				Debug.Log("EXITING LEVEL");
				triggerLock = true; 
				Invoke("LoadLevel", 1f); 
			}
		}
	}

	private void LoadLevel(){
		SceneManager.LoadScene(nextLevelID);
	}
}
