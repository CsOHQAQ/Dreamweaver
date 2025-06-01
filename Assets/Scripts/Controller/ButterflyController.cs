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
        FinishDreambody();
    }
}
