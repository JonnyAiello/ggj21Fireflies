using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCMove : MonoBehaviour{

	// Flags
	[SerializeField] private bool moving;
	[SerializeField] private bool jumping; 
	[SerializeField] private bool dashing;
	
	// Properties
	public bool Moving { get{return moving;} }
	public bool Jumping { get{return jumping;} }
	public bool Dashing { get{return dashing;} }

	// Variables
	[SerializeField] private float maxHSpeed = 6f;
    [SerializeField] private float maxVSpeed = 15f;

	// Reference Variables
	private Move_Run run; 	
	private Rigidbody2D rb; 

	private void Awake(){
		run = GetComponent<Move_Run>(); 
		rb = GetComponent<Rigidbody2D>(); 
	}


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
		rb.velocity = rb.velocity + addativeForce; 

		// set limits
		Vector2 runHorizLimits = run.GetHorizontalLimits();
		horizLimits.x = Mathf.Max( horizLimits.x, runHorizLimits.x);
		horizLimits.y = Mathf.Min( horizLimits.y, runHorizLimits.y);  
		float cappedX = Mathf.Clamp(rb.velocity.x, horizLimits.x, horizLimits.y);

		rb.velocity = new Vector2(cappedX, rb.velocity.y);   
	}
    
}
