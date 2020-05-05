using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCMove : MonoBehaviour{

	// Flags
	[SerializeField] private bool moving;
	[SerializeField] private bool jumping; 
	[SerializeField] private bool dashing;
	[SerializeField] private Vector2 rbVelocity; 
	
	// Variables
	[SerializeField] private float maxHSpeed = 6f;
    [SerializeField] private float maxVSpeed = 15f;

	// Reference Variables
	private Move_Run run; 	
	private Move_Jump jump; 
	private Rigidbody2D rb; 
	private PCInput pcInput; 

	private void Awake(){
		run = GetComponent<Move_Run>(); 
		jump = GetComponent<Move_Jump>(); 
		rb = GetComponent<Rigidbody2D>();
		pcInput = GetComponent<PCInput>();  
	}

	// Properties
	public bool Moving { get{return moving;} }
	public bool Jumping { get{return jumping;} }
	public bool Dashing { get{return dashing;} }
	public float MaxHSpeed { get{return maxHSpeed;} }
	public float MaxVSpeed { get{return maxVSpeed;} }


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
		float cappedY = Mathf.Clamp( rb.velocity.y, vertLimits.x, vertLimits.y);

		// create final vector
		rb.velocity = new Vector2(cappedX, cappedY);
		rbVelocity = rb.velocity;    

		// reset inputs
		if( pcInput.JumpButton && !Input.GetButton("Jump") ){
            pcInput.ResetInputJump(); 
        }
	}

	// [[ ----- SET JUMPING ----- ]]
	public void SetJumping( bool _val ){ jumping = _val; }
    
}
