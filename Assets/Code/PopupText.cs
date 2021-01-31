using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupText : MonoBehaviour {

	// Variables
	public bool triggerZone;
	public bool minFireflyTrigger;
	public bool maxFireflyTrigger;
	public bool singleFire;
	public int minFirefly;
	public int maxFirefly;

	private bool singleLock;

	// Reference Variables
	public GameObject textMesh;
	public Animation popupAnim;
	private TextMeshPro tmp;

	// [[ ----- AWAKE ----- ]]
	private void Awake(){
		tmp = textMesh.GetComponent<TextMeshPro>();
		tmp.color = new Color(1,1,1,0);
	}

	// [[ ----- PLAY ANIM ----- ]]
	public void PlayAnim(){
		if( !singleLock ){
			if( singleFire ){ singleLock = true; }
			popupAnim.Play();
		}
	}

	// [[ ----- SET TEXT ----- ]]
	public void SetText( string _txt ){
		tmp.text = _txt;
	}

	// [[ ----- ON TRIGGER ENTER 2D ----- ]]
	private void OnTriggerEnter2D( Collider2D _other ){
		if( _other.tag == "Player" && !singleLock ){
			if( triggerZone && minFireflyTrigger ){
				if( SceneMaster.active.FOwnedCount >= minFirefly ){ PlayAnim(); }
			}else if( triggerZone && maxFireflyTrigger ){
				if( SceneMaster.active.FOwnedCount <= maxFirefly ){ PlayAnim(); }
			}else if( triggerZone ){ PlayAnim(); }
		}
	}
}
