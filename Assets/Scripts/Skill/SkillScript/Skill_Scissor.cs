using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Scissor : EquipSkillBase
{
    public Skill_Scissor()
    {
        _detectLayers = new List<LayerMask>() {
            LayerMask.NameToLayer("Rope"),
            LayerMask.NameToLayer("Attachable Object"),
            LayerMask.NameToLayer("Base Slot"),
        };
    }
    public override void OnEquip(PlayerController iPlayer)
    {
        base.OnEquip(iPlayer);
    }
    public override bool OnBeginUse(object args = null)
    {
        if (args == null) { return false; }

        RaycastHit hit = (RaycastHit)args;

        RopeObject rope = hit.transform.GetComponent<RopeObject>();
        if (rope)
        {
            Debug.Log($"Scissor Detect:{rope.gameObject}");
            rope.InstantBreak(); 
            return true;
        }

        PreservableObject preservable = hit.transform.GetComponent<PreservableObject>();
        if (player.Inventory.obtainPreserve && preservable)
        {
            Debug.Log($"Preserving Object: {preservable.name}");
            player.Inventory.AddPreservableObject(preservable);
            preservable.OnPreserved();
            return true;
        }

        BaseSlotController baseSlot = hit.transform.GetComponent<BaseSlotController>();
        if (player.Inventory.obtainPreserve && baseSlot)
        {
            Debug.Log($"Clicking at base slot: {hit.transform.gameObject.name}");
            if (player.Inventory.HasPreservableObject())
            {
                player.Inventory.PlacePreservableObject(0, baseSlot.PlacePosition.position);
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
