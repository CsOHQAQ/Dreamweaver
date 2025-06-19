using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AO_BridgePiece : AttachableObject
{
    private RopeObject ropeObject;
    [SerializeField] private Transform originPos;
    [SerializeField][Range(0f, 10f)] private float moveSpeed = 2.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BridgeTrigger") && !gameObject.CompareTag("Player"))
        {
            CutRope(ropeObject);
            gameObject.layer = LayerMask.NameToLayer("Ground");
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            StartCoroutine(MoveToPosition());
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
        while (Vector3.Distance(originPos.position, gameObject.transform.position) > 0.05f)
        {
            gameObject.transform.position = Vector3.MoveTowards(
                gameObject.transform.position,
                originPos.position,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }
        gameObject.transform.position = originPos.position;
    }
}
