using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCState : MonoBehaviour {

	// Flags
	[SerializeField] private bool grounded; 
	[SerializeField] private bool ceilinged; 
	[SerializeField] private bool walledLeft;
	[SerializeField] private bool walledRight; 
	[SerializeField] private bool walled; 

	// Properties
	public bool Grounded { get{return grounded;} }
	public bool Ceilinged { get{return ceilinged;} }
	public bool WalledLeft { get{return walledLeft;} }
	public bool WalledRight { get{return walledRight;} }
	public bool Walled { get{return walled;} }
	public bool MovingHoriz { get; set; }
	public bool Ducked { get; set; }

	// Variables
	const float groundedRadius = .2f;
	const float contactRadius = .1f;

	// Reference Variables
	[SerializeField] private LayerMask whatIsSolid;
	[SerializeField] private bool gizmosOn; 
	public Transform groundCheck; 
	public Transform ceilingCheck;
	public Transform rightCheck;
	public Transform leftCheck;


	private void OnDrawGizmos(){
		// Debug.Log("TESTING");
		// Gizmos.DrawSphere(ceilingCheck.position, contactRadius); 
	}

	private bool IsPlayerCollider( Collider2D _c ){
		if( _c.name == "Collider_standing"
			|| _c.name == "Collider_crouching" ){

			return true;
		}
		return false; 
	}

	// [[ ----- UPDATE ----- ]]
	public void StateUpdate(){

		
	}


	// [[ ----- FIXED UPDATE ----- ]]
	public void StateFixedUpdate(){

		// set grounded flag
		grounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            groundCheck.position, groundedRadius, whatIsSolid);
        for (int i = 0; i < colliders.Length; i++){
            if( !IsPlayerCollider(colliders[i]) ){ 
                grounded = true;  
            }
        }

        // set ceilinged flag
        ceilinged = false;
        colliders = Physics2D.OverlapCircleAll(
            ceilingCheck.position, contactRadius, whatIsSolid);
        for (int i = 0; i < colliders.Length; i++){
            if( !IsPlayerCollider(colliders[i]) ){
                ceilinged = true;  
            }
        }

		// set walled flags	
		walledLeft = false;
		walledRight = false; 
		walled = false; 
		colliders = Physics2D.OverlapCircleAll(
        	rightCheck.position, contactRadius, whatIsSolid);
        for (int i = 0; i < colliders.Length; i++){
        	if( !IsPlayerCollider(colliders[i]) ){ walledRight = true; }
		}
		colliders = Physics2D.OverlapCircleAll(
        	leftCheck.position, contactRadius, whatIsSolid);
        for (int i = 0; i < colliders.Length; i++){
        	if( !IsPlayerCollider(colliders[i]) ){ walledLeft = true; }
		}
		if( walledLeft || walledRight ){ walled = true; }
		else{ walled = false; }

	}

// -----------------------------------------------------------------------------
// Physics Events

	private void OnCollisionEnter2D( Collision2D c ){
        if( c.gameObject.layer == LayerMask.NameToLayer("Solids") ){
        	/*
            // check grounded on collision
            Collider2D[] colliders = Physics2D.OverlapCircleAll(
                groundCheck.position, groundedRadius, whatIsSolid);
            for (int i = 0; i < colliders.Length; i++){
                if (colliders[i].gameObject != gameObject){ 
                    grounded = true;  
                    // Debug.Log("Grounded" + null);
                }
            }*/
        }
    }

/*
    private void OnCollisionExit2D( Collision2D c ){
        if( c.gameObject.layer == LayerMask.NameToLayer("Solids") ){
            // check grounded off collision
            grounded = false;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(
                groundCheck.position, groundedRadius, whatIsSolid); 
            for (int i = 0; i < colliders.Length; i++){
                if (colliders[i].gameObject != gameObject){ 
                    grounded = true;  
                }
            }
            // if( !grounded ){Debug.Log("NOT Grounded" + null);}
        }
    }
 */  
}
