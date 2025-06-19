using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceByKey : MonoBehaviour
{

    public float forceStrength;
    public Vector3 forceDir;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            GetComponent<Rigidbody>().AddForce(forceDir*forceStrength);
        }
    }
}
