using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
Moves at the top of the move list get first priority to be exclusive - once an
exclusive move is processesed, the rest of the list is ignored. Because of this
high-priority moves should be placed at the top, in order of preceedence
*/


public class PCMove : MonoBehaviour{

	// Flags
	[SerializeField] private Vector2 rbVelocity; 
	
	// Variables
	[SerializeField] private float maxHSpeed = 6f;
    [SerializeField] private float maxVSpeed = 15f;

	// Reference Variables
	[SerializeField] private List<MoveBehavior> moveList;
	private Move_Run run; 	
	private Move_Jump jump; 
	private Move_WallSlide wallSlide; 
	private Move_Dash dash; 
	private Rigidbody2D rb; 
	private PCInput pcInput; 

	// Properties
	public float MaxHSpeed { get{return maxHSpeed;} }
	public float MaxVSpeed { get{return maxVSpeed;} }
	public float VelocityX { get{return rbVelocity.x;} }
	public float VelocityY { get{return rbVelocity.y;} }


	private void Awake(){
		run = GetComponent<Move_Run>(); 
		jump = GetComponent<Move_Jump>(); 
		wallSlide = GetComponent<Move_WallSlide>(); 
		dash = GetComponent<Move_Dash>(); 
		rb = GetComponent<Rigidbody2D>();
		pcInput = GetComponent<PCInput>();  
	}

	// [[ ----- MOVE FIXED UPDATE ----- ]]
	public void MoveFixedUpdate(){
		bool overrideMove = false; 
		bool zeroMovement = false;
		Vector2 addativeForce = Vector2.zero; 
		Vector2 horizLimits = new Vector2(maxHSpeed * -1, maxHSpeed); 
		Vector2 vertLimits = new Vector2(maxVSpeed * -1, maxVSpeed);
		rbVelocity = rb.velocity; 

		// process all attached moves
		foreach( MoveBehavior mb in moveList ){
			mb.Init( overrideMove );
			if( mb.IsExclusive() ){ overrideMove = true; }
			if( !zeroMovement && mb.ZeroMovement() ){ zeroMovement = true; }
			if( mb.AffectsForce() ){ addativeForce += mb.GetForce(); }
			if( mb.AffectsHLimits() ){
				Vector2 newHLimits = mb.GetHLimits();
				horizLimits.x = Mathf.Max( horizLimits.x, newHLimits.x);
				horizLimits.y = Mathf.Min( horizLimits.y, newHLimits.y); 
			}
			if( mb.AffectsVLimits() ){
				Vector2 newVLimits = mb.GetVLimits(); 
				vertLimits.x = Mathf.Max( vertLimits.x, newVLimits.x ); 
				vertLimits.y = Mathf.Min( vertLimits.y, newVLimits.y ); 
			}
			if( mb.AffectsPosition() ){transform.position = mb.GetPosition();}
		}

		if( zeroMovement ){ rbVelocity = Vector2.zero; }

		// add forces, apply limits
		rbVelocity += addativeForce; 
		float cappedX = Mathf.Clamp(rbVelocity.x, horizLimits.x, horizLimits.y);
		float cappedY = Mathf.Clamp( rbVelocity.y, vertLimits.x, vertLimits.y);

		rbVelocity = new Vector2(cappedX, cappedY); 
		rb.velocity = rbVelocity; 

		// reset inputs
		pcInput.ResetInputs();
	}
}
