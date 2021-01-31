using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHint : MonoBehaviour {
    
	public PopupText popupText;

    // [[ ----- ON TRIGGER ENTER 2D ----- ]]
	private void OnTriggerEnter2D( Collider2D _other ){
		if( _other.tag == "Player" && SceneMaster.active.DoorsUnlocked < 2 ){
			popupText.PlayAnim();
		}
	}
}
