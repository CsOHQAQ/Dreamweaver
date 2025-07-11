using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AO_Summon : AttachableObject
{
    public GameObject SummonObj;
    
    public float ScaleIndex;

    AO_Summon()
    {
        _collisionEventType=CollisionEventType.summon;
    }

    protected override void CollisionEvent(AttachableObject other)
    {
        base.CollisionEvent(other);
        if (this.isCalledCollisionCallBack || other.isCalledCollisionCallBack)
            return;

        isCalledCollisionCallBack = true;
        other.isCalledCollisionCallBack = true;
        
        AO_Summon test= other as AO_Summon;
        if (test==null)
        {
            isCalledCollisionCallBack = false;
            other.isCalledCollisionCallBack = false;
            return;
        }

        if (other.colEvent != CollisionEventType.summon) 
            return;

        GameObject go = GameObject.Instantiate(SummonObj);
        go.transform.position = (other.transform.position + this.transform.position) / 2;
        go.transform.localScale=this.transform.localScale*ScaleIndex;
        GameObject.Destroy(this.gameObject);
        GameObject.Destroy(other.gameObject);
    }

}
