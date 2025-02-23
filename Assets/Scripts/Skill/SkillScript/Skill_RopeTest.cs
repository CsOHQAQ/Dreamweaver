using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_RopeTest : EquipSkillBase
{
    public float Range=200f;
    public float ThrowSpeed = 100f;

    private RopeObject ropeObject;
    private Stage curStage;
    public override void OnEquip(PlayerController iPlayer)
    {
        base.OnEquip(iPlayer);
        CanUse = true;
        ropeObject = null;
        curStage = Stage.NotConnected;
    }
    public override bool OnBeginUse(object args = null)
    {
        if (!(CanUse && CDTimer <= 0))
        {
            return false;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        
        if(Physics.Raycast(ray, out hit))
        {
            Debug.DrawLine(Camera.main.transform.position,hit.transform.position,Color.red,1);
            AttachableObject obj= hit.transform.GetComponent<AttachableObject>();
            if(obj==null)
                return false;

            var dreamBody = hit.transform.GetComponent<DreamBodyController>();
            if (dreamBody)  {
                dreamBody.GetComponentInChildren<CinemachineFreeLook>().Priority = 11;
                EventManager.TriggerSwitchControl(dreamBody);
                return true;
            }

            switch (curStage)
            {
                case Stage.NotConnected:
                    #region Init Rope
                    ropeObject =GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/RopeTest")).GetComponent<RopeObject>();
                    ropeObject.Speed = ThrowSpeed;
                    ropeObject.clickPos1 = hit.point;
                    ropeObject.connect1 = obj;
                    ropeObject.clickPos2 = player.transform.position;
                    ropeObject.connect2 = player.GetComponent<AttachableObject>();
                    ropeObject.SetLocation(player.transform.position, player.transform.position);
                    ropeObject.isMoving = true;
                    ropeObject.isPulling = false;
                    #endregion

                    curStage = Stage.OneSide;

                    break;
                case Stage.OneSide:
                    if (!ropeObject.isMoving)
                    {
                        ropeObject.clickPos2 = hit.point;
                        ropeObject.connect2 = obj.GetComponent<AttachableObject>();
                        ropeObject.isMoving = true;
                        ropeObject.isPulling= true;
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

        return false;
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
        GameObject.Destroy( ropeObject?.gameObject );
    }

    enum Stage
    {
        NotConnected,
        OneSide,
        BothSide

    }
}
