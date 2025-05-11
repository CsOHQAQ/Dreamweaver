using System;
using UnityEngine;

public class EventManager : BaseManager<EventManager>
{
    public static Action<BaseControllable> OnSwitchControl;

    public static void TriggerSwitchControl(BaseControllable newTarget) 
    {
        OnSwitchControl?.Invoke(newTarget);
    }
}
