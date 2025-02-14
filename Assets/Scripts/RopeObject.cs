using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeObject : MonoBehaviour
{
    public float Speed;
    public float Length;

    public LineRenderer line;
    public AttachableObject connect1, connect2;
    public bool isMoving;
    public bool isPulling;
    public float PullForce=5000;

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

        line.SetPosition(0, Vector3.Lerp(line.GetPosition(0),connect1.transform.position,Speed*Time.deltaTime));
        line.SetPosition(1, Vector3.Lerp(line.GetPosition(1), connect2.transform.position, Speed* Time.deltaTime));

        if (isMoving)
        {
            if (Vector3.Distance(line.GetPosition(0),connect1.transform.position)<0.05f&& Vector3.Distance(line.GetPosition(1), connect2.transform.position) < 0.05f)
            {
                isMoving = false;
            }
        }
        if (isPulling)
        {
            Pull();
        }
    }

    public void DelayBreak(float time)
    {
        StartCoroutine(DelayDie(time));
    }

    IEnumerator DelayDie(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
        yield return null;
    }

    public void Pull()
    {
        Vector3 direction = (connect1.transform.position - connect2.transform.position).normalized;
        if (connect1.Movable&& connect1.GetComponent<Rigidbody>()!=null)
        {            
            connect1.GetComponent<Rigidbody>().AddForce(-direction*PullForce*Time.deltaTime);
        }
        if (connect2.Movable && connect2.GetComponent<Rigidbody>() != null)
        {
            if(!(connect2.gameObject.layer==LayerMask.NameToLayer("Player")))
            connect2.GetComponent<Rigidbody>().AddForce(direction * PullForce * Time.deltaTime);
        }
    }
}
