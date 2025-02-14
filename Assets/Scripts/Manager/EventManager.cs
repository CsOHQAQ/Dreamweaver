using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set;}
    public static Action<BaseControllable> OnSwitchControl;

    public static void TriggerSwitchControl(BaseControllable newTarget) 
    {
        OnSwitchControl?.Invoke(newTarget);
    }
}
