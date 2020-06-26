using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DangerObject))]
public class Enemy_DumbPatrol : MonoBehaviour{ 

    // Variables
    public bool isActive; 
    public bool isWhiteBox; // white boxes act as collectables, no danger
    public bool isBlackBox; // unkillable enemies, don't have vulnerabilities
    public bool patrolLoop; 
    [Range(0, 0.1f)] public float moveSpeed; 
    	// % of distance between points moved per frame
    public float vulnerableTime = 1f;
    public float dangerTime = 3f; 
    private float timer; 
    private bool isOffensive; 
    private bool pathForward;
    private float posRatio;  
    private Transform lastTarg;
    private Transform nextTarg; 
    private int targIndex; 

    // Reference Variables
    public GameObject deathPopPref; 
    public Transform[] path;
    private SpriteRenderer sprite; 
    private DangerObject dangerObj;

    private void Awake(){
    	sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>(); 
    	dangerObj = GetComponent<DangerObject>(); 
    }

    // [[ ----- START ----- ]]
    private void Start(){
    	if( isWhiteBox ){
    		isOffensive = false;
    		dangerObj.isDangerous = false; 
    		sprite.color = Color.white; 
    	
    	}else{
    		sprite.color = Color.black;
    		isOffensive = true; 
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
    }

    // [[ ----- UPDATE ----- ]]
    private void Update(){
    	if( isActive ){
    		if( !isBlackBox ){
	    		// update danger state
	    		if( isOffensive ){
	    			timer += Time.deltaTime; 
	    			if( timer > dangerTime ){
	    				// set to vulnerable
	    				sprite.color = Color.white; 
	    				dangerObj.isDangerous = false; 
	    				timer = 0; 
	    				isOffensive = false; 
	    			}
	    		}else{
	    			timer += Time.deltaTime; 
	    			if( timer > vulnerableTime ){
	    				// set to offensive
	    				sprite.color = Color.black; 
	    				dangerObj.isDangerous = true; 
	    				timer = 0; 
	    				isOffensive = true; 
	    			}
	    		}
	    	}


    		// update position
    		if( posRatio < 1 ){
    			posRatio += moveSpeed; 
    			transform.position = Vector2.Lerp(
    				lastTarg.position, nextTarg.position, posRatio);
    		}else{ SetNewTarget(); }
    	}
    }

    // [[ ----- SET NEW TARGET ----- ]]
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

    // [[ ----- ON TRIGGER ENTER 2D ----- ]]
    private void OnTriggerEnter2D( Collider2D _other ){
    	if( _other.tag == "Player" && !isOffensive ){ Die(); }
    }

    // [[ ----- DIE ----- ]]
    private void Die(){
    	GameObject deathPop = (GameObject)Instantiate(
            deathPopPref, transform.position, transform.rotation); 
    	deathPop.transform.Find("Sprite").GetComponent<SpriteRenderer>().color 
    		= Color.black; 
        deathPop.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 8);
        Destroy(gameObject); 
    }

}
