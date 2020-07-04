using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class LevelGoal : MonoBehaviour {
    
    // Variables
    public string nextLevelID; 
	private bool triggerLock;
	
	// Reference Variables
	public GameObject goalText;
	public Text parLabelText;
	public Text parText;
	public Text timeLabelText;
	public Text timeText; 
	public Text wellDoneText;
	public Player player; 

	void OnTriggerEnter2D(Collider2D col){
		if( !triggerLock ){
			if( col.gameObject.tag == "Player" ){
				Debug.Log("LEVEL COMPLETE");
				triggerLock = true; 
				StartCoroutine("EndLevelRoutine"); 
			}
		}
	}

	private IEnumerator EndLevelRoutine(){
		// stop actions
		player.Disengage();
		SceneMaster.active.PauseTimer(true); 
		// set player time text
		float gameTime = SceneMaster.active.GameTime; 
		int seconds = (int)gameTime;
		int minutes = seconds / 60; 
		int remainder = seconds % 60; 
		timeText.text = System.String.Format(
			"{0}:{1}", minutes.ToString(), remainder.ToString("00"));

		yield return new WaitForSeconds(0.5f);
		goalText.SetActive(false); 

		yield return new WaitForSeconds(0.75f);
		parLabelText.gameObject.SetActive(true); 

		yield return new WaitForSeconds(0.5f);
		parText.gameObject.SetActive(true); 

		yield return new WaitForSeconds(0.5f);
		timeLabelText.gameObject.SetActive(true); 

		yield return new WaitForSeconds(0.5f);
		timeText.gameObject.SetActive(true); 

		yield return new WaitForSeconds(1);
		wellDoneText.gameObject.SetActive(true); 

		yield return new WaitForSeconds(1);
		Destroy(SceneMaster.active.gameObject); 
		SceneManager.LoadScene(nextLevelID);
	}
}
