using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float FollowSpeed;

    Vector3 offsetPosition;
    Transform followObj;

    // Start is called before the first frame update
    void Start()
    {
        followObj = GameObject.Find("Player").transform;
        offsetPosition= transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        FollowTarget();

    }

    void FollowTarget()
    {
        Vector3 deltaPos=followObj.position+offsetPosition;
        transform.position = Vector3.MoveTowards(transform.position, deltaPos, FollowSpeed*Time.deltaTime);

    }
}
