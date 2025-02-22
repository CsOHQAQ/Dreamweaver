using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 该类作为玩家装备技能类的基类
/// </summary>

public abstract class EquipSkillBase
{
    public float CD;
    protected float CDTimer;

    public bool CanUse;


    protected PlayerController player;
    public virtual void OnEquip(PlayerController iPlayer)
    {
        player= iPlayer;
    }

    public virtual bool OnBeginUse(object args=null)
    {

        return true;
    }

    public virtual void OnUse(object args = null)
    {

    }

    public virtual void OnEndUse(object args = null)
    {

    }

    public virtual void OnCanceled(object args=null)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
