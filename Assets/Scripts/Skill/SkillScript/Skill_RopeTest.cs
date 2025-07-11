using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Skill_RopeTest : EquipSkillBase
{
    public float DetectRange = 7f;
    public float MaxLength = 10f;
    public float ThrowSpeed = 50f;

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

        if (!hit.transform.TryGetComponent<AttachableObject>(out var obj))
            return false;

        //Face direction judge
        float facingAngle = Vector3.Angle(obj.transform.position - player.transform.position, player.transform.forward);
        if (facingAngle > 30f)
        {
            Debug.Log($"Skill used failed! Angle {facingAngle}");
            return false;
        }

        //Check the range
        if (Vector3.Distance(obj.transform.position, player.transform.position) > DetectRange)
        {
            Debug.Log($"Object out of range! Current range is {Vector3.Distance(obj.transform.position, player.transform.position)}");
            return false;
        }

        var dreamBody = hit.transform.GetComponent<DreamBodyController>();
        if (dreamBody)
        {
            EventManager.Instance.TriggerSwitchControl(dreamBody);  // Directly trigger by calling the instance. Could change later
            return true;
        }

        switch (curStage)
        {
            case Stage.NotConnected:
                #region Init Rope
                ropeObject = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/RopeTest")).GetComponent<RopeObject>();
                if(
                    ropeObject.Init(ThrowSpeed, MaxLength, hit.point, obj, player.transform.position, player.GetComponent<AttachableObject>(), () =>
                    {
                        OnCanceled();
                    })
                )
                {
                    
                    curStage = Stage.OneSide;
                }
                else
                {
                    return false;
                }
                #endregion

                    break;


            case Stage.OneSide:
                if (obj.GetComponent<Door>() != null)
                {
                    
                }
                else if (!ropeObject.isMoving)
                {
                    if (ropeObject.SetConnect(false, hit.point, obj.GetComponent<AttachableObject>()))
                    {
                        ropeObject.isMoving = true;
                        ropeObject.isPulling = true;
                        curStage = Stage.BothSide;
                        ropeObject.DelayBreak(7);
                    }
                    else
                    {
                        return false;
                    }

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
