using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Duck : MoveBehavior {

	// Variables
    [SerializeField] private bool isActive;
    [SerializeField] private float maxHSpeed = 2f;

    // Reference Variables
    public CircleCollider2D standingCollider;
    public CircleCollider2D crouchingCollider; 
	private PCInput pcInput; 
	private PCState pcState;

	// [[ ----- AWAKE ----- ]]
	private void Awake(){
		pcInput = GetComponent<PCInput>();
		pcState = GetComponent<PCState>();
	}


// -----------------------------------------------------------------------------
// MoveBehavoir

    public override void Init( bool _override ){
    	isActive = false; 
    	if( pcState.Grounded && pcInput.DownButton ){ isActive = true; }

    	// set colliders
    	if( isActive ){
    		if( standingCollider.gameObject.activeSelf ){ 
    			standingCollider.gameObject.SetActive(false);
    		}
    		if( !crouchingCollider.gameObject.activeSelf ){
    			crouchingCollider.gameObject.SetActive(true); 
    		}
    	}else{
    		if( !standingCollider.gameObject.activeSelf ){ 
    			standingCollider.gameObject.SetActive(true);
    		}
    		if( crouchingCollider.gameObject.activeSelf ){
    			crouchingCollider.gameObject.SetActive(false); 
    		}
    	}
    }

    public override bool AffectsHLimits(){ return isActive; }

    public override Vector2 GetHLimits(){
    	return new Vector2( (maxHSpeed * -1), maxHSpeed ); 
    }
}
