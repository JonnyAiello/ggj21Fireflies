using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DangerObject : MonoBehaviour{

	// Variables
	public bool isDangerous; 
	public bool isLethal;

	// [[ ----- AWAKE ----- ]]
	private void Awake(){
		isDangerous = true; 
	}

    private void OnTriggerEnter2D( Collider2D _other ){
    	if( _other.tag == "Player" && isLethal ){
    		_other.transform.parent.GetComponent<Player>().Die(); 
    	}else if( _other.tag == "Player" && isDangerous ){
    		_other.transform.parent.GetComponent<Player>().GetHurt();
    	}
    }
}
