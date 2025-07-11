using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a class/script for debug only, 
/// remove it from all classes if attached when it is deliverable version
/// </summary>
public class PrefabDebug : MonoBehaviour
{
    // public Transform originalTransform;
    // Start is called before the first frame update
    void Start()
    {
        transform.forward = Vector3.forward;
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.R))
        // {
        //     Debug.Log($"{gameObject.name} is now returning to original pos");
        //     gameObject.transform.position = originalTransform.position;
        // }
    }
}
