using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class DangerObject : MonoBehaviour{

    private void OnTriggerEnter2D( Collider2D _other ){
    	if( _other.tag == "Player" ){
    		_other.transform.parent.GetComponent<Player>().Die(); 
    	}
    }
}
