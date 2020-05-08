﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCState : MonoBehaviour {

	// Flags
	[SerializeField] private bool airborn;
	[SerializeField] private bool grounded; 
	[SerializeField] private bool walledLeft;
	[SerializeField] private bool walledRight; 
	[SerializeField] private bool walled; 

	// Properties
	public bool Airborn { get{return airborn;} }
	public bool Grounded { get{return grounded;} }
	public bool WalledLeft { get{return walledLeft;} }
	public bool WalledRight { get{return walledRight;} }
	public bool Walled { get{return walled;} }

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


	// [[ ----- UPDATE ----- ]]
	public void StateUpdate(){

		// set airborn flag (based on grounded)
		if( !grounded && !airborn ){ airborn = true; }
		else if( grounded && airborn ){ airborn = false; }
	}


	// [[ ----- FIXED UPDATE ----- ]]
	public void StateFixedUpdate(){

		// set grounded flag
		grounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            groundCheck.position, groundedRadius, whatIsSolid);
        for (int i = 0; i < colliders.Length; i++){
            if (colliders[i].gameObject != gameObject){ 
                grounded = true;  
            }
        }

		// set walled flags	
		walledLeft = false;
		walledRight = false; 
		walled = false; 
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