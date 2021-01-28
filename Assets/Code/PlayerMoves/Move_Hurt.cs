using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Hurt : MoveBehavior {

	// Variables
	[SerializeField] private bool isActive;
	private bool moveArmed; 
	private bool moveSpent;
	private bool overridden;
	public float knockbackForce = 4f; 
	public float knockupForce = 4f; 
	public float knockbackTime = 0.5f;
	private float timer; 

	// Reference Variables
	private PCState pcState;


	private void Awake(){
		pcState = GetComponent<PCState>();
	}

	private void FixedUpdate(){
		if( isActive ){
			timer -= Time.deltaTime;
			if( timer < 0 ){ isActive = false; }
		}
	}

// -----------------------------------------------------------------------------
// MoveBehavior

	// [[ ----- INIT ----- ]]
	public override void Init( bool _overridden ){
		overridden = _overridden;
		if( overridden ){ isActive = false; }
		else if( !isActive && !moveSpent && pcState.Hurt ){
			isActive = true; 
			moveArmed = true; 
			moveSpent = true; 
			timer = knockbackTime; 
		}else if( moveArmed ){ 
			moveArmed = false; 
			moveSpent = true;
		}

		// reset move once hurt state is over
		if( !isActive && !pcState.Hurt ){ moveSpent = false; }
	}

	// [[ ----- IS EXCLUSIVE ----- ]]
	public override bool IsExclusive(){ return isActive; }

	// [[ ----- ZERO MOVEMENT ----- ]]
	public override bool ZeroMovement(){ return (isActive && moveArmed); }

	// [[ ----- AFFECTS FORCE ----- ]]
	public override bool AffectsForce(){ return (isActive && moveArmed); }

	// [[ ----- GET FORCE ----- ]]
	public override Vector2 GetForce(){
		return new Vector2(knockbackForce, knockupForce); 
	}

}
