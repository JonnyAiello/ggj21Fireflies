using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	// Variables

    // Reference Variables
    private PCState pcState; 
    private PCInput pcInput; 
    private PCMove pcMove; 
    private PCAnim pcAnim; 


	private void Awake(){
        pcState = GetComponent<PCState>();
        pcInput = GetComponent<PCInput>();
        pcMove = GetComponent<PCMove>(); 
        pcAnim = GetComponent<PCAnim>(); 
	}


    // Start is called before the first frame update
    void Start(){
        
    }

    void Update(){
        pcInput.InputUpdate(); 
        pcState.StateUpdate();    
        pcAnim.AnimUpdate(); 
    }

    private void FixedUpdate(){
        pcState.StateFixedUpdate(); 
        pcMove.MoveFixedUpdate(); 
    }
}
