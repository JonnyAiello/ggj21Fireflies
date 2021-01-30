using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMaster : MonoBehaviour{

	// Variables
	public static AudioMaster active;
	private double trackChangeTime;
	private double trackQueueTime;
	private AudioClip aTrack;
	private AudioClip bTrack;
	// private bool playingA;
	private AudioSource currentlyPlaying;
	private bool queueLock; 

	// Test
	private double testLength = 4; 

	// Reference Variables
	public AudioSource audioA;
	public AudioSource audioB;
	public AudioClip musicIntro;
	public AudioClip musicLoop1;
	public AudioClip musicLoop2;

	// [[ ----- AWAKE ----- ]]
	private void Awake(){
		if( active == null ){ active = this; }
		else{ Destroy(this); }
	}

	// [[ ----- START ----- ]]
	private void Start(){
		trackChangeTime = AudioSettings.dspTime + musicIntro.length;
		trackQueueTime = trackChangeTime - 1;  
		currentlyPlaying = audioA;
		aTrack = musicIntro; 
		audioA.Play(); 
	}

	// [[ ----- UPDATE ----- ]]
	private void Update(){


		if( AudioSettings.dspTime > trackQueueTime && !queueLock ){
			queueLock = true; 
			QueueTrack(); 
		}

		// switch tracks
		if( currentlyPlaying == audioA ){
			if( AudioSettings.dspTime > trackChangeTime ){
				currentlyPlaying = audioB;
				trackChangeTime = AudioSettings.dspTime + bTrack.length;
				trackQueueTime = trackChangeTime - 1; 
				queueLock = false;
				audioB.Play();
				audioA.Stop();
				Debug.Log("Playing audio source B");
			}
		}else if( currentlyPlaying == audioB ){
			if( AudioSettings.dspTime > trackChangeTime ){
				currentlyPlaying = audioA;
				trackChangeTime = AudioSettings.dspTime + aTrack.length;
				trackQueueTime = trackChangeTime - 1; 
				queueLock = false;
				audioB.Stop();
				audioA.Play();
				Debug.Log("Playing audio source A");
			}
		}
		
	}



	private void QueueTrack(){
		AudioSource source2Q = null;
		AudioClip track2Q = null;
		AudioClip trackPlaying = null;

		if( currentlyPlaying == audioA ){ 
			trackPlaying = aTrack; 
			source2Q = audioB; 
		}
		else{ 
			trackPlaying = bTrack;
			source2Q = audioA; 
		}

		// determine what comes next based on game state
		if( trackPlaying == musicIntro ){
			track2Q = musicLoop1; 
		}else if( trackPlaying == musicLoop1 ){
			if( SceneMaster.active.DoorsUnlocked > 0 ){ track2Q = musicLoop2; }
			else{ track2Q = musicLoop1; }
		}else if( trackPlaying == musicLoop2 ){
			if( SceneMaster.active.DoorsUnlocked > 1 ){ track2Q = musicLoop1; }
			track2Q = musicLoop2;
		}

		// add track to audio source
		if( currentlyPlaying == audioA ){ bTrack = track2Q; }
		else{ aTrack = track2Q; }
		source2Q.clip = track2Q; 

		Debug.Log("Queued to play next: " + track2Q);
	}

}
