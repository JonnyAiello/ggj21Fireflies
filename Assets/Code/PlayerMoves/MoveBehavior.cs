using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveBehavior : MonoBehaviour {
    
    abstract public void Init();
    abstract public bool IsExclusive();		// this move ignores effects of following moves in the list
	abstract public bool ZeroMovement();	// this sets the velocity at 0,0 before applying affects
    // abstract public bool IsActive();		// determines if the move is contributing
    abstract public bool AffectsForce();
    abstract public Vector2 GetForce(); 
    abstract public bool AffectsHLimits();
    abstract public bool AffectsVLimits();
    abstract public Vector2 GetHLimits();
    abstract public Vector2 GetVLimits();  
    abstract public bool AffectsPosition(); 
    abstract public Vector2 GetPosition(); 

    /*
    public override bool IsExclusive(){ return false; }
	public override bool ZeroMovement(){ return false; }
    public override bool IsActive(){ return false; }
    public override bool AffectsForce(){ return false; }
    public override Vector2 GetForce(){ return Vector2.zero; } 
    public override bool AffectsHLimits(){ return false; }
    public override bool AffectsVLimits(){ return false; }
    public override Vector2 GetHLimits(){ return Vector2.zero; }
    public override Vector2 GetVLimits(){ return Vector2.zero; }  
    public override bool AffectsPosition(){ return false; } 
    public override Vector2 GetPosition(){ return Vector2.zero; } 
    */
}
