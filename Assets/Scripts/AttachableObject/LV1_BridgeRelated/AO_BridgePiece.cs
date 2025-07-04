using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AO_BridgePiece : AttachableObject
{
    [SerializeField] private RopeObject ropeObject;
    [SerializeField] private Transform targetPos;
    [SerializeField][Range(0f, 5f)] private float smoothTime = 0.3f;

    [SerializeField] private Vector3 velocity = Vector3.zero;
    [SerializeField] private float angleVelocity = 0f;
    private Coroutine moveCoroutine;

    public event Action OnArriveAtTarget = () => Debug.Log("Move Finished");

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BridgeTrigger") && !gameObject.CompareTag("Player"))
        {
            CutRope();
            gameObject.layer = LayerMask.NameToLayer("Ground");
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            Movable = false;
            if (moveCoroutine != null) StopCoroutine(moveCoroutine);
            moveCoroutine = StartCoroutine(MoveToPosition());
        }
    }

    public override void RopeObjectSetUp(RopeObject rope)
    {
        base.RopeObjectSetUp(rope);
        ropeObject = rope;
    }

    public override void RopeObjectUnset()
    {
        base.RopeObjectUnset();
        ropeObject = null;
    }

    private void CutRope()
    {
        if (ropeObject == null) return;
        var rope = ropeObject;
        RopeObjectUnset();
        rope.InstantBreak();
    }

    private IEnumerator MoveToPosition()
    {
        Vector3 originalEuler = transform.eulerAngles;
        while (Vector3.Distance(targetPos.position, gameObject.transform.position) > 0.01f)
        {
            if (Vector3.Distance(targetPos.position, gameObject.transform.position) > 0.01f)
            {
                transform.position = Vector3.SmoothDamp(
                    transform.position,
                    targetPos.position,
                    ref velocity,
                    smoothTime
                );
            }

            // --- Smooth Rotation ---
            float targetY = targetPos.GetComponentInParent<Transform>().eulerAngles.y;
            float newY = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetY,
                ref angleVelocity,
                smoothTime
            );

            transform.rotation = Quaternion.Euler(originalEuler.x, newY, originalEuler.z);
            yield return null;
        }
        Debug.Log($"Target angle of y is: {targetPos.GetComponentInParent<Transform>().eulerAngles.y}");
        // gameObject.transform.SetPositionAndRotation(targetPos.position, Quaternion.LookRotation(targetPos.GetComponentInParent<Transform>().forward));
        velocity = Vector3.zero;
        angleVelocity = 0f;
        OnArriveAtTarget?.Invoke();
    }
}
