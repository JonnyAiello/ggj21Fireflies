using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyCollider : MonoBehaviour {

	// Reference Variables
	public FireflyPickup ffPickup; 

    private void OnTriggerEnter2D( Collider2D c ){
    	if( c.gameObject.tag == "Player" ){
    		// update pickup total
    		ffPickup.OnHit();
    	}
    }
}
