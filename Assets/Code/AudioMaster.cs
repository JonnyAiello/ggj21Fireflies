using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMaster : MonoBehaviour{

	// Variables
	public static AudioMaster active;

	// Reference Variables
	public AudioSource music;

	// [[ ----- AWAKE ----- ]]
	private void Awake(){
		if( active == null ){ active = this; }
		else{ Destroy(this); }
	}

	// [[ ----- START ----- ]]
	private void Start(){
		music.Play();
	}

}
