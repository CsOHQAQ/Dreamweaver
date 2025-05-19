using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_RopeTest : EquipSkillBase
{
    public float Range = 200f;
    public float ThrowSpeed = 100f;

    private RopeObject ropeObject;
    private Stage curStage;

    public Skill_RopeTest()
    {
        _detectLayers = new List<LayerMask>() {
            LayerMask.NameToLayer("Attachable Object"),
            LayerMask.NameToLayer("Dream Body"),
        };

    }
    public override void OnEquip(PlayerController iPlayer)
    {
        base.OnEquip(iPlayer);
        CanUse = true;
        ropeObject = null;
        curStage = Stage.NotConnected;
    }
    public override bool OnBeginUse(object args = null)
    {
        if (args == null) { return false; }

        if (!(CanUse && CDTimer <= 0))
        {
            return false;
        }
        RaycastHit hit = (RaycastHit)args;

        AttachableObject obj = hit.transform.GetComponent<AttachableObject>();
        if (obj == null)
            return false;

        var dreamBody = hit.transform.GetComponent<DreamBodyController>();
        if (dreamBody)
        {
            dreamBody.GetComponentInChildren<CinemachineFreeLook>().Priority = 11;
            EventManager.Instance.TriggerSwitchControl(dreamBody);  // Directly trigger by calling the instance. Could change later
            return true;
        }

        switch (curStage)
        {
            case Stage.NotConnected:
                #region Init Rope
                ropeObject = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/RopeTest")).GetComponent<RopeObject>();
                ropeObject.Init(ThrowSpeed, 100, 1000, hit.point, obj, player.transform.position, player.GetComponent<AttachableObject>(), () =>
                {
                    OnCanceled();
                });
                ropeObject.SetLocation(player.transform.position, player.transform.position);

                #endregion

                curStage = Stage.OneSide;

                break;
            case Stage.OneSide:
                if (!ropeObject.isMoving)
                {
                    ropeObject.SetConnect(false, hit.point, obj.GetComponent<AttachableObject>());
                    ropeObject.isMoving = true;
                    ropeObject.isPulling = true;
                    curStage = Stage.BothSide;
                    ropeObject.DelayBreak(7);
                }
                break;
            case Stage.BothSide:
                break;
            default:
                break;
        }
        return true;

    }
    public override void OnUse(object args = null)
    {
        base.OnUse();

        Debug.Log($"Rope's current stage is {curStage}");
        switch (curStage)
        {
            case Stage.NotConnected:
                break;

            case Stage.OneSide:
                if (!ropeObject.isMoving)
                {
                    ropeObject.Pull();
                }
                else
                {
                    Debug.Log("Rope Moving! Pulling Canceled");
                }
                break;

            case Stage.BothSide:

                break;
            default:
                break;
        }

    }

    public override void OnEndUse(object args = null)
    {
        base.OnEndUse();
    }
    public override void OnCanceled(object args = null)
    {
        base.OnCanceled(args);
        curStage = Stage.NotConnected;
        ropeObject = null;
    }

    enum Stage
    {
        NotConnected,
        OneSide,
        BothSide

    }
}
