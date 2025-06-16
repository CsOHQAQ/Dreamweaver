using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class AttachableObject : MonoBehaviour
{
    public bool Movable=true;
    public List<Transform> Sockets;
    [HideInInspector]
    public bool isCalledCollisionCallBack=false;

    protected CollisionEventType _collisionEventType = CollisionEventType.None;
    public CollisionEventType colEvent
    {
        get
        {
            return _collisionEventType;
        }
    }
    public enum CollisionEventType
    {
        None,
        summon, 

    }

    public Transform GetClosestSocket(Vector3 iPos)
    {
        if (Sockets.Count == 0)
        {
            Debug.Log($"{gameObject.name}û�п��õ�socket");
            return null;
        }
        Transform socket = Sockets[0];
        float distance = 19260817;

        foreach (Transform t in socket.transform) 
        {
            if (Vector3.Distance(t.position, iPos) <= distance)
            {
                socket = t;
                distance = Vector3.Distance(t.position, iPos);
            }
        }
        return socket;
    }
     
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        var ao=collision.gameObject.GetComponent<AttachableObject>();
        if (ao != null)
        {
            CollisionEvent(ao);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BridgeTrigger"))
        {
            Debug.Log("Test");
        }
    }

    protected virtual void CollisionEvent(AttachableObject other)
    {
    }
}
