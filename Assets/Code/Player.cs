using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(PlayerConditions))]
[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour {

	// Variables


	// Reference Variables
	private PlayerMove pMove;
	private PlayerConditions pConditions; 
	private PlayerInput pInput; 


	private void Awake(){
		pMove = GetComponent<PlayerMove>();
		pConditions = GetComponent<PlayerConditions>(); 
		pInput = GetComponent<PlayerInput>();
	}


    // Start is called before the first frame update
    void Start(){
        
    }

    void Update(){
        pInput.InputUpdate();
    	

    }

    private void FixedUpdate(){
    	// get input
    	pInput.InputFixedUpdate(); 
        pConditions.UpdateFlags();
    	// process move
    	pMove.Move();

    }
}
