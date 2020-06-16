using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// all checkpoint game objects require unique names 

public class Checkpoint : MonoBehaviour{ 

	// Variables
	private bool isCurrCheckpoint; 

	// Reference Variables
	private GameObject activeSprite; 
	private GameObject inactiveSprite; 

	// Event setup
	public delegate void UpdateCheckpointDel( string _cpName ); 
	public static event UpdateCheckpointDel OnCheckpointHit; 


	// [[ ----- ON ENABLE ----- ]]
	private void OnEnable(){
		OnCheckpointHit += UpdateCheckpoint; 
	}

	// [[ ----- ON DISABLE ----- ]]
	private void OnDisable(){
		OnCheckpointHit -= UpdateCheckpoint; 
	}

	// [[ ----- START ----- ]]
	private void Start(){
		// set up sprite references
		activeSprite = transform.Find("activeSprite").gameObject; 
		inactiveSprite = transform.Find("inactiveSprite").gameObject; 
		if( SceneMaster.active.currentCheckpoint == this ){SetSpriteActive(true);}
		else{ SetSpriteActive(false); }
	}

	// [[ ----- UPDATE CHECKPOINT ----- ]]
	// called by OnCheckpointHit event
	public void UpdateCheckpoint( string _cpName ){
		if( gameObject.name == _cpName ){
			SceneMaster.active.currentCheckpoint = this; 
			SetSpriteActive(true); 
		}else{
			SetSpriteActive(false); 
		}
	}

	// [[ ----- ON TRIGGER ENTER 2D ----- ]]
	private void OnTriggerEnter2D( Collider2D _c ){
		if( !isCurrCheckpoint ){
			if( _c.gameObject.tag == "Player" ){
				Debug.Log("Checkpoint Hit: " + gameObject.name);
				// trigger checkpoint event to deactivate all 
				Checkpoint.OnCheckpointHit( gameObject.name ); 
			}
		}
	}

	// [[ ----- SET SPRITE ACTIVE ----- ]]
	private void SetSpriteActive( bool _true ){
		if( _true ){
			activeSprite.SetActive(true);
			inactiveSprite.SetActive(false);
		}else{
			activeSprite.SetActive(false);
			inactiveSprite.SetActive(true); 
		}
	}
    
}
