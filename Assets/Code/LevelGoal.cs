using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class LevelGoal : MonoBehaviour {
    
	private bool triggerLock;

	void OnTriggerEnter2D(Collider2D col){
		if( !triggerLock ){
			if( col.gameObject.tag == "Player" ){
				Debug.Log("YOU WIN");
				triggerLock = false; 
				Invoke("ReloadLevel", 1f); 
			}
		}
	}

	private void ReloadLevel(){
		SceneManager.LoadScene("SampleScene");
	}
}
