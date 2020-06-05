using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class SceneMaster : MonoBehaviour {

	// Variables
    [HideInInspector] public static SceneMaster active; 
	private bool gameStart; 
	private float timeUpdateTick = 0.5f;
	private float gameTime; 
	private float updateTimer;

	// Reference Variables
    public Checkpoint currentCheckpoint; 
	public Text timerMinutes;
	public Text timerSeconds; 

    // Awake
    private void Awake(){
        // singleton game object
        if( active == null ){ active = this; }
        else{ Destroy(gameObject); }

        if( currentCheckpoint == null ){ 
            Debug.LogError("Checkpoint not provided"); 
        }
    }

    // Update is called once per frame
    void Update() {
    	bool gotInput = (Input.GetButton("Jump") 
    		|| Input.GetButton("Fire1") || Input.GetAxis("Horizontal") != 0);
    	if( !gameStart && gotInput ){ gameStart = true; }

    	if( gameStart ){
    		// process in-game timer
    		updateTimer += Time.deltaTime; 
    		if( updateTimer > timeUpdateTick ){
    			updateTimer = 0;

    			gameTime += timeUpdateTick; 

    			int seconds = (int)gameTime;
    			int minutes = seconds / 60; 
    			int remainder = seconds % 60; 

    			// update timer gui
    			timerMinutes.text = minutes.ToString("00");
    			timerSeconds.text = remainder.ToString("00");
    		}
    	}
        
    }
}
