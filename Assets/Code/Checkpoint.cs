using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// all checkpoint game objects require unique names 

public class Checkpoint : MonoBehaviour{ 

	// Variables
	private static List<Checkpoint> levelCheckpoints = new List<Checkpoint>(); 
	public string level; 
	private float timer; 
	private float lockTime = 1f; 
	private bool onHitLock; 
	private bool isCurrCheckpoint; 

    // Reference Variables
    // private SceneMaster sceneMaster; 

    // Properties
    public string Level { get{return level;} }

	// Event setup
	public delegate void UpdateCheckpointDel( string _cpName ); 
	public static event UpdateCheckpointDel OnCheckpointHit; 


	// [[ ----- ON ENABLE ----- ]]
	private void OnEnable(){
		OnCheckpointHit += UpdateCheckpoint; 
		SceneManager.sceneLoaded += Initialize; 
	}

	// [[ ----- ON DISABLE ----- ]]
	private void OnDisable(){
		OnCheckpointHit -= UpdateCheckpoint; 
		SceneManager.sceneLoaded -= Initialize;
	}

	// [[ ----- AWAKE ----- ]]
	private void Awake(){
		// awake code is run the first time checkpoint is loaded, code to run
		// on a scene load goes in Initialize() 

		// DontDestroyOnLoad(gameObject); 

		// set the level id this checkpoint is attached to
		Scene scene = SceneManager.GetActiveScene();
		level = scene.name;
		Initialize(scene, LoadSceneMode.Single);

	}

	// [[ ----- UPDATE ----- ]]
	private void Update(){
		if( onHitLock ){
			timer += Time.deltaTime;
			if( timer > lockTime ){ 
				onHitLock = false; 
				timer = 0; 
			}
		}
	}

	// [[ ----- INITIALIZE ----- ]]
	private void Initialize( Scene scene, LoadSceneMode mode ){
		if( level == SceneManager.GetActiveScene().name ){
			if( !levelCheckpoints.Contains(this) ){ levelCheckpoints.Add(this); }
		}else{
			if( levelCheckpoints.Contains(this) ){ levelCheckpoints.Remove(this); }
			Destroy(gameObject); 
		}
	}

	public void UpdateCheckpoint( string _cpName ){
		// called by OnCheckpointHit event
		if( gameObject.name == _cpName ){
			SceneMaster.active.currentCheckpoint = this; 
		}
	}

	private void OnTriggerEnter2D( Collider2D _c ){
		if( !onHitLock && !isCurrCheckpoint ){
			if( _c.gameObject.tag == "Player" ){
				Debug.Log("Checkpoint Hit: " + gameObject.name);
				onHitLock = true; 
				// trigger checkpoint event to deactivate all 
				Checkpoint.OnCheckpointHit( gameObject.name ); 
			}
		}
	}
    
}
