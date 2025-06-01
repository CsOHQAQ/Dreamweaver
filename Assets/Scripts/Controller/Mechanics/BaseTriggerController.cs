using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTriggerController : MonoBehaviour
{
    [SerializeField] private DreamBodyController dreamBody;
    [SerializeField] protected GameObject mechanicGameObj;
    // Start is called before the first frame update
    void Start()
    {
        dreamBody.MissionAccomplished += TriggerMechanic;
        dreamBody.OnBeforeDisable += UnsubEvents;
    }

    void OnDestroy()
    {
        UnsubEvents();
    }

    protected virtual void TriggerMechanic()
    {
        Debug.Log($"{gameObject.name}'s Mechanic is triggered.");
    }

    void UnsubEvents()
    {
        if (dreamBody != null)
        {
            dreamBody.MissionAccomplished -= TriggerMechanic;
            dreamBody.OnBeforeDisable -= UnsubEvents;
        }
    }
}
