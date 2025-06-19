using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Scissor : EquipSkillBase
{
    public Skill_Scissor()
    {
        _detectLayers = new List<LayerMask>() {
            LayerMask.NameToLayer("Rope"),
        };
    }
    public override void OnEquip(PlayerController iPlayer)
    {
        base.OnEquip(iPlayer);
    }
    public override bool OnBeginUse(object args = null)
    {
        if (args == null)
        {
            return false;
        }

        RopeObject rope= ((RaycastHit)args).transform.GetComponent<RopeObject>();
        if (rope == null)
        {
            return false;
        }

        
        Debug.Log($"Scissor Detect:{rope.gameObject}");
        GameObject.Destroy(rope.gameObject);        

        return base.OnBeginUse(args);
    }
    public override void OnUse(object args = null)
    {
        base.OnUse(args);
    }
    public override void OnEndUse(object args = null)
    {
        base.OnEndUse(args);
    }
    public override void OnCanceled(object args = null)
    {
        base.OnCanceled(args);
    }
}
