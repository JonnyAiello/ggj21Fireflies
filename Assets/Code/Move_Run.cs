using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Run : MonoBehaviour {
    
	// Variables
	[SerializeField] private float maxHSpeed = 6f;
    [SerializeField] private float accelSpeed = 2f;
    [SerializeField] private float autoBrakes = 1f;
    private float hVelocity; 
	private float minH;
    private float maxH; 
  
	// Reference Variables
	private PCInput pcInput; 
	private PCState pcState; 

	private void Awake(){
		pcInput = GetComponent<PCInput>();
		pcState = GetComponent<PCState>(); 
	}

    public bool IsActive(){
    	float h = pcInput.H; 
    	if( Mathf.Abs(h) > 0.5f ){
    		if( h > 0 && !pcState.WalledRight ){ return true; }
    		if( h < 0 && !pcState.WalledLeft ){ return true; }
    	}
    	return false; 
    }

    public Vector2 GetForces(){
    	float hForce = pcInput.H * accelSpeed;
    	return new Vector2(hForce, 0); 
    }

    public Vector2 GetHorizontalLimits(){
    	// stop at wall
        if( pcState.Walled ){
            if( pcState.WalledRight ){ maxH = 0; }
            if( pcState.WalledLeft ){ minH = 0; }
        // horizontal auto-brakes
        }else{
            if( pcState.Grounded ){
                if( pcInput.H > 0 ){ minH = autoBrakes * -1;}
                else if( pcInput.H < 0 ){ maxH = autoBrakes; }
            }
        }

        return new Vector2(minH, maxH); 
    }	
}
