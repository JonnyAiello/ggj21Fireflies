﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 

/*
Because SceneMaster is persistent across loads, have it instantiate elements
on level load such as the timer UI elements
*/

public class SceneMaster : MonoBehaviour {

	// Variables
    public static SceneMaster active; 
    private string levelID; 
	private bool gameStart; 
    private bool pauseGame; 
	private float timeUpdateTick = 0.5f;
	private float gameTime; 
	private float updateTimer;

	// Reference Variables
    public Checkpoint currentCheckpoint; 
    public GameObject timerGuiPref;
    public GameObject timeModPref; 
	private Text timerMinutes;
	private Text timerSeconds; 
	private Transform canvasTrans; 

    // Properties
    public string LevelID { get{return levelID;} }
    public float GameTime { get{return gameTime;} }	

    // [[ ----- ON ENABLE ----- ]]
    private void OnEnable(){
        SceneManager.sceneLoaded += Initialize; 
    }

    // [[ ----- ON DISABLE ----- ]]
    private void OnDisable(){
        SceneManager.sceneLoaded -= Initialize;
    }

    // [[ ----- AWAKE ----- ]]
    // Logic that should be called once, the first time initialized
    private void Awake(){
        Scene scene = SceneManager.GetActiveScene();
        levelID = scene.name; 
        pauseGame = false; 

        // if SceneMaster exists from a previous level, delete it
        if( SceneMaster.active != null ){
            if( SceneMaster.active.LevelID != scene.name ){
                Destroy( SceneMaster.active.gameObject ); 
                SceneMaster.active = null; 
                Debug.Log("Active SceneMaster not for this level, deleted");
            
            // if SceneMaster exists for this level, delete this instance
            }else{
                Debug.Log("SceneMaster already exists for this level, deleting");
                Destroy( gameObject ); 
            }
        }

        // if no SceneMaster is active, make this the active manager
        if( SceneMaster.active == null ){
            SceneMaster.active = this; 
            Debug.Log("No active SceneMaster found, making this active");
            DontDestroyOnLoad(this); 
        }
    }

    // [[ ----- INITIALIZE ----- ]]
    // Logic that should be called at the load of the level 
    private void Initialize( Scene scene, LoadSceneMode mode ){
        // Debug.Log("Initialize called");
        // create and bind timer GUI
        canvasTrans = GameObject.FindWithTag("Canvas").transform; 
        GameObject tgui = (GameObject)Instantiate(timerGuiPref, canvasTrans); 
        timerMinutes = tgui.transform.Find("Text_Minutes").GetComponent<Text>();
        timerSeconds = tgui.transform.Find("Text_Seconds").GetComponent<Text>(); 
        if( pauseGame ){ pauseGame = false; }
        ProcessTimer(); // only here to prevent a timer of 0:00 after respawn

        // catch missing checkpoint error
        if( currentCheckpoint == null ){ 
            Debug.LogError("Checkpoint not provided"); 
        }
    }

    // [[ ----- UPDATE ----- ]]
    void Update() {
    	bool gotInput = (Input.GetButton("Jump") 
    		|| Input.GetButton("Fire1") 
    		|| Input.GetAxis("Horizontal") != 0);
    	if( !gameStart && gotInput ){ gameStart = true; }
    	if( gameStart && !pauseGame ){ ProcessTimer(); }
    }

    // [[ ----- PROCESS TIMER ----- ]]
    private void ProcessTimer(){
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

// -----------------------------------------------------------------------------
// Public methods

    // [[ ----- PAUSE TIMER ----- ]]
    public void PauseTimer( bool _val ){
        pauseGame = _val;
    }

    // [[ ----- MODIFY GAME TIME ----- ]]
    public void ModifyGameTime( float _val ){
    	if( gameTime + _val < 0 ){ gameTime = 0; }
    	else{ gameTime += _val; }
    	GameObject mgui = (GameObject)Instantiate(timeModPref, canvasTrans);
    	Destroy(mgui, 1f); 
    }


}
