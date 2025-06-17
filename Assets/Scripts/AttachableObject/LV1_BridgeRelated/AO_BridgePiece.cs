using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AO_BridgePiece : AttachableObject
{
    // public event Action OnTriggerAreaEnter;
    private RopeObject ropeObject;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BridgeTrigger") && !gameObject.CompareTag("Player"))
        {
            Debug.Log("Now piece enters the trigger");
            CutRope(ropeObject);
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
        Debug.Log("Test Destroy");
        rope.InstantBreak();
        RopeObjectUnset();
    }
}
