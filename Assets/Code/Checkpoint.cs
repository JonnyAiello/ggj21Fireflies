using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;

// all checkpoint game objects require unique names 

public class Checkpoint : MonoBehaviour{ 

	// Variables
	private bool isCurrCheckpoint;
	private bool isUnlocked; 
	private string noticeString1 = "+";
	private string noticeString2 = " Fireflies \n Required";

	// Reference Variables
	private GameObject activeSprite; 
	private GameObject unlockedSprite; 
	private GameObject inactiveSprite; 
	public Light2D pointLight; 
	public PopupText popupText;
	public Color activeColor;
	public Color unlockedColor;
	public Color inactiveColor;
	public float activeLightRng;
	public float unlockedLightRng;
	public float inactiveLightRng;

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
		unlockedSprite = transform.Find("unlockedSprite").gameObject; 
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
		
		}else if( !isCurrCheckpoint && isUnlocked ){
			Checkpoint.OnCheckpointHit( gameObject.name );
		}
	}

	// [[ ----- SET SPRITE ACTIVE ----- ]]
	private void SetSpriteActive( bool _true ){
		if( _true ){
			activeSprite.SetActive(true);
			unlockedSprite.SetActive(false);
			inactiveSprite.SetActive(false);
			pointLight.color = activeColor;
			pointLight.pointLightOuterRadius = activeLightRng; 
		}else if( isUnlocked ){
			activeSprite.SetActive(false);
			unlockedSprite.SetActive(true);
			inactiveSprite.SetActive(false); 
			pointLight.color = unlockedColor;
			pointLight.pointLightOuterRadius = unlockedLightRng;
		}else{
			activeSprite.SetActive(false);
			unlockedSprite.SetActive(false);
			inactiveSprite.SetActive(true); 
			pointLight.color = inactiveColor;
			pointLight.pointLightOuterRadius = inactiveLightRng;
		}
	}

	// [[ ----- PURCHASE CHECKPOINT ----- ]]
	private void PurchaseCheckpoint(){
		// check if pc has enough ffs
		if( SceneMaster.active.FOwnedCount >= 3 ){
			// PURCHASE CHECKPOINT
			AudioMaster.active.SoundEffect(AudioMaster.FXType.Checkpoint, 0); 
			// deduct the ffs
			SceneMaster.active.UpdateFFCount( -3 );
			// Make active checkpoint
			Checkpoint.OnCheckpointHit( gameObject.name );
			// reset ff pickups
			foreach( FireflyPickup ff in FireflyPickup.MasterList ){
				ff.SetActive(true); 
			}
			// Unlock door
			SceneMaster.active.UnlockDoor();
		}else{
			// display how many ffs required
			int ffreq = 3-SceneMaster.active.FOwnedCount;
			popupText.SetText(noticeString1 + ffreq + noticeString2);
			popupText.PlayAnim();
		}
	}
    
}
