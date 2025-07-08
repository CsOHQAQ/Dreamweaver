using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AO_PullLever : AttachableObject
{
    public FloatPlatform platform;
    [SerializeField] private float forceThreshold = 0;
    public override void OnPulled(float force = 0)
    {
        base.OnPulled();
        if (platform.enabled == false || force < forceThreshold) return;
        platform.IsPulling = true;

    }
}
