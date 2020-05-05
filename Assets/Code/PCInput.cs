using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCInput : MonoBehaviour{

	// Variables
	[SerializeField] private float h; 
	[SerializeField] private bool jumpButton; 
    [SerializeField] private bool dashButton; 

    // Properties
    public float H { get{return h;} }
    public bool JumpButton { get{return jumpButton;} }
    public bool DashButton { get{return dashButton;} }


    // Update 
    public void InputUpdate(){
    	h = Input.GetAxis("Horizontal");
    	
        // only engage here, disengage bool in fixed update after PlayerMove
        // has had a chance to process the button push (avoid dropped inputs)
        if( !jumpButton && Input.GetButton("Jump") ){ jumpButton = true; }
        if( !dashButton && Input.GetButton("Fire1") ){ dashButton = true; }
    }

    // FixedUpdate
    public void InputFixedUpdate(){
    	// h = Input.GetAxis("Horizontal");
    	// jumpButton = false; 
    }

    public void ResetInputJump(){
        jumpButton = false; 
    }
}
