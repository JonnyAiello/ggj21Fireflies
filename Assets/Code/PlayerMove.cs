using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Rigidbody settings: 
Freeze rotation: z
mass: 1
gravity scale: 3 (default 2d gravity of -9.51)
*/

[RequireComponent(typeof(Move_RunJump))]
[RequireComponent(typeof(Move_Dash))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour {

    // Variables
    [SerializeField] private State state = State.Free;
    [SerializeField] private float hForce;
    [SerializeField] private float vForce;
    [SerializeField] private float hVelocity;
    [SerializeField] private float vVelocity;
    [SerializeField] private float maxH;
    [SerializeField] private float minH;
    [SerializeField] private float maxV;
    [SerializeField] private float minV;


    // Reference Variables
    private PlayerInput pInput;
    private PlayerConditions pCon; 
    private Move_RunJump mRunJump; 
    private Move_Dash mDash; 
    private Rigidbody2D rb; 

    // Enums
    public enum State{
        Free,
        Dashing,
        Hurt
    }

    private void Awake(){
    	rb = GetComponent<Rigidbody2D>();
        pInput = GetComponent<PlayerInput>(); 
        pCon = GetComponent<PlayerConditions>(); 
        mRunJump = GetComponent<Move_RunJump>(); 
        mDash = GetComponent<Move_Dash>(); 
    }

    // Called during Player.FixedUpdate()
    public void Move(){
        
        // apply last frame's velocity
        hVelocity = rb.velocity.x;
        vVelocity = rb.velocity.y;

        // apply move physics - state machine
        switch( state ){

            case State.Free:
                if( pCon.dashing ){ state = State.Dashing; }
                else{
                    mRunJump.RunJump(); 
                }    
                break;
            
            case State.Dashing:
                if( !mDash.Dash() ){ state = State.Free; }
                break;

            case State.Hurt:
                break;
            default:
                Debug.Log("switch: value match not found");
                break;
        }

        
/*
        // set final velocity
        hVelocity = Mathf.Clamp(hVelocity, minH, maxH); 
        vVelocity = Mathf.Clamp(vVelocity, minV, maxV);
        rb.velocity = new Vector2(hVelocity, vVelocity);
*/
        ReleaseInputs();
    }

    private void ReleaseInputs(){
        if( pInput.jumpButton && !Input.GetButton("Jump") ){
            pInput.jumpButton = false;  
        }
        if( pInput.dashButton && !Input.GetButton("Fire1") ){
            pInput.dashButton = false;  
        }
    }
}
