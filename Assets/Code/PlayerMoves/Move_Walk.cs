﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Walk : MoveBehavior {
    
	// Variables
    [SerializeField] private bool isActive; 
    [SerializeField] private float accelSpeed = 2f;
    [SerializeField] private float autoBrakes = 1f;
    [SerializeField] private float maxSpeed = 3f; 
    private bool overridden; 
	private float minH;
    private float maxH; 
  
	// Reference Variables
    public Transform foxSprite;
    public Transform foxSpriteWallSlide;
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
        overridden = _overridden; 
    	if( !overridden
    		&& !pcState.Running
            && (pcInput.LeftButton || pcInput.RightButton) ){ 

            isActive = true; 
            pcState.MoveWalk = true; 

            // Flip Sprite
            if( pcInput.LeftButton ){
                if( foxSprite.localScale.x > 0 ){ 
                    foxSprite.localScale = new Vector2(-0.5f, 0.5f); 
                    foxSpriteWallSlide.localScale = new Vector2(0.5f, 0.5f); 
                } 
            }
            else if( pcInput.RightButton ){
                if( foxSprite.localScale.x < 0 ){ 
                    foxSprite.localScale = new Vector2(0.5f, 0.5f);
                    foxSpriteWallSlide.localScale = new Vector2(-0.5f, 0.5f); 
                }
            }

        }else{
            isActive = false; 
            pcState.MoveWalk = false; 
        }   
    }

    public override bool AffectsForce(){ return isActive; }
    
    // [[ ----- GET FORCE ----- ]]
    public override Vector2 GetForce(){
    	float hForce = accelSpeed;
        if( pcInput.LeftButton ){ hForce *= -1; }
    	return new Vector2(hForce, 0); 
    }

    public override bool AffectsHLimits(){ return !(overridden || pcState.Running); }
    
    // [[ ----- GET H LIMITS ----- ]]
    public override Vector2 GetHLimits(){ 
        minH = maxSpeed * -1; 
        maxH = maxSpeed; 

    	// stop at wall
        if( pcState.Walled ){
            if( pcState.WalledRight ){ maxH = 0; }
            if( pcState.WalledLeft ){ minH = 0; }
        
        // horizontal auto-brakes
        }else{
            if( pcState.Grounded ){
                if( !isActive ){
                    minH = autoBrakes * -1;
                    maxH = autoBrakes;
                }else if( pcInput.RightButton ){ minH = autoBrakes * -1; }
                else if( pcInput.LeftButton ){ maxH = autoBrakes; }
            }
        }
        return new Vector2(minH, maxH); 
    }	
}
