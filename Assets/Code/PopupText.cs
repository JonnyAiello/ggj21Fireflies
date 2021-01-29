using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupText : MonoBehaviour {

	// Variables
	public bool triggerZone;

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
		popupAnim.Play();
	}

	// [[ ----- SET TEXT ----- ]]
	public void SetText( string _txt ){
		tmp.text = _txt;
	}

	// [[ ----- ON TRIGGER ENTER 2D ----- ]]
	private void OnTriggerEnter2D( Collider2D _other ){
		if( triggerZone && _other.tag == "Player" ){
			PlayAnim();
		}
	}
}
