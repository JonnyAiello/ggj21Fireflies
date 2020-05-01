using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerConditions : MonoBehaviour {

	// Player Flags
	[SerializeField] private bool grounded;
	[SerializeField] private bool walledLeft; 
	[SerializeField] private bool walledRight;
	[SerializeField] private Jump jumpPhase; 
/*    [SerializeField] private Dash dashPhase; */

	// Properties
	public bool Grounded { get{return grounded;} } 
	public bool WalledLeft { get{return walledLeft;} }
	public bool WalledRight { get{return walledRight;} }
	public Jump JumpPhase { get{return jumpPhase;} }

	// Variables
	[SerializeField] private LayerMask whatIsSolid;
	[SerializeField] private bool gizmosOn; 
	const float groundedRadius = .2f;
	const float contactRadius = .15f;
	private float escapeTimer = 0f; 
	private float escapeDelay = 0.1f; 
    private float dashCooldown = 0; 

    // move flags
	public bool liftoffGranted = false; 
    public bool dashing = false; 

	// Reference Variables
	private Player p; 
	private PlayerInput pInput; 
	public Transform groundCheck; 
	public Transform ceilingCheck;
	public Transform rightCheck;
	public Transform leftCheck;

	// Enums
	public enum Jump{
		Landed_ButtonReleased,
		WaitForLiftoff,
		Landed_ButtonHeld,
		Jumping_EscapeVelocity, 
		Jumping_ButtonHeld,
		Jumping_ButtonReleased,
		Freefall
	}

    /*
    public enum Dash{
        Starting,
        Moving,
        Ending,
        Cooldown,
        Ready
    }
    */

	// Awake
	private void Awake(){
		p = GetComponent<Player>(); 
		pInput = GetComponent<PlayerInput>();
		jumpPhase = Jump.Landed_ButtonReleased; 
	}

	// On Draw Gizmos
	private void OnDrawGizmos(){
		if( gizmosOn ){
			Gizmos.color = Color.green; 
			Gizmos.DrawSphere(groundCheck.position, groundedRadius);
			Gizmos.DrawSphere(ceilingCheck.position, contactRadius);
			Gizmos.DrawSphere(rightCheck.position, contactRadius);
			Gizmos.DrawSphere(leftCheck.position, contactRadius); 
		}

	}

	// Update Flags - called during Player.FixedUpdate()
    public void UpdateFlags(){

    	// grounded flag
        grounded = false; 
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
        	groundCheck.position, groundedRadius, whatIsSolid);
        for (int i = 0; i < colliders.Length; i++){
        	if (colliders[i].gameObject != gameObject){ 
        		grounded = true;  
        	}
		}

		// set move states
		SetJumpPhase();
        SetDashing(); 

		// walled flags
		walledRight = false;
		walledLeft = false; 
		colliders = Physics2D.OverlapCircleAll(
        	rightCheck.position, contactRadius, whatIsSolid);
        for (int i = 0; i < colliders.Length; i++){
        	if (colliders[i].gameObject != gameObject){ walledRight = true; }
		}
		colliders = Physics2D.OverlapCircleAll(
        	leftCheck.position, contactRadius, whatIsSolid);
        for (int i = 0; i < colliders.Length; i++){
        	if (colliders[i].gameObject != gameObject){ walledLeft = true; }
		}
    }


    private void SetDashing(){
        dashCooldown += Time.fixedDeltaTime; 
        if( !dashing
            && (pInput.h > 0.05f || pInput.h < -0.05f)
            && pInput.dashButton 
            && dashCooldown > 1f){

            dashing = true; 
            dashCooldown = 0; 
        }
    }

    // Jump state machine
    private void SetJumpPhase(){
    	switch( jumpPhase ){

    		case Jump.Landed_ButtonReleased:
    			if( !grounded ){ jumpPhase = Jump.Freefall; }
    			else if( pInput.jumpButton ){
    				jumpPhase = Jump.WaitForLiftoff;
    			}
    			/*Debug.Log("switch: jumpPhase = " + Jump.Landed_ButtonReleased);*/
    			break;
    		
    		// wait for PlayerMove to apply the jump force to pc
			case Jump.WaitForLiftoff:
    			if( liftoffGranted ){
    				jumpPhase = Jump.Jumping_EscapeVelocity; 
    				liftoffGranted = false; 
    			}
    			/*Debug.Log("switch: jumpPhase = " + Jump.WaitForLiftoff);*/
    			break;

    		// creates timegap so we escape the ground check
    		case Jump.Jumping_EscapeVelocity:
    			escapeTimer += Time.deltaTime;
    			if( escapeTimer > escapeDelay ){
    				if( pInput.jumpButton ){jumpPhase = Jump.Jumping_ButtonHeld;}
    				else{jumpPhase = Jump.Jumping_ButtonReleased;}
    				escapeTimer = 0; 
    			}
    			/*Debug.Log("switch: jumpPhase = " + Jump.Jumping_EscapeVelocity);*/
    			break;

    		case Jump.Jumping_ButtonHeld:
    			if( grounded ){ jumpPhase = Jump.Landed_ButtonHeld; }
    			else if( !pInput.jumpButton ){
    				jumpPhase = Jump.Jumping_ButtonReleased;
    			}
    			/*Debug.Log("switch: jumpPhase = " + Jump.Jumping_ButtonHeld);*/
    			break;
    		
    		case Jump.Jumping_ButtonReleased:
    			// wall jump
    			if( (walledLeft || walledRight) && pInput.jumpButton ){
    				jumpPhase = Jump.WaitForLiftoff; 
    			
    			}else if( grounded ){
    				if( pInput.jumpButton ){jumpPhase = Jump.Landed_ButtonHeld;}
    				else{ jumpPhase = Jump.Landed_ButtonReleased; }
    			}
    			/*Debug.Log("switch: jumpPhase = " + Jump.Jumping_ButtonReleased);*/
    			break;
    		
    		// must release jump before next jump begins
    		case Jump.Landed_ButtonHeld:
    			if( !grounded ){ jumpPhase = Jump.Freefall; }
    			if( !pInput.jumpButton ){jumpPhase = Jump.Landed_ButtonReleased;}
    			/*Debug.Log("switch: jumpPhase = " + Jump.Landed_ButtonHeld);*/
    			break;

    		// if pc steps off or airborn not from jumping
    		case Jump.Freefall:
    			if( (walledLeft || walledRight) && pInput.jumpButton ){
    				jumpPhase = Jump.WaitForLiftoff; 
    			}
    			if( grounded ){
    				if( pInput.jumpButton ){jumpPhase = Jump.Landed_ButtonHeld;}
    				else{jumpPhase = Jump.Landed_ButtonReleased;}
    			}
    			/*Debug.Log("switch: jumpPhase = " + Jump.Freefall);*/
    			break;
    		
    		default:
    			Debug.Log("switch: value match not found");
    			break;
    	}
    }

}
