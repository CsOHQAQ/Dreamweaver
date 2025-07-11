using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AO_PullLever : AttachableObject
{
    public List< FloatPlatform> platforms;
    [SerializeField] private float forceThreshold = 0;
    public override void OnPulled(float force = 0)
    {
        base.OnPulled();
        Debug.Log($"Lever pull force {force}");
        foreach (var platform in platforms)
        {
            if (platform.enabled == false || force < forceThreshold) continue;
            platform.IsPulling = true;
        }
    }
}
