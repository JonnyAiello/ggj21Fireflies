using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelGoal_Fireflies : MonoBehaviour {

	// Variables
	private string nextLevelID = "Fireflies_0";
	private bool triggerLock;

	// Reference Variables
	public Player player;
	public Transform camFocusPoint;
	public GameObject gameOverTxt;
	public GameObject yourTimeTxt;
	public Text timeStampTxt;
	public GameObject creditsPanel;
	public GameObject tyfpTxt;

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
		// focus cam
		Vector2 newCamPoint = camFocusPoint.transform.position - player.transform.position;
		player.SetCamPoint( newCamPoint );
		// stop actions
		player.Disengage();
		SceneMaster.active.PauseTimer(true); 
		// set player time text
		float gameTime = SceneMaster.active.GameTime; 
		int seconds = (int)gameTime;
		int minutes = seconds / 60; 
		int remainder = seconds % 60; 
		timeStampTxt.text = System.String.Format(
			"{0}:{1}", minutes.ToString(), remainder.ToString("00"));

		// turn on text in sequence
		yield return new WaitForSeconds(1f);
		gameOverTxt.SetActive(true);

		yield return new WaitForSeconds(1.25f);
		yourTimeTxt.SetActive(true);
		timeStampTxt.gameObject.SetActive(true);

		yield return new WaitForSeconds(1.25f);
		creditsPanel.SetActive(true);

		yield return new WaitForSeconds(2f);
		tyfpTxt.SetActive(true);

		yield return new WaitForSeconds(8f);
		Destroy(SceneMaster.active.gameObject); 
		SceneManager.LoadScene(nextLevelID);
	}
}
