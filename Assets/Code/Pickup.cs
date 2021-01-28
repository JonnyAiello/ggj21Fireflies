using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D( Collider2D c ){
    	if( c.gameObject.tag == "Player" ){
    		// update pickup total
    		SceneMaster.active.UpdateFFCount( 1 ); 

    		// activate collection animation
    		gameObject.SetActive(false); 
    	}
    }
}
