using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Jump : MoveBehavior {

	// Variables
    [SerializeField] private State jState;
    [SerializeField] private bool isActive; 
    [SerializeField] private float jumpForce = 13f;
    [SerializeField] private float wallJumpForce = 16;
    [SerializeField] private float jumpReleaseLimit = 0.5f; // maximum upward speed once State button released
    [SerializeField] private float bufferWindow = 0.1f; 

     
    private float vForce; 
    private float hForce;  
    private float escapeTimer = 0f; 
	private float escapeDelay = 0.1f; 
    private float bufferTimer; 

    // Reference Variables
    private PCMove pcMove;
    private PCInput pcInput; 
    private PCState pcState;

    // Properties
    public bool IsActive { get{return isActive;} }
    public State JState { get{return jState;} }

    // Enums
	public enum State{
		Landed_ButtonReleased,
		Landed_ButtonHeld,
		Jumping_Liftoff,
        Jumping_DJ_Liftoff,
		Jumping_EscapeVelocity, 
		Jumping_ButtonHeld,
		Jumping_ButtonReleased,
        // Jumping_WallJump,
        JumpBuffer, 
		Freefall
	}

    private void Awake(){
    	pcMove = GetComponent<PCMove>(); 
    	pcInput = GetComponent<PCInput>(); 
    	pcState = GetComponent<PCState>(); 
    }

    private void Update(){
        if( jState == State.JumpBuffer ){
            bufferTimer += Time.deltaTime; 
        }
    }

    // [[ ----- SPEND FF ----- ]]
    private void SpendFF(){
        SceneMaster.active.SpendFFPower(1);
    }

    // [[ ----- RESET FF POWER ----- ]]
    private void ResetFFPower(){
        if( SceneMaster.active.FPowerCount < SceneMaster.active.FOwnedCount ){
            SceneMaster.active.ResetFFPower();
        }
    }

// -----------------------------------------------------------------------------
// State Machine


    // [[ ----- INIT ----- ]]
    public override void Init( bool _overridden ){
        if( _overridden ){ jState = State.Freefall; }
        bool ffAvailable = (SceneMaster.active.FPowerCount > 0);

        // Jump state machine
        switch( jState ){

            case State.Landed_ButtonReleased:
                ResetFFPower();
                if( !pcState.Grounded ){ 
                    jState = State.JumpBuffer;
                    bufferTimer = 0;  
                }else if( pcInput.JumpButton && !pcState.Ducked ){ 
                    jState = State.Jumping_Liftoff; 
                }
                break;
            
            case State.Jumping_Liftoff:
                jState = State.Jumping_EscapeVelocity; 
                break;

            case State.Jumping_DJ_Liftoff:
                SpendFF();
                jState = State.Jumping_EscapeVelocity; 
                break;

            // creates timegap so we escape the ground check
            case State.Jumping_EscapeVelocity:
                escapeTimer += Time.deltaTime;
                if( escapeTimer > escapeDelay ){
                    if( pcInput.JumpButton ){jState = State.Jumping_ButtonHeld;}
                    else{jState = State.Jumping_ButtonReleased;}
                    escapeTimer = 0; 
                }
                break;

            case State.Jumping_ButtonHeld:
                if( pcState.Grounded ){ 
                    jState = State.Landed_ButtonHeld; 
                }else if( !pcInput.JumpButton ){
                    jState = State.Jumping_ButtonReleased;
                }
                break;
            
            case State.Jumping_ButtonReleased:
                if( pcState.Grounded ){
                    if( pcInput.JumpButton ){jState = State.Landed_ButtonHeld;}
                    else{ jState = State.Landed_ButtonReleased; }
                }else if( ffAvailable && pcInput.JumpButton ){
                    jState = State.Jumping_DJ_Liftoff; 
                    // Debug.Log("DJ Liftoff");
                }
                break;
            
            // must release State before next State begins
            case State.Landed_ButtonHeld:
                if( !pcState.Grounded ){ jState = State.Freefall; }
                if( !pcInput.JumpButton ){jState = State.Landed_ButtonReleased;}
                break;

            // if pc steps off of edge, gives grace period
            case State.JumpBuffer:
                if( bufferTimer > bufferWindow ){ jState = State.Freefall; }
                if( bufferTimer < bufferWindow && pcInput.JumpButton ){
                    jState = State.Jumping_Liftoff; 
                }
                if( pcState.Grounded ){
                    if( pcInput.JumpButton ){jState = State.Landed_ButtonHeld;}
                    else{jState = State.Landed_ButtonReleased;}
                }

                break;

            // if pc steps off or airborn not from jumping
            case State.Freefall:
                if( pcState.Walled && pcInput.JumpButton ){
                    jState = State.Jumping_Liftoff; 
                }
                if( pcState.Grounded ){
                    if( pcInput.JumpButton ){jState = State.Landed_ButtonHeld;}
                    else{jState = State.Landed_ButtonReleased;}
                }
                break;
            
            default:
                Debug.LogError("switch: value match not found");
                break;
        }

        // set isActive
        if( jState == State.Jumping_Liftoff
            || jState == State.Jumping_EscapeVelocity
            || jState == State.Jumping_ButtonHeld
            || jState == State.Jumping_ButtonReleased ){

            isActive = true; 
        }else{ isActive = false; }
    }


// -----------------------------------------------------------------------------
// MoveBehavior Overrides 

    // [[ ----- ZERO MOVEMENT ----- ]]
    public override bool ZeroMovement(){
        if( jState == State.Jumping_DJ_Liftoff ){ return true; }
        return false;
    }

    // [[ ----- AFFECTS FORCE ----- ]]
    public override bool AffectsForce(){
        if( jState == State.Jumping_Liftoff
            || jState == State.Jumping_DJ_Liftoff ){

            return true; 
        }
        return false; 
    }

    // [[ ----- GET FORCE ----- ]]
    public override Vector2 GetForce(){
        float vForce = 0;
        float hForce = 0; 

        // if jumping or double-jumping
        if( jState == State.Jumping_Liftoff
            || jState == State.Jumping_DJ_Liftoff ){ 

            vForce = jumpForce; 
        }

        /*// if wall-jumping
        if( jState == State.Jumping_WallJump ){
            vForce = wallJumpForce;
            if( pcState.WalledLeft ){ hForce += 200; }
            else{ hForce += -200; }
        }*/

        return new Vector2(hForce, vForce); 
    } 


    // [[ ----- AFFECTS V LIMITS ----- ]]
    public override bool AffectsVLimits(){
        if( jState == State.Jumping_ButtonReleased ){ 
            return true; 
        }
        return false; 
    }
    

    // [[ ----- GET V LIMITS ----- ]]
    public override Vector2 GetVLimits(){
        float minV = pcMove.MaxVSpeed * -1; 
        float maxV = jumpReleaseLimit; 
        return new Vector2(minV, maxV);
    }  
}
