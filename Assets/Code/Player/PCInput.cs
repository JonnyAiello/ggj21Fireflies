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
    [SerializeField] private bool leftDoubleTap;
    [SerializeField] private bool rightDoubleTap; 
    [SerializeField] private bool downButton;     
	[SerializeField] private bool jumpButton; 
    [SerializeField] private bool dashButton; 
    private Dictionary<string, DoubleTapInput> doubleTapDict 
        = new Dictionary<string, DoubleTapInput>();
    private bool leftButtonDownLock; 
    private bool rightButtonDownLock; 

    // Properties
    public bool LeftButton { get{return leftButton;} }
    public bool RightButton { get{return rightButton;} }
    public bool DownButton { get{return downButton;} }
    public bool JumpButton { get{return jumpButton;} }
    public bool DashButton { get{return dashButton;} }
    public bool LeftDT { get{return leftDoubleTap;} }
    public bool RightDT { get{return rightDoubleTap;} }


    // [[ ----- INPUT UPDATE ----- ]]
    public void InputUpdate(){    	
        // only engage here, disengage bool in fixed update after PlayerMove
        // has had a chance to process the button push (avoid dropped inputs)
        
        // left
        if( (!leftButton && Input.GetButton("LeftButton"))
            || Input.GetAxis("Horizontal") < -0.1f ){ 
            
            leftButton = true;
        }

        // left double-tap logic 
        if( leftButton ){ leftButtonDownLock = true; }
        if( leftButtonDownLock && !leftButton ){ leftButtonDownLock = false; }
        else if( leftButtonDownLock ){
            if( !doubleTapDict.ContainsKey("leftButton") ){ 
                doubleTapDict.Add("leftButton", new DoubleTapInput());
            }else{ doubleTapDict["leftButton"].UpdateDP(true); }
        }else if( !leftButtonDownLock ){
            if( doubleTapDict.ContainsKey("leftButton") ){
                doubleTapDict["leftButton"].UpdateDP(false);
            }
        }

        // right
        if( (!rightButton && Input.GetButton("RightButton"))
            || Input.GetAxis("Horizontal") > 0.1f){ 

            rightButton = true; 
        }

        // right double-tap logic
        if( rightButton ){ rightButtonDownLock = true; }
        if( rightButtonDownLock && !rightButton ){ rightButtonDownLock = false; }
        else if( rightButtonDownLock ){
            if( !doubleTapDict.ContainsKey("rightButton") ){ 
                doubleTapDict.Add("rightButton", new DoubleTapInput());
            }else{ doubleTapDict["rightButton"].UpdateDP(true); }
        }else if( !rightButtonDownLock ){
            if( doubleTapDict.ContainsKey("rightButton") ){
                doubleTapDict["rightButton"].UpdateDP(false);
            }
        }

        // down
        if( (!downButton && Input.GetButton("DownButton"))
            || Input.GetAxis("Vertical") < -0.1f){ 

            downButton = true; 
        }

        // jump + dash
        if( !jumpButton && Input.GetButton("Jump") ){ jumpButton = true; }
        if( !dashButton && Input.GetButton("Dash") ){ dashButton = true; }

        // check for successful touble-taps and clean up finished/unsuccessfuls
        List<string> keysToRemove = new List<string>(); 
        foreach( KeyValuePair<string,DoubleTapInput> dti in doubleTapDict ){
            if( dti.Value.Succeeded ){
                switch( dti.Key ){
                    case "leftButton": 
                        leftDoubleTap = true; 
                        break;
                    case "rightButton": 
                        rightDoubleTap = true; 
                        break;
                }
            }
            if( dti.Value.IsFinished() ){ keysToRemove.Add(dti.Key); }
        }
        foreach( string k in keysToRemove ){ doubleTapDict.Remove(k); }
    }

    // [[ ----- RESET INPUTS ----- ]]
    public void ResetInputs(){
        if( (leftButton && !Input.GetButton("LeftButton")) 
            || Input.GetAxis("Horizontal") > -0.1f){
            
            leftButton = false;
            leftDoubleTap = false; 
        }
        if( (rightButton && !Input.GetButton("RightButton"))
            || Input.GetAxis("Horizontal") < 0.1f){ 

            rightButton = false;
            rightDoubleTap = false;  
        }
        if( (downButton && !Input.GetButton("DownButton"))
            || (downButton && Input.GetAxis("Vertical") > -0.1f)){ 

            downButton = false; 
        }
        if( jumpButton && !Input.GetButton("Jump") ){ jumpButton = false; }
        if( dashButton && !Input.GetButton("Dash") ){ dashButton = false; }
         
    }
}




public class DoubleTapInput {

    // Variables
    private const float firstTapMaxDuration = 0.5f;
    private const float releaseMaxDuration = 0.6f; 
    private float startTime;
    private State state; 
    private bool isFinished; 
    private bool succeeded;

    // Properties
    public bool Succeeded { get{return succeeded;} }

    // enums
    public enum State{
        FirstTap,
        Release,
        Finished
    }

    // [[ ----- CONSTRUCTOR ----- ]]
    public DoubleTapInput(){
        startTime = Time.time; 
        state = State.FirstTap; 
    }

    // [[ ----- UPDATE DP ----- ]]
    public void UpdateDP( bool _inputBool ){
        float timer = Time.time - startTime;
        switch( state ){
            
            case State.FirstTap:
                if( _inputBool ){
                    if( timer > firstTapMaxDuration ){ isFinished = true; }
                }else{
                    state = State.Release; 
                    startTime = Time.time; 
                }
                break;
            
            case State.Release:
                if( !_inputBool ){
                    if( timer > releaseMaxDuration ){ isFinished = true; }
                }else{
                    state = State.Finished; 
                    succeeded = true; 
                }
                break;
            
            case State.Finished: break;
            default: break;
        }
    }

    // [[ ----- IS FINISHED ----- ]]
    public bool IsFinished(){
        if( isFinished ){ return true; }
        if( state == State.Finished ){ return true; }
        return false; 
    }
}
