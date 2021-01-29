using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyPickup : MonoBehaviour {

    // Reference Variables
    public FireflyHover hoverScript;

    private void OnTriggerEnter2D( Collider2D c ){
    	if( c.gameObject.tag == "Player" ){
    		// update pickup total
    		SceneMaster.active.UpdateFFCount( 1 ); 

    		hoverScript.SetActive( false ); 
    	}
    }
}
