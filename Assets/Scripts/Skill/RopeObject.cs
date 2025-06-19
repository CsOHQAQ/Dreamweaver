using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RopeObject : MonoBehaviour
{
    public bool isMoving;
    public bool isPulling;

    float Speed;
    float Length;
    LineRenderer line;
    AttachableObject connect1=null, connect2=null;


    Vector3 clickPos1, clickPos2;
    public float PullForce;
    private Action DestoryCallback;
    private CapsuleCollider capCollider;

    public void Init(float iSpeed, float iLength,Vector3 iClickPos1, AttachableObject iConnect1, Vector3 iClickPos2, AttachableObject iConnect2, Action iCallback)
    {
        Speed = iSpeed;
        Length = iLength;
        clickPos1 = iClickPos1;
        clickPos2 = iClickPos2;
        connect1 = iConnect1;
        connect1.RopeObjectSetUp(this);
        connect2 = iConnect2;
        connect2.RopeObjectSetUp(this);
        DestoryCallback = iCallback;
        capCollider = GetComponentInChildren<CapsuleCollider>();
        line = GetComponent<LineRenderer>();
        isMoving = true;
        isPulling = false;
    }

    public void SetLocation(Vector3 point1,Vector3 point2)
    {
        line.SetPosition(0,point1);
        line.SetPosition(1,point2);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(connect1==null||connect2==null)
        {
            Destroy(gameObject);
            return;
        }
        
        line.SetPosition(0, Vector3.Lerp(line.GetPosition(0), connect1.GetClosestSocket(clickPos1).position,Speed*Time.deltaTime));
        line.SetPosition(1, Vector3.Lerp(line.GetPosition(1), connect2.GetClosestSocket(clickPos2).position, Speed* Time.deltaTime));

        Vector3 pos0= line.GetPosition(0),pos1=line.GetPosition(1);
        transform.position = (pos0 + pos1) / 2;
        // capCollider.transform.position = (pos0 + pos1) / 2;
        capCollider.height=Vector3.Distance(pos0,pos1);
        capCollider.transform.rotation = Quaternion.FromToRotation(Vector3.up,pos1-pos0);

        if (isMoving)
        {
            if (Vector3.Distance(line.GetPosition(0), connect1.GetClosestSocket(clickPos1).position) <0.05f&& Vector3.Distance(line.GetPosition(1), connect2.GetClosestSocket(clickPos2).position) < 0.05f)
            {
                isMoving = false;
            }
        }
        if (isPulling)
        {
            //Check gear
            AO_Gear gear=null;
            if(connect1 is AO_Gear)
                gear= (AO_Gear)connect1;
            else if(connect2 is AO_Gear)
                gear= (AO_Gear)connect2;

            if (gear!=null)
            {
                if (gear.IsRunning)
                    Pull(gear.PullForce);
            }
            Pull();
        }
    }

    public void DelayBreak(float time)
    {
        StartCoroutine(DelayDie(time));
    }

    IEnumerator DelayDie(float time)
    {
        Debug.Log("Rope Wait for dying");
        yield return new WaitForSeconds(time);
        GameObject.Destroy(gameObject);
        yield return null;
    }

    public void InstantBreak()
    {
        Destroy(gameObject);
    }

    public void SetConnect(bool isConnect1, Vector3 iClickPos, AttachableObject iObj)
    {
        if (isConnect1)
        {
            clickPos1 = iClickPos;
            connect1 = iObj;
        }
        else
        {
            clickPos2 = iClickPos;
            connect2 = iObj;
        }
    }

    public void Pull(float pullforce = -1)
    {
        if (pullforce == -1)
            pullforce = PullForce;

        connect1.OnPulled();
        connect2.OnPulled();

        Vector3 direction = (connect1.GetClosestSocket(clickPos1).position - connect2.GetClosestSocket(clickPos2).position).normalized;
        if (connect1.Movable&& connect1.GetComponent<Rigidbody>()!=null)
        {            
            connect1.GetComponent<Rigidbody>().AddForce(-direction* pullforce * Time.deltaTime);
        }
        if (connect2.Movable && connect2.GetComponent<Rigidbody>() != null)
        {
            Debug.Log("Test Pull");
                connect2.GetComponent<Rigidbody>().AddForce(direction * pullforce * Time.deltaTime);
            
        }
        else
        {
            if(connect2.tag == "Player")
            {
                PlayerController player = connect2.GetComponent<PlayerController>();
                if (player.GetGroundObject() != null) 
                {
                    if(player.GetGroundObject().GetComponent<Rigidbody>() != null)
                    {
                        player.GetGroundObject().GetComponent<Rigidbody>().AddForce(direction * pullforce * Time.deltaTime);
                    }
                        
                }
            }

        }
    }

    public void OnDestroy()
    {
        Debug.Log("Rope starting to destory");
        connect1.RopeObjectUnset();
        connect2.RopeObjectUnset();
        connect1 = null;
        connect2 = null;
        DestoryCallback();
    }

}
