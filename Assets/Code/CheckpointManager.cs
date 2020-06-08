using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class CheckpointManager : MonoBehaviour {
    
	// Variables
	public static CheckpointManager active; 
	private string levelID;

	// Properties
	public string LevelID { get{return levelID;} }

    // [[ ----- AWAKE ----- ]]
    private void Awake(){
    	Scene scene = SceneManager.GetActiveScene();
    	levelID = scene.name; 

    	// if last checkpoint was on different level, delete it
    	if( CheckpointManager.active != null ){
    		if( CheckpointManager.active.LevelID != scene.name ){
    			Destroy( CheckpointManager.active.gameObject ); 
    			CheckpointManager.active = null; 
    			Debug.Log("Active checkpoint not for this level, deleted");
    		
    		// if checkpoint manager exists for this level, delete this instance
    		}else{
    			Debug.Log("Checkpoint already exists for this level, deleting");
    			Destroy( gameObject ); 
    		}
    	}

    	// if no checkpoint manager is active, make this the active manager
    	if( CheckpointManager.active == null ){
    		CheckpointManager.active = this; 
    		DontDestroyOnLoad(this); 
    		Debug.Log("No active checkpoint found, making this active");
    	}
    }
}
