using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TE_Cpt1_Collaspe : TriggerEventBase
{
    public GameObject fallGO;
    public override bool OnTriggered(EventTrigger sender, object args)
    {
        fallGO.transform.rotation = Quaternion.Euler(-90f, fallGO.transform.rotation.eulerAngles.y, fallGO.transform.rotation.eulerAngles.z);
        return true;
    }
}
