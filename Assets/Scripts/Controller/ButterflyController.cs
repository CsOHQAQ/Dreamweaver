using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyController : DreamBodyController
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Triggerable"))
            OnFinishMission();
    }

    public override void OnFinishMission()
    {
        base.OnFinishMission();
        gameObject.SetActive(false);
    }
}
