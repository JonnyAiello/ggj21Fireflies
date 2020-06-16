using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DumbPatrol : MonoBehaviour{ 

    // Variables
    public bool isActive; 
    public bool patrolLoop; 
    [Range(0, 0.1f)] public float moveSpeed; 
    	// % of distance between points moved per frame
    private bool pathForward;
    private float posRatio;  
    private Transform lastTarg;
    private Transform nextTarg; 
    private int targIndex; 

    // Reference Variables
    public Transform[] path;

    private void Start(){
    	pathForward = true; // sets to true in SetNewTarget initializiation 

    	// get starting path
    	if( path.Length < 2 ){ Debug.LogError("Path Length Error: " + name);}
    	else{
    		if( patrolLoop ){
	    		targIndex = path.Length; 
	    		nextTarg = path[0]; // used to set start pos at start of path
	    	}else{
	    		targIndex = 0; 
	    		nextTarg = path[0]; 
	    	}
    		SetNewTarget(); 
    		isActive = true; 
    	}
    }

    private void Update(){
    	if( isActive ){
    		if( posRatio < 1 ){
    			posRatio += moveSpeed; 
    			transform.position = Vector2.Lerp(
    				lastTarg.position, nextTarg.position, posRatio);
    		}else{ SetNewTarget(); }
    	}
    }

    private void SetNewTarget(){
    	// exactly set pos to next target position
    	transform.position = nextTarg.position; 
    	posRatio = 0; 
    	// loop through path array
    	if( patrolLoop ){
    		targIndex++; 
    		if( targIndex >= path.Length ){ targIndex = 0; }
    	// bounce back up array 
    	}else{
    		if( pathForward ){
    			targIndex++;
    			if( targIndex >= path.Length ){
    				targIndex = (path.Length - 2); 
    				pathForward = false; 
    			}
    		}else{
    			targIndex--;
    			if( targIndex < 0 ){
    				targIndex = 1; 
    				pathForward = true; 
    			}
    		}
    	}
    	lastTarg = nextTarg;
    	nextTarg = path[targIndex]; 
    }

}
