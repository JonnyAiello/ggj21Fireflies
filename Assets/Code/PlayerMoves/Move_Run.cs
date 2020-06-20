﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Run allows PC to move at their maximum horiz speed. Set run speed by changing
the maxHSpeed value of PCMove */

public class Move_Run : MoveBehavior {
    
	// Variables
    [SerializeField] private bool isActive; 
    private float accelSpeed = 2f;
    private bool overriden; 
	private float minH;
    private float maxH; 
  
	// Reference Variables
	private PCInput pcInput; 
	private PCState pcState;
    private PCMove pcMove;  

    // Properties
    public bool IsActive { get{return isActive;} }

	private void Awake(){
		pcInput = GetComponent<PCInput>();
		pcState = GetComponent<PCState>(); 
        pcMove = GetComponent<PCMove>(); 
	}

// -----------------------------------------------------------------------------
// Movebehavior

    public override void Init( bool _overridden ){
        overriden = _overridden; 
    	if( !overriden
            && (pcInput.LeftDT || pcInput.RightDT) ){ 

            isActive = true; 
            pcState.MovingHoriz = true; 
            pcState.Running = true; 
        }else{
            isActive = false; 
            pcState.MovingHoriz = false; 
            pcState.Running = false; 
        }
    }

    public override bool AffectsForce(){ return isActive; }
    
    // [[ ----- GET FORCE ----- ]]
    public override Vector2 GetForce(){
    	float hForce = accelSpeed;
        if( pcInput.LeftButton ){ hForce *= -1; }
    	return new Vector2(hForce, 0); 
    }

    public override bool AffectsHLimits(){ return isActive; }

    // [[ ----- GET H LIMITS ----- ]]
    public override Vector2 GetHLimits(){ 
        minH = pcMove.MaxHSpeed * -1; 
        maxH = pcMove.MaxHSpeed; 

        // stop at wall
        if( pcState.Walled ){
            if( pcState.WalledRight ){ maxH = 0; }
            if( pcState.WalledLeft ){ minH = 0; }
        }
        
        return new Vector2(minH, maxH); 
    }   
}
