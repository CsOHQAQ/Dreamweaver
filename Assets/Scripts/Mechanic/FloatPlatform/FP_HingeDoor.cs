using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FP_HingeDoor : FloatPlatform
{
    [Header("Door Angle")]
    public float openAngle = 90.0f;
    public float closeAngle = 0f;

    [Header("Rotation Speed")]
    public float rotationSpeed = 5f;

    // private float targetAngle => IsPulling ? openAngle : closeAngle;
    private float currentAngle;

    void Start()
    {
        currentAngle = transform.localEulerAngles.y;
    }

    protected override void StepForward()
    {
        // base.StepForward();
        float currY = NormalizeAngle(transform.localEulerAngles.y);
        float angleDiff = Mathf.Abs(Mathf.DeltaAngle(currY, openAngle));
        if (angleDiff < 1f) return;

        float desireY = Mathf.MoveTowardsAngle(currY, openAngle, rotationSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(0f, desireY, 0f);
    }

    protected override void StepBackward()
    {
        // base.StepBackward();
        float currY = NormalizeAngle(transform.localEulerAngles.y);
        float angleDiff = Mathf.Abs(Mathf.DeltaAngle(currY, closeAngle));
        if (angleDiff < 1f) return;

        float desireY = Mathf.MoveTowardsAngle(currY, closeAngle, rotationSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(0f, desireY, 0f);
    }

    private float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle > 180f) angle -= 360f;
        return angle;
    }
}
