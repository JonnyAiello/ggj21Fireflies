using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Note: to edit the duration of the coodlown particle system effect, change
// the START TIME value of ParticleSystemReverseSimulationSuperSimple, this 
// overrides the duration value of the actual particle system


public class Move_Dash : MoveBehavior {

	// Variables
    [SerializeField] private bool isActive; 
	[SerializeField] private float dist = 4f; 
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float momentumDuration = 0.3f; 
    [SerializeField] private float cooldownDuration = 0.4f; 
    [SerializeField] private State dState; 
    [SerializeField] private LayerMask dashMask; 

	private bool initialized; 
    private bool momentumAddedLock;
    private bool momentumDone; 
    private bool cooldownDone; 
	private float dashTimer;
    private float momentumTimer; 
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
    public CircleCollider2D bodyCollider;  
    public GameObject cooldownEffectGO;
    public GameObject dashBurstEffectPrefab;

    // Properties
    public bool IsActive { get{return isActive;} }
    public State DState { get{return dState;} }

    // Enums
    public enum State{
        NotDashing,
        Initialize,
        Move,
        PostMomentum,
        Cooldown
    }

	private void Awake(){
		pcInput = GetComponent<PCInput>();
		pcState = GetComponent<PCState>(); 
        pcMove = GetComponent<PCMove>();
        colliderRadius = (bodyCollider.bounds.size.x / 2f) + 0.2f; 

	}

	// On Draw Gizmos
	private void OnDrawGizmos(){

		/*if( hitPoint != Vector2.zero ){
			Gizmos.color = Color.green; 
			Gizmos.DrawSphere(hitPoint, .2f);
		}*/

	}

// -----------------------------------------------------------------------------
// MoveBehavor

	public override void Init( bool _overridden ){
        switch( dState ){
            
            case State.NotDashing:
                if( !_overridden ){
                    if( pcInput.DashButton && pcState.MovingHoriz ){
                        dState = State.Initialize;
                    }
                }
                break;
            
            case State.Initialize:
                if( _overridden ){ dState = State.NotDashing; }
                else if( initialized ){ dState = State.Move; }
                break;
            
            case State.Move:
                if( _overridden ){ dState = State.NotDashing; }
                else if( targetReached ){ dState = State.PostMomentum; }
                break;

            case State.PostMomentum:
                if( _overridden ){ dState = State.NotDashing; }
                else if( momentumDone ){ dState = State.Cooldown; }
                break;
            
            case State.Cooldown:
                if( _overridden || cooldownDone ){ dState = State.NotDashing; }
                break;
            
            default:
                Debug.Log("switch: value match not found");
                break;
        }

        // reset triggers if not dashing
        if( dState == State.NotDashing
            && (isActive || initialized || targetReached || cooldownDone)){

            isActive = false;
            initialized = false;
            targetReached = false; 
            momentumAddedLock = false; 
            momentumDone = false; 
            cooldownDone = false; 
        }

        // Initialize dash 
        if( dState == State.Initialize ){
            // determine hit point / end point
            wallHit = false; 
            if( pcInput.RightButton ){ castDir = Vector2.right; }
            else{ castDir = Vector2.left; }
            startPoint = transform.position;
            endPoint = (Vector2)transform.position + (castDir * dist);
            // raycast
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position, castDir, dist, dashMask);
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
            isActive = true; 
            initialized = true;
            cooldownTimer = 0; 
            momentumTimer = 0; 
            dashTimer = dashDuration * 0.25f; 
                // add a little skip to the beginning of dash 

            // create burst effect
            GameObject burst = Instantiate(dashBurstEffectPrefab, 
                transform.position, Quaternion.identity) as GameObject;
            burst.GetComponent<ParticleSystem>().Play();
            Destroy(burst, 1f); 
        }

        // post momentum timer
        if( dState == State.PostMomentum ){
            momentumTimer += Time.fixedDeltaTime; 
            if( momentumTimer > momentumDuration ){ 
                momentumDone = true; 
                isActive = false;
            }
        }

        // Cooldown timer
        if( dState == State.Cooldown ){
            cooldownTimer += Time.fixedDeltaTime; 
            if( cooldownTimer > cooldownDuration ){ cooldownDone = true; }
        }
    }

    public override bool IsExclusive(){ return isActive; }

    public override bool AffectsPosition(){
        return (dState == State.Move); 
    } 

    // [[ ----- GET POSITION ----- ]]
    public override Vector2 GetPosition(){
    	Vector2 setToPosition = transform.position; 

	    // if dash started - determine point lerp
	    if( !targetReached ){
	    	dashTimer += Time.fixedDeltaTime;

            // set position 
            float ratio = dashTimer / dashDuration; 
            setToPosition = Vector2.Lerp(startPoint, endPoint, ratio);
            if( (castDir.x > 0 && setToPosition.x >= hitPoint.x)
                || (castDir.x < 0 && setToPosition.x <= hitPoint.x)
                || ratio >= 1 ){

                targetReached = true; 
                setToPosition = hitPoint;
            }
	    }
	    return setToPosition; 
    }


    public override bool AffectsForce(){
        if( dState == State.PostMomentum && !momentumAddedLock ){ return true; }    
        return false; 
    }

    // [[ ----- GET FORCE ----- ]]
    public override Vector2 GetForce(){
        float xForce = 40f;
        if( castDir.x < 0 ){ xForce *= -1; }
        cooldownEffectGO.SetActive(true); 
            // begin cooldown particle effect
        momentumAddedLock = true; 

        return new Vector2(xForce, 0); 
    } 

}
