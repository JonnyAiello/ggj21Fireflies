using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class FallBlock : MonoBehaviour {

	// Variables
	public float triggerDelay = 1f;
	public float resetDelay = 5f; 
	public float cooldownTime = 3f; 
	public float effectLifetime = 1f; 
	private bool touchLock;

	// Reference Variables
	public SpriteRenderer sprite; 
	private BoxCollider2D bc2d; 

	// [[ ----- AWAKE ----- ]]
	private void Awake(){
		bc2d = GetComponent<BoxCollider2D>();
	}

	// [[ ----- MAKE EFFECT BLOCK ----- ]]
	public void MakeEffectBlock(){
		GetComponent<BoxCollider2D>().enabled = false; 
		GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
		Destroy(gameObject, effectLifetime); 

	}

	// [[ ----- COROUTINE - FALL ROUTINE ----- ]]
	public IEnumerator FallRoutine(){
		Debug.Log("Fall Triggered");
		yield return new WaitForSeconds(triggerDelay);
		Debug.Log("Executing Fall"); 
		// make effect block
		GameObject clone = (GameObject)Instantiate(this.gameObject); 
		clone.GetComponent<FallBlock>().MakeEffectBlock(); 
		// deactivate physics
		sprite.gameObject.SetActive(false); 
		bc2d.enabled = false;
		yield return new WaitForSeconds(resetDelay); 
		Debug.Log("Resetting block");
		sprite.gameObject.SetActive(true); 
		bc2d.enabled = true; 
		yield return new WaitForSeconds(cooldownTime);
		Debug.Log("Block active again"); 
		touchLock = false; 
	}

	// [[ ----- ON COLLISION 2D ----- ]]
	private void OnCollisionEnter2D( Collision2D _other ){
		if( !touchLock ){
			if( _other.gameObject.tag == "Player" ){
				touchLock = true; 
				StartCoroutine("FallRoutine");
			}
		}
	}
}
