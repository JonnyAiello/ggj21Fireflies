using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_WallSlide : MonoBehaviour {

	// Variables
	[SerializeField] private float slidingSpeedMax = -1f; 

	// Reference Variables
	private PCInput pcInput; 
	private PCState pcState;
    private PCMove pcMove;  


	private void Awake(){
		pcInput = GetComponent<PCInput>();
		pcState = GetComponent<PCState>(); 
        pcMove = GetComponent<PCMove>(); 
	}

	public bool IsActive(){
		bool leftWallSlide = (pcState.WalledLeft && pcInput.LeftButton);
		bool rightWallSlide = (pcState.WalledRight && pcInput.RightButton); 
		if( pcState.Airborn && (leftWallSlide || rightWallSlide) ){return true;}
		return false; 
	} 

	public Vector2 GetVerticalLimits(){
    	float minV = slidingSpeedMax; 
    	float maxV = pcMove.MaxVSpeed; 

    	return new Vector2(minV, maxV); 
    }
}
