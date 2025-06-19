using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AO_PullLever : AttachableObject
{
    public FloatPlatform platform;
    public override void OnPulled()
    {
        base.OnPulled();

        platform.IsPulling = true;

    }
}
