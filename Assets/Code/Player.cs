using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(PlayerConditions))]
[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour {

	// Variables

    // Reference Variables
    private PCState pcState; 
    private PCInput pcInput; 

	// Reference Variables OLD
	private PlayerMove pMove;
	private PlayerConditions pConditions; 
	private PlayerInput pInput; 


	private void Awake(){
        pcState = GetComponent<PCState>();
        pcInput = GetComponent<PCInput>();

        // OLD
		pMove = GetComponent<PlayerMove>();
		pConditions = GetComponent<PlayerConditions>(); 
		pInput = GetComponent<PlayerInput>();
	}


    // Start is called before the first frame update
    void Start(){
        
    }

    void Update(){
        
        pcInput.InputUpdate(); 
        pcState.StateUpdate();    

        // OLD LOGIC
        // pInput.InputUpdate();
    	

    }

    private void FixedUpdate(){

        pcInput.InputFixedUpdate(); 
        pcState.StateFixedUpdate(); 

        // OLD LOGIC
    	// get input
    	pInput.InputFixedUpdate(); 
        pConditions.UpdateFlags();
    	// process move
    	// pMove.Move();

    }
}
