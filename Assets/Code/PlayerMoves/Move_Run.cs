using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Run : MoveBehavior {
    
	// Variables
    [SerializeField] private bool isActive; 
    [SerializeField] private float accelSpeed = 2f;
    [SerializeField] private float autoBrakes = 1f;
	private float minH;
    private float maxH; 
  
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

    public override void Init(){
    	if( pcInput.LeftButton || pcInput.RightButton ){ isActive = true; }
        else{ isActive = false; }
    }

    public override bool AffectsForce(){ return isActive; }
    
    public override Vector2 GetForce(){
    	float hForce = accelSpeed;
        if( pcInput.LeftButton ){ hForce *= -1; }
    	return new Vector2(hForce, 0); 
    }

    public override bool AffectsHLimits(){ return true; }
    
    // [[ ----- GET H LIMITS ----- ]]
    public override Vector2 GetHLimits(){ 
        minH = pcMove.MaxHSpeed * -1; 
        maxH = pcMove.MaxHSpeed; 

    	// stop at wall
        if( pcState.Walled ){
            if( pcState.WalledRight ){ maxH = 0; }
            if( pcState.WalledLeft ){ minH = 0; }
        
        // horizontal auto-brakes
        }else{
            if( pcState.Grounded ){
                if( !isActive ){
                    minH = autoBrakes * -1;
                    maxH = autoBrakes;
                }else if( pcInput.RightButton ){ minH = autoBrakes * -1; }
                else if( pcInput.LeftButton ){ maxH = autoBrakes; }
            }
        }
        return new Vector2(minH, maxH); 
    }	

    public override bool AffectsVLimits(){ return false; }
    public override bool IsExclusive(){ return false; }
    public override bool ZeroMovement(){ return false; }
    public override Vector2 GetVLimits(){ return Vector2.zero; }  
    public override bool AffectsPosition(){ return false; } 
    public override Vector2 GetPosition(){ return Vector2.zero; } 
}
