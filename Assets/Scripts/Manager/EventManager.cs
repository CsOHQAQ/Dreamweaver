using System;
using UnityEngine;

public class EventManager : BaseManager<EventManager>
{
    public event Action<BaseControllable> OnSwitchControl;
    public event Action OnDreamBodyFinish = () => Debug.Log("Finished");

    public void TriggerSwitchControl(BaseControllable newTarget)
    {
        OnSwitchControl?.Invoke(newTarget);
    }

    public void TriggerMissionFinish() => OnDreamBodyFinish?.Invoke();
}
