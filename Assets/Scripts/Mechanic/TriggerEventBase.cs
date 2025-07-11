using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 这个是用于让EventTrigger 触发的事件的基类
/// </summary>
public class TriggerEventBase : MonoBehaviour
{
    public virtual bool OnTriggered(EventTrigger sender,object args=null)
    {
        return true;
    }
}
