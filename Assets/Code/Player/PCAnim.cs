using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCAnim : MonoBehaviour {
    
    // Variables
    [SerializeField] private State aState; 
    private bool transition;
    private bool process;  
    private float processTimer;

    // Reference Variables
    public SpriteRenderer spriteRenderer; 
    private Animator anim; 
    private PCState pcState;
    private Move_Jump mJump; 
    private Move_WallSlide mWallslide;

    // Enums
    public enum State{
    	BoxIdle,
    	BoxJump,
    	BoxLand,
    	BoxWallslide
    }

    private void Awake(){
    	anim = GetComponent<Animator>(); 
    	// spriteRenderer = GetComponent<SpriteRenderer>(); 
    	pcState = GetComponent<PCState>();
    	mJump = GetComponent<Move_Jump>();
    	mWallslide = GetComponent<Move_WallSlide>(); 

    	aState = State.BoxIdle;
    	transition = true; 
    }

    public void AnimUpdate(){
    	switch( aState ){
    	

    		case State.BoxIdle:
    			if( transition 
    				&& anim.GetCurrentAnimatorStateInfo(0).IsName("BoxIdle")){

    				transition = false; 
    				process = true; 
    				anim.SetBool("ToIdle", false);
    			}else if( process ){
    				if( !pcState.Grounded ){
    					// go to Wallslide
    					if( mWallslide.IsActive ){
    						transition = true;
	    					process = false;  
	    					anim.SetBool("ToWallslide", true);
	    					aState = State.BoxWallslide; 
	    				// go to jump
    					}else{
    						transition = true;
	    					process = false;  
	    					anim.SetBool("ToJump", true);
	    					aState = State.BoxJump; 
    					}
    				// go to jump
    				}else if( mJump.IsActive ){
    					transition = true;
    					process = false;  
    					anim.SetBool("ToJump", true);
    					aState = State.BoxJump; 
    				}
    			}
    			break;


    		case State.BoxJump:
    			if( transition 
    				&& anim.GetCurrentAnimatorStateInfo(0).IsName("BoxJump")){
    				
    				transition = false; 
    				process = true; 
    				anim.SetBool("ToJump", false);
    			}else if( process ){
    				// go to wallslide
    				if( mWallslide.IsActive ){
    					transition = true; 
    					process = false; 
    					anim.SetBool("ToWallslide", true);
    					aState = State.BoxWallslide;
    				// go to land
    				}else if( pcState.Grounded 
    					|| mJump.JState == Move_Jump.State.Landed_ButtonHeld
    					|| mJump.JState == Move_Jump.State.Landed_ButtonReleased){

    					transition = true;
    					process = false;  
    					anim.SetBool("ToLand", true); 
    					aState = State.BoxLand; 
    				}
    			}   			
    			break;
    		

    		case State.BoxLand:
    			if( transition 
    				&& anim.GetCurrentAnimatorStateInfo(0).IsName("BoxLanding")){

    				transition = false; 
    				process = true; 
    				anim.SetBool("ToLand", false);
    				processTimer = 0; 
    			}else if( process ){
    				// go to idle
    				processTimer += Time.deltaTime;
    				if( processTimer >= 0.33 ){
    					transition = true;
    					process = false;  
    					anim.SetBool("ToIdle", true); 
    					aState = State.BoxIdle; 
    				}
    			}
    			break;
    		

    		case State.BoxWallslide:
    			if( transition 
    				&& anim.GetCurrentAnimatorStateInfo(0).IsName("BoxWallSlide")){
    				
    				transition = false; 
    				process = true; 
    				anim.SetBool("ToWallslide", false);
    				// flip sprite
    				if( pcState.WalledLeft 
    					&& spriteRenderer.transform.localScale.x > 0 ){

    					spriteRenderer.flipX = true; 
    				}else{ spriteRenderer.flipX = false; }
    			}else if( process ){
    				// go to idle
    				if( pcState.Grounded ){
    					transition = true;
    					process = false;  
    					anim.SetBool("ToIdle", true); 
    					aState = State.BoxIdle; 
    				// go to jump
    				}else if( !mWallslide.IsActive
    					|| mJump.JState == Move_Jump.State.Jumping_WallJump ){
    					transition = true;
    					process = false;  
    					anim.SetBool("ToJump", true); 
    					aState = State.BoxJump; 
    				}
    			}
    			break;
    		

    		default:
    			Debug.Log("switch: value match not found");
    			break;
    	}
    }
}
