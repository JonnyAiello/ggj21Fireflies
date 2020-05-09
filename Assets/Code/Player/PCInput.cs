using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCInput : MonoBehaviour{

	// Variables
    [SerializeField] private bool leftButton; 
    [SerializeField] private bool rightButton; 
	[SerializeField] private bool jumpButton; 
    [SerializeField] private bool dashButton; 

    // Properties
    public bool LeftButton { get{return leftButton;} }
    public bool RightButton { get{return rightButton;} }
    public bool JumpButton { get{return jumpButton;} }
    public bool DashButton { get{return dashButton;} }


    // [[ ----- INPUT UPDATE ----- ]]
    public void InputUpdate(){    	
        // only engage here, disengage bool in fixed update after PlayerMove
        // has had a chance to process the button push (avoid dropped inputs)
        if( (!leftButton && Input.GetButton("LeftButton"))
            || Input.GetAxis("Horizontal") < -0.1f ){ 
            
            leftButton = true; 
        }
        if( (!rightButton && Input.GetButton("RightButton"))
            || Input.GetAxis("Horizontal") > 0.1f){ 

            rightButton = true; 
        }
        if( !jumpButton && Input.GetButton("Jump") ){ jumpButton = true; }
        if( !dashButton && Input.GetButton("Dash") ){ dashButton = true; }
    }

    // [[ ----- RESET INPUTS ----- ]]
    public void ResetInputs(){
        if( (leftButton && !Input.GetButton("LeftButton")) 
            || Input.GetAxis("Horizontal") > -0.1f){
            
            leftButton = false; 
        }
        if( (rightButton && !Input.GetButton("RightButton"))
            || Input.GetAxis("Horizontal") < 0.1f){ 

            rightButton = false; 
        }
        if( jumpButton && !Input.GetButton("Jump") ){ jumpButton = false; }
        if( dashButton && !Input.GetButton("Dash") ){ dashButton = false; }
         
    }
}
