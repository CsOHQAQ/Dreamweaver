using System;
using UnityEngine;

public class EventManager : BaseManager<EventManager>
{
    public event Action<BaseControllable> OnSwitchControl;
    public event Action OnDreamBodyFinish;

    public event Action OnCameraBlendFinish;

    public void TriggerSwitchControl(BaseControllable newTarget)
    {
        OnSwitchControl?.Invoke(newTarget);
    }

    public void TriggerMissionFinish() => OnDreamBodyFinish?.Invoke();
    public void TriggerCameraBlendFinish() => OnCameraBlendFinish?.Invoke();
}
