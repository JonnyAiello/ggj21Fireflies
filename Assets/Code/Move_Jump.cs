﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Jump : MonoBehaviour {

	// Variables
    [SerializeField] private float jumpForce = 13f;
    [SerializeField] private float jumpReleaseLimit = 0.5f; // maximum upward speed once State button released
    [SerializeField] private State jState; 
    private float vForce; 
    private Vector2 vLimits; 
    private float escapeTimer = 0f; 
	private float escapeDelay = 0.1f; 

    // Reference Variables
    private PCMove pcMove;
    private PCInput pcInput; 
    private PCState pcState;

    // Enums
	public enum State{
		Landed_ButtonReleased,
		Landed_ButtonHeld,
		Jumping_Liftoff,
		Jumping_EscapeVelocity, 
		Jumping_ButtonHeld,
		Jumping_ButtonReleased,
		Freefall
	}

    private void Awake(){
    	pcMove = GetComponent<PCMove>(); 
    	pcInput = GetComponent<PCInput>(); 
    	pcState = GetComponent<PCState>(); 
    }

    public bool IsActive(){
    	return true; 
    }

    public Vector2 GetForces( State _newState ){
    	jState = _newState; 
    	return GetForces(); 
    }
    public Vector2 GetForces(){
    	vForce = 0; 
    	switch( jState ){

    		case State.Landed_ButtonReleased:
    			if( pcMove.Jumping ){ pcMove.SetJumping(false); }
    			if( !pcState.Grounded ){ jState = State.Freefall; }
    			else if( pcInput.JumpButton ){ GetForces( State.Jumping_Liftoff ); }
    			/*Debug.Log("switch: jState = " + State.Landed_ButtonReleased);*/
    			break;
    		
    		// apply State force
			case State.Jumping_Liftoff:
				vForce = jumpForce; 
				jState = State.Jumping_EscapeVelocity; 
				if( !pcMove.Jumping ){ pcMove.SetJumping(true); }
    			/*Debug.Log("switch: jState = " + State.Jumping_Liftoff);*/
    			break;

    		// creates timegap so we escape the ground check
    		case State.Jumping_EscapeVelocity:
    			escapeTimer += Time.deltaTime;
    			if( escapeTimer > escapeDelay ){
    				if( pcInput.JumpButton ){jState = State.Jumping_ButtonHeld;}
    				else{jState = State.Jumping_ButtonReleased;}
    				escapeTimer = 0; 
    			}
    			/*Debug.Log("switch: jState = " + State.Jumping_EscapeVelocity);*/
    			break;

    		case State.Jumping_ButtonHeld:
    			if( pcState.Grounded ){ 
                    // anim.SetBool("Jumped", true); 
                    // anim.SetBool("Landed", false);
                    jState = State.Landed_ButtonHeld; 
                }
    			else if( !pcInput.JumpButton ){
    				jState = State.Jumping_ButtonReleased;
    			}
    			/*Debug.Log("switch: jState = " + State.Jumping_ButtonHeld);*/
    			break;
    		
    		case State.Jumping_ButtonReleased:
    			// wall State
    			if( pcState.Walled && pcInput.JumpButton ){
    				jState = State.Jumping_Liftoff; 
    			
    			}else if( pcState.Grounded ){
                    // anim.SetBool("Jumped", false); 
                    // anim.SetBool("Landed", true);
    				if( pcInput.JumpButton ){jState = State.Landed_ButtonHeld;}
    				else{ jState = State.Landed_ButtonReleased; }
    			}
    			/*Debug.Log("switch: jState = " + State.Jumping_ButtonReleased);*/
    			break;
    		
    		// must release State before next State begins
    		case State.Landed_ButtonHeld:
    			if( pcMove.Jumping ){ pcMove.SetJumping(false); }
    			if( !pcState.Grounded ){ jState = State.Freefall; }
    			if( !pcInput.JumpButton ){jState = State.Landed_ButtonReleased;}
    			/*Debug.Log("switch: jState = " + State.Landed_ButtonHeld);*/
    			break;

    		// if pc steps off or airborn not from jumping
    		case State.Freefall:
    			if( pcState.Walled && pcInput.JumpButton ){
    				jState = State.Jumping_Liftoff; 
    			}
    			if( pcState.Grounded ){
                    // anim.SetBool("Jumped", false); 
                    // anim.SetBool("Landed", true);
    				if( pcInput.JumpButton ){jState = State.Landed_ButtonHeld;}
    				else{jState = State.Landed_ButtonReleased;}
    			}
    			/*Debug.Log("switch: jState = " + State.Freefall);*/
    			break;
    		
    		default:
    			Debug.Log("switch: value match not found");
    			break;
    	}

    	return new Vector2(0, vForce); 
    }

    public Vector2 GetVerticalLimits(){
    	float minV = pcMove.MaxVSpeed * -1; 
    	float maxV = pcMove.MaxVSpeed; 

    	if( jState == State.Jumping_ButtonReleased ){ maxV = jumpReleaseLimit; }

    	return new Vector2(minV, maxV); 
    }
}