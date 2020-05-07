using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCInput : MonoBehaviour{

	// Variables
    [SerializeField] private bool leftButton; 
    [SerializeField] private bool rightButton; 

	[SerializeField] private float h; 
	[SerializeField] private bool jumpButton; 
    [SerializeField] private bool dashButton; 

    // Properties
    public float H { get{return h;} }
    public bool LeftButton { get{return leftButton;} }
    public bool RightButton { get{return rightButton;} }
    public bool JumpButton { get{return jumpButton;} }
    public bool DashButton { get{return dashButton;} }


    // Update 
    public void InputUpdate(){
    	h = Input.GetAxis("Horizontal");
    	
        // only engage here, disengage bool in fixed update after PlayerMove
        // has had a chance to process the button push (avoid dropped inputs)
        if( !leftButton && Input.GetButton("LeftButton") ){ leftButton = true; }
        if( !rightButton && Input.GetButton("RightButton") ){ rightButton = true; }
        if( !jumpButton && Input.GetButton("Jump") ){ jumpButton = true; }
        if( !dashButton && Input.GetButton("Fire1") ){ dashButton = true; }
    }

    // FixedUpdate
    public void InputFixedUpdate(){
    	// h = Input.GetAxis("Horizontal");
    	// jumpButton = false; 
    }

    public void ResetInputs(){
        if( leftButton && !Input.GetButton("LeftButton") ){ leftButton = false; }
        if( rightButton && !Input.GetButton("RightButton") ){ rightButton = false; }
        if( jumpButton && !Input.GetButton("Jump") ){ jumpButton = false; }
         
    }
}
