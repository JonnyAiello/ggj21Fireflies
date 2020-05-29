using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Input Notes
- Using both digital keys and axis to affect movement, so that these controls
should work with both keyboard and controller input
- Because of this, "Gravity" and "Sensitivity" for both "Horizontal" and "Vertical"
axis in the Input Manager should be set to 1000 - this causes the analogue 
input to act more closely to a digital input
*/

public class PCInput : MonoBehaviour{

	// Variables
    [SerializeField] private bool leftButton; 
    [SerializeField] private bool rightButton; 
    [SerializeField] private bool downButton;     
	[SerializeField] private bool jumpButton; 
    [SerializeField] private bool dashButton; 

    // Properties
    public bool LeftButton { get{return leftButton;} }
    public bool RightButton { get{return rightButton;} }
    public bool DownButton { get{return downButton;} }
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
        if( (!downButton && Input.GetButton("DownButton"))
            || Input.GetAxis("Vertical") < -0.1f){ 

            downButton = true; 
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
        if( (downButton && !Input.GetButton("DownButton"))
            || (downButton && Input.GetAxis("Vertical") > -0.1f)){ 

            downButton = false; 
        }
        if( jumpButton && !Input.GetButton("Jump") ){ jumpButton = false; }
        if( dashButton && !Input.GetButton("Dash") ){ dashButton = false; }
         
    }
}
