using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyPickup : MonoBehaviour {

	// Variables
	public static List<FireflyPickup> MasterList = new List<FireflyPickup>();
	private bool isActive;
	public float angleInc = 0.05f;
	public float radInc = 0.05f;
	public float maxRadius = .5f;
	private float radius;
	private float angle = 0;
	private bool growRadius;
	private bool swapDirection;

	// Reference Variables
	public Transform ffTrans;

	// [[ ----- AWAKE ----- ]]
	private void Awake(){
		// MasterList.Add(this);
	}

    // [[ ----- START ----- ]]
    void Start(){
    	MasterList.Add(this);
        radius = maxRadius; 
        growRadius = false;
        SetActive( true );
        
        // set random hover cycle
        angle = Random.Range(0, 5);
        swapDirection = (Random.value > 0.5f);
        radius = Random.Range( 0.25f, 0.5f ); 
    }
    

    // Update is called once per frame
    void Update(){

    	if( isActive ){
		   	// change radius
		   	if( radius >= maxRadius ){ growRadius = false; }
		   	if( radius <= (maxRadius * 0.5f) ){ growRadius = true; }

		   	if( growRadius ){ radius += radInc; }
		   	else{ radius -= radInc; }

		    float x = radius * Mathf.Cos(angle);
		    float y = radius * Mathf.Sin(angle);
		 
		    ffTrans.localPosition = new Vector2(x, y);
		 
		 	// reset angles to keep numbers from getting astro
		 	if( !swapDirection && angle > 20 ){ swapDirection = true; }
		 	else if( swapDirection && angle < -20 ){ swapDirection = false; }

		 	if( !swapDirection ){
		    	angle += angleInc * Mathf.Rad2Deg * Time.deltaTime;        
		    }else{
		    	angle -= angleInc * Mathf.Rad2Deg * Time.deltaTime; 
		    }
		}
    }

    // [[ ----- SET ACTIVE ----- ]]
    public void SetActive( bool _val ){
    	if( !(_val == isActive) ){
    		isActive = _val;

    		ffTrans.gameObject.SetActive(isActive); 
    	}
    }

    // [[ ----- ON HIT ----- ]]
    public void OnHit(){
    	SceneMaster.active.UpdateFFCount( 1 ); 
    	AudioMaster.active.SoundEffect(
    		AudioMaster.FXType.Pickup, SceneMaster.active.FOwnedCount);
		SetActive( false ); 
    }
}
