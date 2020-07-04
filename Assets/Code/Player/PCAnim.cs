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
    // public GameObject particleEffects;
    public ParticleSystem wallSlideRightEffect;
    public ParticleSystem wallSlideLeftEffect;
    public ParticleSystem runRightEffect;
    public ParticleSystem runLeftEffect;
    private Animator anim; 
    private PCState pcState;
    private PCInput pcInput;
    private PCMove pcMove;  
    private Move_Jump mJump; 
    private Move_Run mRun; 
    private Move_WallSlide mWallslide;
    private Move_Dash mDash;
    private Move_Duck mDuck; 

    // Enums
    public enum State{
    	BoxIdle,
        BoxRun,
    	BoxJump,
    	BoxLand,
    	BoxWallslide,
    	BoxDash,
        BoxDuck, 
        BoxUnduck
    }

    private void Awake(){
    	anim = GetComponent<Animator>(); 
    	pcState = GetComponent<PCState>();
    	pcInput = GetComponent<PCInput>();
        pcMove = GetComponent<PCMove>();
    	mJump = GetComponent<Move_Jump>();
        mRun = GetComponent<Move_Run>(); 
    	mWallslide = GetComponent<Move_WallSlide>(); 
    	mDash = GetComponent<Move_Dash>(); 
        mDuck = GetComponent<Move_Duck>(); 

    	aState = State.BoxIdle;
    	transition = true; 
    }

    public void AnimUpdate(){
    	RunEffects(); 
        WallSlideEffects();

        switch( aState ){

    		case State.BoxIdle:
    			if( transition 
    				&& anim.GetCurrentAnimatorStateInfo(0).IsName("BoxIdle")){

    				transition = false; 
    				process = true; 
    				anim.SetBool("ToIdle", false);
    			}else if( process ){
                    // go to run 
                    if( mRun.IsActive ){
                        transition = true; 
                        process = false; 
                        anim.SetBool("ToRun", true);
                        aState = State.BoxRun; 
                    // go to duck 
                    }else if( mDuck.IsActive ){
                        transition = true; 
                        process = false; 
                        anim.SetBool("ToDuck", true);
                        aState = State.BoxDuck; 
    				// go to dash
    				}else if( mDash.DState == Move_Dash.State.Move ){
    					transition = true; 
    					process = false;  
    					anim.SetBool("ToDash", true);
    					aState = State.BoxDash; 
    				}else if( !pcState.Grounded ){
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


            case State.BoxRun:
                if( transition 
                    && anim.GetCurrentAnimatorStateInfo(0).IsName("BoxRun")){

                    transition = false; 
                    process = true; 
                    anim.SetBool("ToRun", false);
                }else if( process ){
                    // go to duck 
                    if( mDuck.IsActive ){
                        transition = true; 
                        process = false; 
                        anim.SetBool("ToDuck", true);
                        aState = State.BoxDuck; 
                    // go to dash
                    }else if( mDash.DState == Move_Dash.State.Move ){
                        transition = true; 
                        process = false;  
                        anim.SetBool("ToDash", true);
                        aState = State.BoxDash; 
                    }else if( !pcState.Grounded ){
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
                    // go to idle
                    }else if( !mRun.IsActive ){
                        transition = true; 
                        process = false; 
                        anim.SetBool("ToIdle", true);
                        aState = State.BoxIdle;
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
    				// go to dash
    				if( mDash.DState == Move_Dash.State.Move ){
    					transition = true; 
    					process = false;  
    					anim.SetBool("ToDash", true);
    					aState = State.BoxDash; 
    				// go to wallslide
    				}else if( mWallslide.IsActive ){
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
                    // go to run
                    }else if( mRun.IsActive ){
                        transition = true; 
                        process = false; 
                        anim.SetBool("ToRun", true);
                        aState = State.BoxRun; 
    				// go to dash
    				}else if( mDash.DState == Move_Dash.State.Move ){
    					transition = true; 
    					process = false;  
    					anim.SetBool("ToDash", true);
    					aState = State.BoxDash; 
    				// go to duck 
                    }else if( mDuck.IsActive ){
                        transition = true; 
                        process = false; 
                        anim.SetBool("ToDuck", true);
                        aState = State.BoxDuck; 
                    // go to jump
    				}else if( mJump.IsActive ){
    					transition = true;
    					process = false;  
    					anim.SetBool("ToJump", true);
    					aState = State.BoxJump; 
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
                        wallSlideLeftEffect.gameObject.SetActive(true);
                        // wallSlideLeftEffect.Play();
    				}else{ 
                        spriteRenderer.flipX = false; 
                        wallSlideRightEffect.gameObject.SetActive(true); 
                        // wallSlideRightEffect.Play();
                    }
    			}else if( process ){                    
    				// go to dash
    				if( mDash.DState == Move_Dash.State.Move ){
    					transition = true; 
    					process = false;  
    					anim.SetBool("ToDash", true);
    					aState = State.BoxDash; 
                        ResetWallslideEffects();
    				// go to idle
    				}else if( pcState.Grounded ){
    					transition = true;
    					process = false;  
    					anim.SetBool("ToIdle", true); 
    					aState = State.BoxIdle; 
                        ResetWallslideEffects();
    				// go to jump
    				}else if( !mWallslide.IsActive
    					|| mJump.JState == Move_Jump.State.Jumping_WallJump ){
    					transition = true;
    					process = false;  
    					anim.SetBool("ToJump", true); 
    					aState = State.BoxJump; 
                        ResetWallslideEffects();
    				}
    			}
    			break;
    		

    		case State.BoxDash:
    			if( transition 
    				&& anim.GetCurrentAnimatorStateInfo(0).IsName("BoxDash")){

    				transition = false; 
    				process = true; 
    				anim.SetBool("ToDash", false);
    				processTimer = 0;
    				// flip sprite
    				if( pcInput.RightButton 
    					&& spriteRenderer.transform.localScale.x > 0 ){

    					spriteRenderer.flipX = true; 
    				}else{ spriteRenderer.flipX = false; }
    			// go to idle
    			}else if( process ){
    				processTimer += Time.deltaTime;
    				if( processTimer >= 0.33 ){
    					transition = true;
    					process = false;  
    					anim.SetBool("ToIdle", true); 
    					aState = State.BoxIdle; 
    				}
    			}
    			break;

            case State.BoxDuck:
                if( transition 
                    && anim.GetCurrentAnimatorStateInfo(0).IsName("BoxDuck")){

                    transition = false; 
                    process = true; 
                    anim.SetBool("ToDuck", false);
                }else if( process ){
                    // go to Unduck
                    if( !mDuck.IsActive ){
                        transition = true; 
                        process = false; 
                        anim.SetBool("ToUnduck", true); 
                        aState = State.BoxUnduck; 
                    // go to dash
                    }else if( mDash.DState == Move_Dash.State.Move ){
                        transition = true; 
                        process = false;  
                        anim.SetBool("ToDash", true);
                        aState = State.BoxDash; 
                    // go to jump
                    }/*else if( mJump.IsActive ){
                        transition = true; 
                        process = false; 
                        anim.SetBool("ToJump", true); 
                        aState = State.BoxJump; 
                    }*/
                }
                break;

            case State.BoxUnduck:
                if( transition 
                    && anim.GetCurrentAnimatorStateInfo(0).IsName("BoxUnduck")){

                    transition = false; 
                    process = true; 
                    anim.SetBool("ToUnduck", false);
                    processTimer = 0;
                }else if( process ){
                    processTimer += Time.deltaTime;
                    // go to dash
                    if( mDash.DState == Move_Dash.State.Move ){
                        transition = true; 
                        process = false;  
                        anim.SetBool("ToDash", true);
                        aState = State.BoxDash; 
                    // got to run
                    }else if( mRun.IsActive ){
                        transition = true; 
                        process = false; 
                        anim.SetBool("ToRun", true);
                        aState = State.BoxRun; 
                    // go to jump
                    }else if( mJump.IsActive ){
                        transition = true; 
                        process = false; 
                        anim.SetBool("ToJump", true); 
                        aState = State.BoxJump; 
                    // go to idle
                    }else if( processTimer >= 0.2f ){
                        transition = true; 
                        process = false; 
                        anim.SetBool("ToIdle", true); 
                        aState = State.BoxIdle; 
                    }
                    
                }
                break;

    		default:
    			Debug.Log("switch: value match not found");
    			break;
    	}
    }

    private void WallSlideEffects(){
        if( aState == State.BoxWallslide ){
            if( pcMove.VelocityY < 0 ){
                if( wallSlideLeftEffect.gameObject.activeSelf 
                    && !wallSlideLeftEffect.isPlaying){

                    wallSlideLeftEffect.Play();
                
                }else if( wallSlideRightEffect.gameObject.activeSelf 
                    && !wallSlideRightEffect.isPlaying){

                    wallSlideRightEffect.Play();
                }
            }
        }   
    }

    private void ResetWallslideEffects(){
        wallSlideRightEffect.gameObject.SetActive(false);
        wallSlideLeftEffect.gameObject.SetActive(false); 
        wallSlideRightEffect.Stop();
        wallSlideLeftEffect.Stop(); 
    }

    private void RunEffects(){
        if( pcState.MoveRun && pcState.Grounded ){
            if( pcMove.VelocityX > 0 ){
                // turn on right
                if( !runRightEffect.gameObject.activeSelf ){
                    runRightEffect.gameObject.SetActive(true);
                }
                if( !runRightEffect.isPlaying ){ runRightEffect.Play(); }
                // turn off left
                if( runLeftEffect.isPlaying ){ runLeftEffect.Stop(); }
                if( runLeftEffect.gameObject.activeSelf ){
                    runLeftEffect.gameObject.SetActive(false);
                }
            }else if( pcMove.VelocityX < 0 ){
                // turn on left
                if( !runLeftEffect.gameObject.activeSelf ){
                    runLeftEffect.gameObject.SetActive(true);
                }
                if( !runLeftEffect.isPlaying ){ runLeftEffect.Play(); }
                // turn off right
                if( runRightEffect.isPlaying ){ runRightEffect.Stop(); }
                if( runRightEffect.gameObject.activeSelf ){
                    runRightEffect.gameObject.SetActive(false);
                }
            }
        }else{
            if( runLeftEffect.isPlaying ){ runLeftEffect.Stop(); }
            if( runLeftEffect.gameObject.activeSelf ){ 
                runLeftEffect.gameObject.SetActive(false); 
            }
            if( runRightEffect.isPlaying ){ runRightEffect.Stop(); }
            if( runRightEffect.gameObject.activeSelf ){ 
                runRightEffect.gameObject.SetActive(false); 
            }
        }
    }
}
