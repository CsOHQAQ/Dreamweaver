using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AO_BridgePiece : AttachableObject
{
    private RopeObject ropeObject;
    [SerializeField] private Transform originPos;
    [SerializeField][Range(0f, 5f)] private float smoothTime = 0.3f;

    [SerializeField] private Vector3 velocity = Vector3.zero;
    private Coroutine moveCoroutine;

    public event Action OnArriveAtOrigin = () => Debug.Log("Move Finished");

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BridgeTrigger") && !gameObject.CompareTag("Player"))
        {
            CutRope(ropeObject);
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

    private void CutRope(RopeObject rope)
    {
        rope.InstantBreak();
        RopeObjectUnset();
    }

    private IEnumerator MoveToPosition()
    {

        while (Vector3.Distance(originPos.position, gameObject.transform.position) > 0.01f)
        {
            transform.position = Vector3.SmoothDamp(
                transform.position,
                originPos.position,
                ref velocity,
                smoothTime
            );
            yield return null;
        }
        gameObject.transform.position = originPos.position;
        velocity = Vector3.zero;
        OnArriveAtOrigin?.Invoke();
    }
}
