using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_WallSlide : MoveBehavior {

	// Variables
	[SerializeField] private bool isActive; 
	[SerializeField] private float slidingSpeedMax = -1f; 

	// Reference Variables
	private PCInput pcInput; 
	private PCState pcState;
    private PCMove pcMove;  

    // Properties
    public bool IsActive { get{return isActive;} }

	private void Awake(){
		pcInput = GetComponent<PCInput>();
		pcState = GetComponent<PCState>(); 
        pcMove = GetComponent<PCMove>(); 
	}

// -----------------------------------------------------------------------------
// Movebehavior

	// [[ ----- INIT ----- ]]
	public override void Init(){
		bool leftWallSlide = (pcState.WalledLeft && pcInput.LeftButton);
		bool rightWallSlide = (pcState.WalledRight && pcInput.RightButton); 
		if( pcState.Airborn && (leftWallSlide || rightWallSlide) ){
			isActive = true;
		}else{ isActive = false; }
	}
       
    public override bool AffectsVLimits(){ return true; }
     
    // [[ ----- GET V LIMITS ----- ]]
    public override Vector2 GetVLimits(){
    	float minV = slidingSpeedMax; 
    	float maxV = pcMove.MaxVSpeed; 

    	return new Vector2(minV, maxV); 
    }

    public override bool IsExclusive(){ return false; }
	public override bool ZeroMovement(){ return false; }
	public override bool AffectsForce(){ return false; }
    public override Vector2 GetForce(){ return Vector2.zero; } 
    public override bool AffectsHLimits(){ return false; }
    public override Vector2 GetHLimits(){ return Vector2.zero; }
    public override bool AffectsPosition(){ return false; } 
    public override Vector2 GetPosition(){ return Vector2.zero; } 
}
