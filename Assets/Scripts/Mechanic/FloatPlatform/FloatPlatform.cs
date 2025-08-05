using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class FloatPlatform : MonoBehaviour
{
    public bool IsLocked;
    public float CD;
    public bool AutoReturn;
    float curCDCount;
    public float MoveSpeed;
    public Transform Platform;
    public List<Transform> RouteTransforms;
    public bool IsPulling = false;
    public bool DEBUG_ShowRoute;
    [Header("Debug")]
    [SerializeField]
    protected int currentStation = -1;

    // Start is called before the first frame update
    void Start()
    {
        if (Platform != null && RouteTransforms.Count > 0)
        {
            Platform.position = RouteTransforms[0].position;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (DEBUG_ShowRoute)
        {
            for (int i=0;i<RouteTransforms.Count-1;i++)
            {
                if (i == currentStation)
                {
                    Debug.DrawLine(RouteTransforms[i].position, RouteTransforms[i+1].position,Color.green);
                }
                else
                {
                    Debug.DrawLine(RouteTransforms[i].position, RouteTransforms[i + 1].position, Color.red);
                }
            }
        }

        if (!IsLocked)
        {
            if (!IsPulling)//Return to startPoint;
            {
                if (AutoReturn)
                {
                    if (curCDCount > CD)
                    {
                        StepBackward();
                    }
                    curCDCount += Time.deltaTime;
                }
            }
            else
            {
                curCDCount = 0;
                StepForward();
            }
        }
        if(!AutoReturn) 
            IsPulling = false;
    }

    protected virtual void StepForward()
    {
        if(RouteTransforms.Count==0)
            return;

        if(currentStation==-1)
            currentStation = 0;

        if (currentStation >= RouteTransforms.Count-1)
        {
            currentStation = RouteTransforms.Count-1;
            Platform.position = RouteTransforms[currentStation].position;
            return;
        }
        else
        {
            if (Vector3.Distance(Platform.position, RouteTransforms[currentStation + 1].position) < 0.05f)
            {
                currentStation++;
                Platform.position= RouteTransforms[currentStation].position;
            }
            else
            {
                Platform.position = Vector3.MoveTowards(Platform.position, RouteTransforms[currentStation + 1].position, MoveSpeed * Time.deltaTime);
            }
        }

    }
    protected virtual void StepBackward() 
    {
        if (RouteTransforms.Count == 0)
            return;

        if (currentStation == -1)
        {
            Platform.position = RouteTransforms[0].position;
            return;
        }
        else
        {
            if (Vector3.Distance(Platform.position, RouteTransforms[currentStation].position) < 0.05f)
            {
                Platform.position = RouteTransforms[currentStation].position;
                currentStation--;
            }
            else
            {
                Platform.position = Vector3.MoveTowards(Platform.position, RouteTransforms[currentStation].position, MoveSpeed * Time.deltaTime);
            }
        }
    }


}
