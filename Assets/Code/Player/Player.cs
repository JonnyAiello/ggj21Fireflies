using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class Player : MonoBehaviour {

	// Variables
    private bool dead; 
    private string currScene; 

    // Reference Variables
    public GameObject deathPopPref; 
    private PCState pcState; 
    private PCInput pcInput; 
    private PCMove pcMove; 
    private PCAnim pcAnim; 

// -----------------------------------------------------------------------------
// Unity Events

	private void Awake(){
        pcState = GetComponent<PCState>();
        pcInput = GetComponent<PCInput>();
        pcMove = GetComponent<PCMove>(); 
        pcAnim = GetComponent<PCAnim>(); 
        currScene = SceneManager.GetActiveScene().name;
        Debug.Log("scene name: " + currScene);
	}


    void Start(){
        transform.position 
            = SceneMaster.active.currentCheckpoint.transform.position;         
        Debug.Log("Box Position: " + transform.position);
    }

    void Update(){
        if( !dead ){
            pcInput.InputUpdate(); 
            pcState.StateUpdate();    
            pcAnim.AnimUpdate(); 
        }
    }

    private void FixedUpdate(){
        if( !dead ){
            pcState.StateFixedUpdate(); 
            pcMove.MoveFixedUpdate(); 
        }
    }

// -----------------------------------------------------------------------------
// Public methods

    public void Die(){
        if( !dead ){
            dead = true; 
            SceneMaster.active.PauseTimer(true); 
            pcAnim.spriteRenderer.gameObject.SetActive(false); 
            Destroy(transform.Find("CamFollowPoint").gameObject);
            // play dead particle effect
            GameObject deathPop = (GameObject)Instantiate(
                deathPopPref, transform.position, transform.rotation); 
            deathPop.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 8); 
            Invoke("ReloadLevel", 2f); 
        }
    }

    private void ReloadLevel(){
        SceneManager.LoadScene(currScene);
    }

}
