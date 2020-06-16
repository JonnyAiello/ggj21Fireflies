using System.Collections;
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
	private float timeUpdateTick = 0.5f;
	private float gameTime; 
	private float updateTimer;

	// Reference Variables
    public Checkpoint currentCheckpoint; 
    public GameObject timerGuiPref;
	private Text timerMinutes;
	private Text timerSeconds; 

    // Properties
    public string LevelID { get{return levelID;} }

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
        Debug.Log("Initialize called");
        // create and bind timer GUI
        Transform canvasTrans = GameObject.FindWithTag("Canvas").transform; 
        GameObject tgui = (GameObject)Instantiate(timerGuiPref, canvasTrans); 
        timerMinutes = tgui.transform.Find("Text_Minutes").GetComponent<Text>();
        timerSeconds = tgui.transform.Find("Text_Seconds").GetComponent<Text>(); 

        // catch missing checkpoint error
        if( currentCheckpoint == null ){ 
            Debug.LogError("Checkpoint not provided"); 
        }
    }

    // [[ ----- UPDATE ----- ]]
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
