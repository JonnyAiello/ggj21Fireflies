using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Dash : MonoBehaviour {

	// Variables
	[SerializeField] private float dist = 3f; 
    [SerializeField] private float dashDuration = 0.3f; 
    [SerializeField] private float cooldownDuration = 0.6f; 
    [SerializeField] private LayerMask dashMask; 
	private bool active; 
	private bool initialized; 
	private float dashTimer;
	private float cooldownTimer; 
	private float colliderRadius; 

	private Vector2 castDir; 
    private Vector2 startPoint; 
    private Vector2 hitPoint; // used as target for ending the dash
	private Vector2 endPoint; // used to determine speed of lerp, based on no collisions
    private bool wallHit; 
    private bool targetReached; 

	// Reference Variables
	private PCInput pcInput; 
	private PCState pcState;
    private PCMove pcMove; 
    private CircleCollider2D bodyCollider;  

	private void Awake(){
		pcInput = GetComponent<PCInput>();
		pcState = GetComponent<PCState>(); 
        pcMove = GetComponent<PCMove>();
        bodyCollider = GetComponent<CircleCollider2D>(); 
        colliderRadius = (bodyCollider.bounds.size.x / 2f) + 0.2f; 

	}

	// On Draw Gizmos
	private void OnDrawGizmos(){
		/*
		if( hitPoint != Vector2.zero ){
			Gizmos.color = Color.green; 
			Gizmos.DrawSphere(hitPoint, .2f);
		}
		*/
	}

	public bool IsActive(){
		if( active ){ return true; }
    	else{
    		cooldownTimer += Time.fixedDeltaTime; 
    		if( pcInput.DashButton 
    			&& pcMove.Moving
    			&& cooldownTimer > cooldownDuration ){
    			
    			cooldownTimer = 0;  
	    		active = true; 
	    		return true; 
	    	}
    	}
       	
        active = false;
    	return false; 
    }


    public Vector2 GetPosition(){
    	Vector2 setToPosition = transform.position; 

    	if( !initialized ){
	    	// determine hit point / end point
	        wallHit = false; 
	        if( pcInput.RightButton ){ castDir = Vector2.right; }
	        else{ castDir = Vector2.left; }
	        startPoint = transform.position;
			endPoint = (Vector2)transform.position + (castDir * dist);

			// raycast
    		RaycastHit2D hit = Physics2D.Raycast(
                transform.position, castDir, 3f, dashMask);

    		// if collided with wall
    		if( hit.collider != null ){ 
                wallHit = true; 
    			hitPoint = hit.point; 
    			// set beside raycast hit object
    			if( castDir == Vector2.right ){ 
    				hitPoint += new Vector2(colliderRadius * -1, 0); 
    			}else{ hitPoint += new Vector2(colliderRadius, 0); }
            }else{
                hitPoint = endPoint; 
            }
    		
            // init dash variables
    		initialized = true;
            targetReached = false; 
            dashTimer = dashDuration * 0.25f; 
            	// add a little skip to the beginning of dash  
		}

	    // if dash started - determine point lerp
	    if( !targetReached ){
	    	dashTimer += Time.fixedDeltaTime;

            // set position 
            float ratio = dashTimer / dashDuration; 
            setToPosition = Vector2.Lerp(startPoint, endPoint, ratio);
            if( (castDir.x > 0 && setToPosition.x >= hitPoint.x)
                || (castDir.x < 0 && setToPosition.x <= hitPoint.x) ){

                targetReached = true; 
                setToPosition = hitPoint;
            }else if( ratio >= 1 ){
                Debug.LogError("Dash stuck, time exit:\n");
                Debug.LogError("pos x: " + transform.position.x + " hit x: " + hitPoint.x); 
                targetReached = true; 
            }


	    // end the move
	    }else{
	    	initialized = false; 
	    	active = false; 
	    }

	    return setToPosition; 
    }

}
