using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// all checkpoint game objects require unique names 

public class Checkpoint : MonoBehaviour{ 

	// Variables
	private bool isCurrCheckpoint;
	private bool isUnlocked; 
	private string noticeString1 = "+";
	private string noticeString2 = " Fireflies \n Required";

	// Reference Variables
	private GameObject activeSprite; 
	private GameObject inactiveSprite; 
	public PopupText popupText;

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
		if( SceneMaster.active.currentCheckpoint == this ){
			// SetSpriteActive(true);
			UpdateCheckpoint( gameObject.name );
		}else{ SetSpriteActive(false); }
	}

	// [[ ----- UPDATE CHECKPOINT ----- ]]
	// called by OnCheckpointHit event
	public void UpdateCheckpoint( string _cpName ){
		if( gameObject.name == _cpName ){
			SceneMaster.active.currentCheckpoint = this; 
			isUnlocked = true;
			SetSpriteActive(true); 
		}else{
			SetSpriteActive(false); 
		}
	}

	// [[ ----- ON TRIGGER ENTER 2D ----- ]]
	private void OnTriggerEnter2D( Collider2D _c ){
		if( !isCurrCheckpoint && !isUnlocked ){
			if( _c.gameObject.tag == "Player" ){
				Debug.Log("Checkpoint Hit: " + gameObject.name);
				PurchaseCheckpoint();
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

	// [[ ----- PURCHASE CHECKPOINT ----- ]]
	private void PurchaseCheckpoint(){
		// check if pc has enough ffs
		if( SceneMaster.active.FOwnedCount >= 3 ){
			// deduct the ffs
			SceneMaster.active.UpdateFFCount( -3 );
			Checkpoint.OnCheckpointHit( gameObject.name );
			// reset ff pickups
			foreach( FireflyPickup ff in FireflyPickup.MasterList ){
				ff.SetActive(true); 
			}
		}else{
			// display how many ffs required
			int ffreq = 3-SceneMaster.active.FOwnedCount;
			popupText.SetText(noticeString1 + ffreq + noticeString2);
			popupText.PlayAnim();
		}
	}
    
}
