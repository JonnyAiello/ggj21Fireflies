using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class DangerObject : MonoBehaviour{

	// Variables
	public bool isDangerous; 

	// [[ ----- START ----- ]]
	private void Start(){
		isDangerous = true; 
	}

    private void OnTriggerEnter2D( Collider2D _other ){
    	if( _other.tag == "Player" && isDangerous ){
    		_other.transform.parent.GetComponent<Player>().Die(); 
    	}
    }
}
