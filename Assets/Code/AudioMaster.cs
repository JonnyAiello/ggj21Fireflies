using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMaster : MonoBehaviour{

	// Variables
	public static AudioMaster active;
	public float musicVol;
	public float musicDuckVol;
	private double trackChangeTime;
	private double trackQueueTime;
	private AudioClip aTrack;
	private AudioClip bTrack;
	// private bool playingA;
	private AudioSource currentlyPlaying;
	private bool queueLock; 
	private int loop2Count; 

	// Test
	private double testLength = 4; 

	// Reference Variables
	public AudioSource audioA;
	public AudioSource audioB;
	public AudioSource audioC; // for sound effects
	public AudioClip musicIntro;
	public AudioClip musicLoop1pt1;
	public AudioClip musicLoop1pt2;
	public AudioClip musicLoop1pt3;
	public AudioClip musicLoop1pt4;
	public AudioClip musicLoop1pt5;
	public AudioClip musicLoop1pt6;
	public AudioClip musicLoop2;
	public AudioClip deathFx;
	public AudioClip checkpointFx;
	public AudioClip[] jumpFx;
	public AudioClip[] pickupFx;

	// enum
	public enum FXType{
		Jump,
		Pickup,
		Death,
		Checkpoint
	}

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
		loop2Count = 0; 
	}

	// [[ ----- UPDATE ----- ]]
	private void Update(){

		// check to queue track
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

	// [[ ----- QUEUE TRACK ----- ]]
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
			track2Q = musicLoop1pt1;

		}else if( trackPlaying == musicLoop1pt1 ){
			if( SceneMaster.active.DoorsUnlocked > 0 ){ track2Q = musicLoop2; }
			else{ track2Q = musicLoop1pt2; }

		}else if( trackPlaying == musicLoop1pt2 ){
			if( SceneMaster.active.DoorsUnlocked > 0 ){ track2Q = musicLoop2; }
			else{ track2Q = musicLoop1pt3;}

		}else if( trackPlaying == musicLoop1pt3 ){
			if( SceneMaster.active.DoorsUnlocked > 0 ){ track2Q = musicLoop2; }
			else{ track2Q = musicLoop1pt5;}

		/*}else if( trackPlaying == musicLoop1pt4 ){
			track2Q = musicLoop1pt5;*/

		}else if( trackPlaying == musicLoop1pt5 ){
			if( SceneMaster.active.DoorsUnlocked > 0 ){ track2Q = musicLoop2; }
			else{ track2Q = musicLoop1pt6;}
		
		}else if( trackPlaying == musicLoop1pt6 ){
			if( SceneMaster.active.DoorsUnlocked > 0 ){ track2Q = musicLoop2; }
			else{ track2Q = musicLoop2;}
		
		}else if( trackPlaying == musicLoop2 ){
			if( loop2Count >= 2 ){ 
				track2Q = musicLoop1pt2; 
				loop2Count = 0; 
			}
			else{ 
				track2Q = musicLoop2;
				loop2Count++;
			}
		}

/*
		if( trackPlaying == musicIntro ){
			track2Q = musicLoop1; 
		}else if( trackPlaying == musicLoop1 ){
			if( SceneMaster.active.DoorsUnlocked > 0 ){ track2Q = musicLoop2; }
			else{ track2Q = musicLoop1; }
		}else if( trackPlaying == musicLoop2 ){
			if( SceneMaster.active.DoorsUnlocked > 1 ){ track2Q = musicLoop1; }
			track2Q = musicLoop2;
		}
*/
		// add track to audio source
		if( currentlyPlaying == audioA ){ bTrack = track2Q; }
		else{ aTrack = track2Q; }
		source2Q.clip = track2Q; 

		Debug.Log("Queued to play next: " + track2Q);
	}

	private void Unduck(){
		audioA.volume = musicVol; 
		audioB.volume = musicVol; 
	}

// -----------------------------------------------------------------------------
// Public Methods


	// [[ ----- SOUND EFFECT ----- ]]
	public void SoundEffect( FXType _type, int _num ){
		float duckTime = 0;

		switch( _type ){
			case FXType.Jump:
				int j = Mathf.Clamp(jumpFx.Length, 1, _num) - 1;
				audioC.PlayOneShot(jumpFx[j]);
				break;
			
			case FXType.Pickup:
				int f = Mathf.Clamp(pickupFx.Length, 1, _num) - 1;
				audioC.PlayOneShot(pickupFx[f]);
				break;
			
			case FXType.Death:
				duckTime = 3.25f;
				audioC.PlayOneShot( deathFx ); 
				break;
			
			case FXType.Checkpoint:
				duckTime = 3f;
				audioC.PlayOneShot( checkpointFx ); 
				break;
		}

		// duck audio
		if( duckTime > 0 ){
			audioA.volume = musicDuckVol; 
			audioB.volume = musicDuckVol; 
			Invoke( "Unduck", duckTime ); 
		}
	}

}
