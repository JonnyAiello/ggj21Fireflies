using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveBehavior : MonoBehaviour {
    
    abstract public void Init( bool _overridden );
        // set up internal move logic, overridden lets higher priority moves
        // cancel other moves in progress
    public virtual bool IsExclusive(){ return false; }		
        // this move ignores effects of following moves in the list
	public virtual bool ZeroMovement(){ return false; }	    
        // this sets the velocity at 0,0 before applying affects
    public virtual bool AffectsForce(){ return false; }
    public virtual Vector2 GetForce(){ return Vector2.zero; } 
    public virtual bool AffectsHLimits(){ return false; }
    public virtual bool AffectsVLimits(){ return false; }
    public virtual Vector2 GetHLimits(){ return Vector2.zero; } 
    public virtual Vector2 GetVLimits(){ return Vector2.zero; } 
    public virtual bool AffectsPosition(){ return false; }
    public virtual Vector2 GetPosition(){ return Vector2.zero; } 
}
