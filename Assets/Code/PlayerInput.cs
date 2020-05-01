using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour{

	// Variables
	public float h; 
	public bool jumpButton; 
    public bool dashButton; 


    // Update 
    public void InputUpdate(){
        // only engage here, disengage bool in fixed update after PlayerMove
        // has had a chance to process the button push (avoid dropped inputs)
        if( !jumpButton && Input.GetButton("Jump") ){ jumpButton = true; }
        if( !dashButton && Input.GetButton("Fire1") ){ dashButton = true; }
    }

    // FixedUpdate
    public void InputFixedUpdate(){
    	h = Input.GetAxis("Horizontal");
    	// jumpButton = false; 
    }
}
