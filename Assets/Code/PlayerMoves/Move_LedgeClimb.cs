using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_LedgeClimb : MoveBehavior {

    // Variables
    [SerializeField] private bool isActive; 
    [SerializeField] private float boostForce = 5f; 
    private bool overridden; 
    private bool boostLock; 

    // Reference Variables
    private PCMove pcMove;
    private PCInput pcInput; 
    private PCState	pcState;

    // Properties
    public bool IsActive { get{return isActive;} }

    private void Awake(){
    	pcMove = GetComponent<PCMove>(); 
    	pcInput = GetComponent<PCInput>(); 
    	pcState = GetComponent<PCState>(); 
    }


// -----------------------------------------------------------------------------
// Movebehaviors

    public override void Init( bool _overridden ){
    	overridden = _overridden; 
    	isActive = false; 

    	// if grounded, reset boost lock
    	if( pcState.Grounded && boostLock ){ boostLock = false; }

    	// set active
    	if( !overridden && !boostLock && !pcState.Grounded 
    		&& pcState.LedgeClimbable ){

    		if( pcState.LedgeClimbableRight && pcInput.RightButton
    			|| pcState.LedgeClimbableLeft && pcInput.LeftButton){

    			isActive = true; 
    			boostLock = true; 
    		}
    	}
    }

    public override bool AffectsForce(){ return isActive; }

 	// [[ ----- GET FORCE ----- ]]
    public override Vector2 GetForce(){
    	return new Vector2(0, boostForce); 
    }   
}
