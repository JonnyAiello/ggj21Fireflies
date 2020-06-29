using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionTrigger : MonoBehaviour {

	// Variables
	public Vector2 cameraPosition; 
	private bool triggeredLock; 

    private void OnTriggerEnter2D( Collider2D _other ){
    	Debug.Log("TESTING - Entered");
    	if( !triggeredLock ){
    		if( _other.gameObject.tag == "Player" ){
    			Debug.Log("Setting location");
    			triggeredLock = true; 
    			Player player = _other.transform.parent.GetComponent<Player>(); 
    			player.SetCamPoint(cameraPosition); 
    			// _other.transform.parent.BroadcastMessage("SetCamPoint", cameraPosition); 
    		}
    	}
    }

    private void OnTriggerExit2D( Collider2D _other ){
    	Debug.Log("TESTING - Exited");
    	if( _other.gameObject.tag == "Player" && triggeredLock ){ 
    		Debug.Log("Resetting location");
    		triggeredLock = false; 
    		Player player = _other.transform.parent.GetComponent<Player>(); 
    		player.SetCamPoint( new Vector2(0,1) ); 
    	}
    }
}
