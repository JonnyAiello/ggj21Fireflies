using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionGUI : MonoBehaviour{

	// Reference Variables
    public Color activeColor;
    public Color spentColor;
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

    // [[ ----- SET AVAILABLE POWER GUI ----- ]]
    public void SetAvailablePowerGUI( int _spentPower, int _ffs ){
    	int spentPower = _ffs - _spentPower;
    	for( int i = 0; i < ffIconArray.Length; i++ ){
    		if( i < _spentPower ){ ffIconArray[i].color = activeColor; }
    		else{ ffIconArray[i].color = spentColor; }
    	}
    }

    // [[ ----- RESET AVAILABLE POWER GUI ----- ]]
    public void ResetAvailablePowerGUI(){
    	for( int i = 0; i < ffIconArray.Length; i++ ){
    		ffIconArray[i].color = activeColor;
    	}
    }
}
