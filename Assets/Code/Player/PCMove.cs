using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCMove : MonoBehaviour{

	// Flags
	[SerializeField] private bool moving;
	[SerializeField] private bool jumping; 
	[SerializeField] private bool dashing;
	[SerializeField] private bool wallSliding;
	[SerializeField] private Vector2 rbVelocity; 
	
	// Variables
	[SerializeField] private float maxHSpeed = 6f;
    [SerializeField] private float maxVSpeed = 15f;
    /*[SerializeField] private float inputThreshold = 0.2f;
    [SerializeField] private float inputReleaseThreshold = 0.6f; 
    	// the minimum axis input to recognize from joystick/keyboard
*/

	// Reference Variables
	[SerializeField] private List<MoveBehavior> moveList;
	private Move_Run run; 	
	private Move_Jump jump; 
	private Move_WallSlide wallSlide; 
	private Move_Dash dash; 
	private Rigidbody2D rb; 
	private PCInput pcInput; 

	private void Awake(){
		run = GetComponent<Move_Run>(); 
		jump = GetComponent<Move_Jump>(); 
		wallSlide = GetComponent<Move_WallSlide>(); 
		dash = GetComponent<Move_Dash>(); 
		rb = GetComponent<Rigidbody2D>();
		pcInput = GetComponent<PCInput>();  
	}

	// Properties
	public bool Moving { get{return moving;} }
	public bool Jumping { get{return jumping;} }
	public bool Dashing { get{return dashing;} }
	public bool WallSliding { get{return wallSliding;} }
	public float MaxHSpeed { get{return maxHSpeed;} }
	public float MaxVSpeed { get{return maxVSpeed;} }
/*	public float InputThresh { get{return inputThreshold;} }
	public float InputReleaseThresh { get{return inputReleaseThreshold;} }*/
	public float VelocityX { get{return rbVelocity.x;} }
	public float VelocityY { get{return rbVelocity.y;} }


	// [[ ----- MOVE FIXED UPDATE ----- ]]
	public void MoveFixedUpdate(){
		bool zeroMovement = false;
		Vector2 addativeForce = Vector2.zero; 
		Vector2 horizLimits = new Vector2(maxHSpeed * -1, maxHSpeed); 
		Vector2 vertLimits = new Vector2(maxVSpeed * -1, maxVSpeed);
		rbVelocity = rb.velocity; 

		// process all attached moves
		foreach( MoveBehavior mb in moveList ){
			mb.Init();
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
			if( mb.IsExclusive() ){ break; }
		}

		if( zeroMovement ){ rbVelocity = Vector2.zero; }

		// add forces, apply limits
		rbVelocity += addativeForce; 
		float cappedX = Mathf.Clamp(rbVelocity.x, horizLimits.x, horizLimits.y);
		float cappedY = Mathf.Clamp( rbVelocity.y, vertLimits.x, vertLimits.y);

		rbVelocity = new Vector2(cappedX, cappedY); 
		rb.velocity = rbVelocity; 

/*
		// add forces from moves
		moving = run.IsActive();
		if( moving ){
			addativeForce += run.GetForces();
		}
		if( jump.IsActive() ){
			addativeForce += jump.GetForces(); 
		}
		wallSliding = wallSlide.IsActive();
		dashing = dash.IsActive(); 
		

		rb.velocity = rb.velocity + addativeForce; 

		// set limits
		Vector2 runHorizLimits = run.GetHorizontalLimits();
		horizLimits.x = Mathf.Max( horizLimits.x, runHorizLimits.x);
		horizLimits.y = Mathf.Min( horizLimits.y, runHorizLimits.y);  
		float cappedX = Mathf.Clamp(rb.velocity.x, horizLimits.x, horizLimits.y);

		Vector2 jumpVertLimits = jump.GetVerticalLimits();
		vertLimits.x = Mathf.Max( vertLimits.x, jumpVertLimits.x ); 
		vertLimits.y = Mathf.Min( vertLimits.y, jumpVertLimits.y ); 
		

		if( wallSliding ){
			Vector2 wallSlideLimits = wallSlide.GetVerticalLimits(); 
			vertLimits.x = Mathf.Max( vertLimits.x, wallSlideLimits.x ); 
			vertLimits.y = Mathf.Min( vertLimits.y, wallSlideLimits.y );
		}

		float cappedY = Mathf.Clamp( rb.velocity.y, vertLimits.x, vertLimits.y);

		// create final vector
		rb.velocity = new Vector2(cappedX, cappedY);
		rbVelocity = rb.velocity;    

		// set position (if move sets position directly)
		if( dashing ){ transform.position = dash.GetPosition(); }
		
		*/
		// reset inputs
		pcInput.ResetInputs();
	}

	// [[ ----- SET JUMPING ----- ]]
	public void SetJumping( bool _val ){ jumping = _val; }
    
}
