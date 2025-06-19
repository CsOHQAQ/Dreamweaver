using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AO_BridgePiece : AttachableObject
{
    private RopeObject ropeObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BridgeTrigger") && !gameObject.CompareTag("Player"))
        {
            CutRope(ropeObject);
            gameObject.layer = LayerMask.NameToLayer("Ground");
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
        yield return null;
    }
}
