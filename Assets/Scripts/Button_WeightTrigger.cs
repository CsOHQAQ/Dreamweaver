using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_WeightTrigger : MonoBehaviour
{
    public float TriggerWeight;
    public Door DemoDoor;

    [SerializeField]
    float curWeight;

    private void Start()
    {
        curWeight = 0f;
    }

    private void Update()
    {
        if (curWeight >=TriggerWeight) 
        {
            DemoDoor.Open();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            curWeight += other.attachedRigidbody.mass;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            curWeight -= other.attachedRigidbody.mass;
        }
    }
}
