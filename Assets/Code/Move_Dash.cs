using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerConditions))]
[RequireComponent(typeof(Rigidbody2D))]
public class Move_Dash : MonoBehaviour {
    
    // Variables
    [SerializeField] private float dist = 3f; 
    [SerializeField] private float dashTime = 0.3f; 
    [SerializeField] private LayerMask dashMask; 
	private float dashTimer; 
	private bool dashLock; 
    private Vector2 castDir; 
    private Vector2 startPoint; 
    private Vector2 hitPoint; // used as target for ending the dash
	private Vector2 endPoint; // used to determine speed of lerp, based on no collisions
    private bool wallHit; 
    private bool targetReached; 
	private float colliderRadius; 
    
    // Reference Variables
    public CircleCollider2D bodyCollider; 
    private PlayerInput pInput; 
    private PlayerConditions pCon; 
    private Rigidbody2D rb;

    
    public void Awake(){
    	pInput = GetComponent<PlayerInput>();
    	pCon = GetComponent<PlayerConditions>();
    	colliderRadius = (bodyCollider.bounds.size.x / 2f) + 0.2f; 
    	rb = GetComponent<Rigidbody2D>(); 
    }


    public bool Dash(){
        // if dash hasn't started, initiate
        if( !dashLock ){
            /*Debug.Log("DASHING" + null);*/

    		// determine hit point / end point
            wallHit = false; 
            if( pInput.h > 0.05f ){ castDir = Vector2.right; }
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
    		dashLock = true;
            targetReached = false; 
            dashTimer = dashTime * 0.33f; // add a little skip to the dash  
        }

        // if dash started - determine point lerp
        else if( !targetReached ){
            /*Debug.Log("LERPING" + null);*/
            dashTimer += Time.fixedDeltaTime;

            // set position 
            float ratio = dashTimer / dashTime; 
            transform.position = Vector2.Lerp(startPoint, endPoint, ratio);
            /*Debug.Log("pos x: " + transform.position.x + " hit x: " + hitPoint.x); */
            if( (castDir.x > 0 && transform.position.x >= hitPoint.x)
                || (castDir.x < 0 && transform.position.x <= hitPoint.x)  ){
                targetReached = true; 
                transform.position = hitPoint;
            }else if( ratio >= 1 ){
                Debug.LogError("Dash stuck, time exit:\n");
                Debug.LogError("pos x: " + transform.position.x + " hit x: " + hitPoint.x); 
                targetReached = true; 
            }
        }

        else{
            /*Debug.Log("DONE" + null);*/
            dashLock = false; 
            pCon.dashing = false;
            return false;  
        }

        return true; 
    }
}
