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
	private Move_Run run; 	
	private Move_Jump jump; 
	private Move_WallSlide wallSlide; 
	private Rigidbody2D rb; 
	private PCInput pcInput; 

	private void Awake(){
		run = GetComponent<Move_Run>(); 
		jump = GetComponent<Move_Jump>(); 
		wallSlide = GetComponent<Move_WallSlide>(); 
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
		Vector2 addativeForce = Vector2.zero; 
		Vector2 horizLimits = new Vector2(maxHSpeed * -1, maxHSpeed); 
		Vector2 vertLimits = new Vector2(maxVSpeed * -1, maxVSpeed);

		// make active moves list

		// add forces from moves
		moving = run.IsActive();
		if( moving ){
			addativeForce += run.GetForces();
		}
		if( jump.IsActive() ){
			addativeForce += jump.GetForces(); 
		}
		wallSliding = wallSlide.IsActive();
		

		rb.velocity = rb.velocity + addativeForce; 

		// set limits
		Vector2 runHorizLimits = run.GetHorizontalLimits();
		/*Debug.Log("Horiz Min Limit: " + runHorizLimits.x);
		Debug.Log("Horiz Max Limit: " + runHorizLimits.y);*/
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

		// reset inputs
		pcInput.ResetInputs();
	}

	// [[ ----- SET JUMPING ----- ]]
	public void SetJumping( bool _val ){ jumping = _val; }
    
}
