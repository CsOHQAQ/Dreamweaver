using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Scissor : EquipSkillBase
{
    public override void OnEquip(PlayerController iPlayer)
    {
        base.OnEquip(iPlayer);
    }
    public override bool OnBeginUse(object args = null)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.GetComponent<RopeObject>()!=null)
            {
                GameObject.Destroy(hit.transform.gameObject);                
            }
        }

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
