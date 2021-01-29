using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyHover : MonoBehaviour {

	// Variables
	private bool isActive;
	public float angleInc = 0.05f;
	public float radInc = 0.05f;
	public float maxRadius = .5f;
	private float radius;
	private float angle = 0;
	private bool growRadius;

	// Reference Variables
	public Transform ffTrans;

    // Start is called before the first frame update
    void Start(){
        radius = maxRadius; 
        growRadius = false;
        SetActive( true );
        // ffTrans.GetComponent<Animation>().Play();
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
		 
		    angle += angleInc * Mathf.Rad2Deg * Time.deltaTime;        
		}
    }

    // [[ ----- SET ACTIVE ----- ]]
    public void SetActive( bool _val ){
    	if( !(_val == isActive) ){
    		isActive = _val;

    		ffTrans.gameObject.SetActive(isActive); 
    	}
    }
}
