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
    private float axisThresh = 0.05f;
    private DoubleTapInput accessDT; //used to reference DTs contained in dtDict

    [SerializeField] private bool leftButton;  
    [SerializeField] private bool rightButton;
    // [SerializeField] private bool leftDoubleTap;
    // [SerializeField] private bool rightDoubleTap; 
    [SerializeField] private bool downButton;     
	[SerializeField] private bool jumpButton; 
    [SerializeField] private bool dashButton; 
    // private Dictionary<string, DoubleTapInput> doubleTapDict 
    //     = new Dictionary<string, DoubleTapInput>();
    private bool leftButtonDownLock; 
    private bool rightButtonDownLock; 

    // Properties
    public bool LeftButton { get{return leftButton;} }
    public bool RightButton { get{return rightButton;} }
    public bool DownButton { get{return downButton;} }
    public bool JumpButton { get{return jumpButton;} }
    public bool DashButton { get{return dashButton;} }
    // public bool LeftDT { get{return leftDoubleTap;} }
    // public bool RightDT { get{return rightDoubleTap;} }


    // [[ ----- AWAKE ----- ]]
    private void Awake(){
        DoubleTapInput.dtDict = null;
        accessDT = new DoubleTapInput(); 
    }

    // [[ ----- INPUT UPDATE ----- ]]
    public void InputUpdate(){    	
        // only engage here, disengage bool in fixed update after PlayerMove
        // has had a chance to process the button push (avoid dropped inputs)
        
        // left
        if( (!leftButton && Input.GetButton("LeftButton"))
            || (!leftButton && Input.GetAxis("Horizontal") < axisThresh * -1) ){ 
            
            leftButton = true;
            // left double-tap logic
            Debug.Log("Updating LEFT");
            accessDT.UpdateDT("leftButton", true);
        }

        // right
        if( (!rightButton && Input.GetButton("RightButton"))
            || (!rightButton && Input.GetAxis("Horizontal") > axisThresh)){ 

            rightButton = true; 
            Debug.Log("Updating RIGHT");
            accessDT.UpdateDT("rightButton", true);
        }

        // down
        if( (!downButton && Input.GetButton("DownButton"))
            || (!downButton && Input.GetAxis("Vertical") < axisThresh * -1)){ 

            downButton = true; 
        }

        // jump + dash
        if( !jumpButton && Input.GetButton("Jump") ){ jumpButton = true; }
        if( !dashButton && Input.GetButton("Dash") ){ dashButton = true; }
    }

    // [[ ----- RESET INPUTS ----- ]]
    public void ResetInputs(){
        // left
        if( (leftButton && !Input.GetButton("LeftButton")) 
            || (leftButton && Input.GetAxis("Horizontal") > axisThresh * -1)){
            
            leftButton = false;
            // double-tap logic
            accessDT.UpdateDT("leftButton", false);
            accessDT.CleanUpDT("leftButton");

        }
        // right
        if( (rightButton && !Input.GetButton("RightButton"))
            || (rightButton && Input.GetAxis("Horizontal") < axisThresh) ){ 

            rightButton = false;
            accessDT.UpdateDT("rightButton", false);
            accessDT.CleanUpDT("rightButton");
        }
        // down
        if( (downButton && !Input.GetButton("DownButton"))
            || (downButton && Input.GetAxis("Vertical") > axisThresh * -1)){ 

            downButton = false; 
        }
        // jump + dash
        if( jumpButton && !Input.GetButton("Jump") ){ jumpButton = false; }
        if( dashButton && !Input.GetButton("Dash") ){ dashButton = false; }
         
    }


    // [[ ----- DOUBLE TAP ACTIVE ----- ]]
    public bool DoubleTapActive( string _inputName ){
        return accessDT.IsActive( _inputName ); 
    }
}




public class DoubleTapInput {

    // Variables
    public static Dictionary<string, DoubleTapInput> dtDict;
    private const float firstTapMaxDuration = 0.25f;
    private const float releaseMaxDuration = 0.6f; 
    private float startTime;
    private State state; 
    private bool isFinished; 
    private bool succeeded;

    // test variables
    public static int instanceIndex; 
    public int index; 
    // --

    // Properties
    public State CurrState { get{return state;} }
    public bool Succeeded { get{return succeeded;} }

    // enums
    public enum State{
        FirstTap,
        Release,
        Finished
    }

    // [[ ----- CONSTRUCTOR ----- ]]
    public DoubleTapInput(){
        // initialize dict
        if( dtDict == null ){
            dtDict = new Dictionary<string, DoubleTapInput>();
        }

        // set up timer + state
        startTime = Time.time; 
        state = State.FirstTap; 

        // test
        index = DoubleTapInput.instanceIndex;
        DoubleTapInput.instanceIndex++; 

        Debug.Log("DTI Created: " + index);
    }

     // [[ ----- UPDATE DT ----- ]]
    public void UpdateDT( string _dtid, bool _inputBool ){
        if( dtDict.ContainsKey(_dtid) ){ dtDict[_dtid].UpdateState(_inputBool); }
        else{
            // create new dt entry
            DoubleTapInput newDT = new DoubleTapInput(); 
            dtDict.Add( _dtid, newDT ); 
        }
    }

    // [[ ----- IS ACTIVE ----- ]]
    public bool IsActive( string _dtid ){
        bool isActive = false; 
        if( dtDict.ContainsKey(_dtid) ){
            if( dtDict[_dtid].Succeeded ){ isActive = true; }
        }
        if( isActive ){
            Debug.Log("Testing IsActive " + _dtid + " " + isActive);
        }
        return isActive; 
    }

    // [[ ----- CLEAN UP DT ----- ]]
    public void CleanUpDT( string _dtid ){
        if( dtDict.ContainsKey(_dtid) ){
            if( dtDict[_dtid].CurrState == State.Finished ){
                Debug.Log("CLEANUP REMOVED: " + _dtid + " " + dtDict[_dtid].index);
                dtDict.Remove(_dtid); 
            }
        }
    }

    // [[ ----- UPDATE STATE ----- ]]
    private void UpdateState( bool _buttonDown ){
        float timer = Time.time - startTime;
        // Debug.Log("Updating State, index: " + index + " state: " + state + " timer: " + timer);
        Debug.Log("updating");
        if( _buttonDown ){
            if( state == State.Release ){
                Debug.Log("moving to state - finished " + index);
                state = State.Finished; 
                if( timer < releaseMaxDuration ){
                    succeeded = true; 
                    Debug.Log("DT SUCCEEDED: " + index); 
                }
            }

        }else{
            // if button up
            if( state == State.FirstTap ){
                if( timer > firstTapMaxDuration ){
                    state = State.Finished; 
                    Debug.Log("FirstTap held beyond limit, dt finished: " + index); 
                }else{
                    state = State.Release;
                    startTime = Time.time; 
                    Debug.Log("FirstTap released, reset timer, move to release state: " + index);
                }
            }
        }
    }
}
