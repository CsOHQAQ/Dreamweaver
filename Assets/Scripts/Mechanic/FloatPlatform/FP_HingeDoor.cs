using System.Collections;
using System.Collections.Generic;
// using System.Numerics;
using Unity.Mathematics;
using UnityEngine;

public class FP_HingeDoor : FloatPlatform
{
    [Header("Door Angle")]
    public float openAngle = 90.0f;
    public float closeAngle = 0f;

    // private float targetAngle => IsPulling ? openAngle : closeAngle;
    private float currentAngle;
    private Vector3 originalRotation;

    void Start()
    {
        currentAngle = Platform.localEulerAngles.y;
        originalRotation = Platform.localEulerAngles;
    }

    protected override void StepForward()
    {
        float currY = NormalizeAngle(Platform.localEulerAngles.y);
        float angleDiff = Mathf.Abs(Mathf.DeltaAngle(currY, openAngle));
        if (angleDiff < 1f) return;

        float desireY = Mathf.MoveTowardsAngle(currY, openAngle, MoveSpeed * Time.deltaTime);
        Platform.localRotation = Quaternion.Euler(originalRotation.x, desireY, originalRotation.z);
    }

    protected override void StepBackward()
    {
        float currY = NormalizeAngle(Platform.localEulerAngles.y);
        float angleDiff = Mathf.Abs(Mathf.DeltaAngle(currY, closeAngle));
        if (angleDiff < 1f) return;

        float desireY = Mathf.MoveTowardsAngle(currY, closeAngle, MoveSpeed * Time.deltaTime);
        Platform.localRotation = Quaternion.Euler(originalRotation.x, desireY, originalRotation.z);
    }

    private float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle > 180f) angle -= 360f;
        return angle;
    }
}
