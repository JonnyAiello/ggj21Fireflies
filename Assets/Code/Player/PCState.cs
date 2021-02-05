using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCState : MonoBehaviour {

	// Flags
	[SerializeField] private bool grounded; 
	[SerializeField] private bool ceilinged; 
	[SerializeField] private bool walledLeft;
	[SerializeField] private bool walledRight; 
	[SerializeField] private bool walled; 
	[SerializeField] private bool ledgeClimbableRight;
	[SerializeField] private bool ledgeClimbableLeft;
	[SerializeField] private bool ledgeClimbable;	
	[SerializeField] private bool movingHoriz;
	[SerializeField] private bool downwardFall;

	// Properties
	public bool Grounded { get{return grounded;} }
	public bool Ceilinged { get{return ceilinged;} }
	public bool WalledLeft { get{return walledLeft;} }
	public bool WalledRight { get{return walledRight;} }
	public bool Walled { get{return walled;} }
	public bool LedgeClimbableRight { get{return ledgeClimbableRight;} }
	public bool LedgeClimbableLeft { get{return ledgeClimbableLeft;} }
	public bool LedgeClimbable { get{return ledgeClimbable;} }	
	public bool MovingHoriz { get{return (MoveRun || MoveWalk);} }
	public bool Running { get; set; }
	public bool Ducked { get; set; }

	public bool MoveRun { get; set; }
	public bool MoveWalk { get; set; }
	public bool Hurt { get; set; }
	public bool DownwardFall { get{return downwardFall;} }

	// Variables
	public float hurtDuration = 1f;
	private float hurtTimer; 
	const float groundedRadius = .2f;
	const float contactRadius = .1f;

	// Reference Variables
	public SpriteRenderer spriteRenderer;
	[SerializeField] private LayerMask whatIsSolid;
	public Transform groundCheck; 
	public Transform ceilingCheck;
	public Transform rightCheck;
	public Transform leftCheck;
	private PCInput pcInput; 
	private Rigidbody2D rb2d; 


	private void OnDrawGizmos(){
		Gizmos.color = Color.green; 
		Gizmos.DrawSphere(ceilingCheck.position, contactRadius); 
		Gizmos.DrawSphere(rightCheck.position, contactRadius);
	}

	private void Awake(){
		rb2d = GetComponent<Rigidbody2D>(); 
		pcInput = GetComponent<PCInput>(); 
	}

	private bool IsPlayerCollider( Collider2D _c ){
		if( _c.name == "Collider_standing"
			|| _c.name == "Collider_crouching" ){

			return true;
		}
		return false; 
	}

	// [[ ----- UPDATE ----- ]]
	public void StateUpdate(){
		if( Hurt ){ 
			hurtTimer -= Time.deltaTime; 

			if( hurtTimer <= 0 ){ HurtOff(); }
		}
	}


	// [[ ----- FIXED UPDATE ----- ]]
	public void StateFixedUpdate(){

		// set grounded flag
		grounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            groundCheck.position, groundedRadius, whatIsSolid);
        for (int i = 0; i < colliders.Length; i++){
            if( !IsPlayerCollider(colliders[i]) ){ 
                grounded = true;  
                break;
            }
        }

        // set ceilinged flag
        ceilinged = false;
        colliders = Physics2D.OverlapCircleAll(
            ceilingCheck.position, contactRadius, whatIsSolid);
        for (int i = 0; i < colliders.Length; i++){
            if( !IsPlayerCollider(colliders[i]) ){
                ceilinged = true;  
                break;
            }
        }

		// set walled flags	
		walledLeft = false;
		walledRight = false; 
		walled = false; 
		Collider2D lastCollider = null; 
		// check walled right
		colliders = Physics2D.OverlapCircleAll(
        	rightCheck.position, contactRadius, whatIsSolid);
        for (int i = 0; i < colliders.Length; i++){
        	if( !IsPlayerCollider(colliders[i]) ){ 
        		walledRight = true;
        		lastCollider = colliders[i]; 
        		break;
        	}
		}

		// set ledge climbable right
		if( lastCollider != null 
			&& (transform.position.y > lastCollider.transform.position.y) ){

			ledgeClimbableRight = true; 
		}else{ ledgeClimbableRight = false; }

		// check walled left
		colliders = Physics2D.OverlapCircleAll(
        	leftCheck.position, contactRadius, whatIsSolid);
        for (int i = 0; i < colliders.Length; i++){
        	if( !IsPlayerCollider(colliders[i]) ){ 
        		walledLeft = true; 
        		lastCollider = colliders[i]; 
        		break;
        	}
		}

		// set ledge climbable left
		if( lastCollider != null 
			&& (transform.position.y > lastCollider.transform.position.y) ){

			ledgeClimbableLeft = true; 
		}else{ ledgeClimbableLeft = false; }

		// set walled flags
		if( walledLeft || walledRight ){ walled = true; }
		else{ walled = false; }

		// set ledge climbable
		if( ledgeClimbableLeft || ledgeClimbableRight ){ ledgeClimbable = true; }
		else{ ledgeClimbable = false; }

		// set downward fall
		if( rb2d.velocity.y < -0.5 ){ downwardFall = true; }
		else{ downwardFall = false; }
	}

// -----------------------------------------------------------------------------
// Physics Events

	private void OnCollisionEnter2D( Collision2D c ){
        // if( c.gameObject.layer == LayerMask.NameToLayer("Solids") ){}
    }

// -----------------------------------------------------------------------------
// Public Methods

    // [[ ----- HURT ON ----- ]]
    public void HurtOn(){
    	if( !Hurt ){
    		Debug.Log("OUCH!");
    		Hurt = true; 
    		hurtTimer = hurtDuration;
    		spriteRenderer.color = new Color(1, 1, 1, 0.5f);

    		// decrement fownedcount
    		SceneMaster.active.UpdateFFCount( -1 ); 
    	}
    }

    // [[ ----- HURT OFF ----- ]]
    public void HurtOff(){
    	Hurt = false;
    	spriteRenderer.color = new Color(1, 1, 1, 1);
    }
}
