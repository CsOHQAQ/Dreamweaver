using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class AttachableObject : MonoBehaviour
{
    public bool Movable=true;
    public List<Transform> Sockets;

    public Transform GetClosestSocket(Vector3 iPos)
    {
        if (Sockets.Count == 0)
        {
            Debug.Log($"{gameObject.name}没有可用的socket");
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
}
