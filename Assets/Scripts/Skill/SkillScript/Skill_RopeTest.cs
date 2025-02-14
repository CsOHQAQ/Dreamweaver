using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_RopeTest : EquipSkillBase
{
    public float Range=200f;
    public float ThrowSpeed = 10f;

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

                    ropeObject.connect1 = obj;
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


    enum Stage
    {
        NotConnected,
        OneSide,
        BothSide
    }
}
