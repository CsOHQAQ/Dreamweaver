using System;
using UnityEngine;

public class EventManager : BaseManager<EventManager>
{
    public event Action<BaseControllable> OnSwitchControl;
    public event Action OnDreamBodyFinish;      // Trigger this when the Dreambody finish the job, can trigger some music etc.

    public event Action OnCameraBlendFinish;    // Trigger this when camera finish transitioning, this can activate control etc.

    public void TriggerSwitchControl(BaseControllable newTarget)
    {
        OnSwitchControl?.Invoke(newTarget);
    }

    public void TriggerMissionFinish() => OnDreamBodyFinish?.Invoke();
    public void TriggerCameraBlendFinish() => OnCameraBlendFinish?.Invoke();
}
