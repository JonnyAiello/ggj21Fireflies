using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerConditions))]
public class Move_RunJump : MonoBehaviour{

    [SerializeField] private float maxHSpeed = 6f;
    [SerializeField] private float maxVSpeed = 15f;
    [SerializeField] private float accelSpeed = 2f;
    [SerializeField] private float jumpForce = 13f;
    [SerializeField] private float jumpReleaseLimit = .5f; // maximum upward speed once jump button released
    [SerializeField] private float wallSlideMax = -1f;
    [SerializeField] private float autoBrakes = 1f;
    // [Range(0, 1)] [SerializeField] private float crouchSpeed = .36f;  // Amount of maxHSpeed applied to crouching movement. 1 = 100%
    private float hVelocity;
    private float vVelocity; 
    private float maxH; 
    private float minH; 
    private float maxV; 
    private float minV; 

    // Reference Variables
    private PlayerInput pInput;
    private PlayerConditions pCon; 
    private Rigidbody2D rb; 

    private void Awake(){
    	rb = GetComponent<Rigidbody2D>();
        pInput = GetComponent<PlayerInput>(); 
        pCon = GetComponent<PlayerConditions>(); 
    }

    public void RunJump(){
    	// horizontal force
        float hForce = pInput.h * accelSpeed; 

        // wall slide-down
        bool wallDrag = false; 
        if( !pCon.Grounded ){
            if( (pCon.WalledRight && pInput.h > 0.5f)
                || (pCon.WalledLeft && pInput.h < -0.5f) ){

                wallDrag = true;  
            }
        }
        
        // vertical foce
        float vForce = 0; 
        // jump launch logic
        if( pCon.JumpPhase == PlayerConditions.Jump.WaitForLiftoff ){
            // wall jump bump 
            if( !pCon.Grounded ){
                if( pCon.WalledLeft ){ hForce += 200; }
                else if( pCon.WalledRight ){ hForce += -200; }
            }
            // launch jump
            vForce += jumpForce; 
            pCon.liftoffGranted = true; 
        }

        // Unadjusted Velocities
        hVelocity = rb.velocity.x + hForce; 
        vVelocity = rb.velocity.y + vForce; 

        // Velocity limiters
        maxH = maxHSpeed;
        minH = maxHSpeed * -1; 
        maxV = maxVSpeed; 
        minV = maxVSpeed * -1; 
        // stop at wall
        if( pCon.WalledLeft || pCon.WalledRight ){
            if( pCon.WalledRight ){ maxH = 0; }
            if( pCon.WalledLeft ){ minH = 0; }
        // horizontal auto-brakes
        }else{
            if( pCon.Grounded ){
                if( pInput.h < 0.5f && pInput.h > -0.5f ){
                    maxH = autoBrakes; 
                    minH = autoBrakes * -1; 
                }else if( pInput.h > 0 ){
                    hVelocity = Mathf.Max(autoBrakes * -1, hVelocity);
                }else if( pInput.h < 0 ){ 
                    hVelocity = Mathf.Min(autoBrakes, hVelocity); 
                }
            }
        }
        // jump release
        if( pCon.JumpPhase == PlayerConditions.Jump.Jumping_ButtonReleased ){
            maxV = jumpReleaseLimit; 
        }
        // wall drag
        if( wallDrag ){ minV = wallSlideMax; } 

        // set final velocity
        hVelocity = Mathf.Clamp(hVelocity, minH, maxH); 
        vVelocity = Mathf.Clamp(vVelocity, minV, maxV);
        rb.velocity = new Vector2(hVelocity, vVelocity);
    }
}
