using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionGUI : MonoBehaviour{

	// Reference Variables
	public Image[] ffIconArray;


// -----------------------------------------------------------------------------
// Public Methods

    // [[ ----- SET GUI ----- ]]
    public void SetGUI( int _val ){
    	for( int i = 0; i < ffIconArray.Length; i++ ){
    		if( i < _val ){ ffIconArray[i].gameObject.SetActive(true); }
    		else{ ffIconArray[i].gameObject.SetActive(false); }
    	}
    }
}
