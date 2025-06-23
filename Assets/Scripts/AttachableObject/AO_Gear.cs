using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AO_Gear : AttachableObject
{
    public bool IsRunning = true;
    public float RotateSpeed;
    public float PullForce;
    public float PullSpeed;
    // Update is called once per frame
    void Update()
    {
        if (IsRunning)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles+new Vector3(0,RotateSpeed*Time.deltaTime,0));
        }
    }
}
