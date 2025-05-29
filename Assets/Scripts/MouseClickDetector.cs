using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class MouseClickDetector
{


    public static bool GetClickObject(int LayerMask,out RaycastHit result)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit debug_hit;
        Physics.Raycast(ray, out debug_hit);
        foreach (var obj in GameObject.FindObjectsOfType<RopeObject>())
        {
            bool flag=obj.GetComponentInChildren<CapsuleCollider>().Raycast(ray,out debug_hit,10000f);
        }

        if (Physics.Raycast(ray,out result, 100000f, LayerMask,QueryTriggerInteraction.Collide))
        {
            Debug.Log($"{result.transform.name} is detected!");
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 1000f, Color.green, 10f);
            Debug.DrawLine(result.point, result.point +Vector3.up* 10f, Color.yellow, 10f);
            return true;
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 1000f, Color.red, 10f);
            return false;
        }


        #region HitAllVersion
        /*
        RaycastHit[] hit,_debugHit;
        hit=Physics.RaycastAll(ray,10000f,LayerMask);
        _debugHit= Physics.RaycastAll(ray, 10000f);
        float distance = 19260817f;
        result =new RaycastHit();
        if (hit.Length < 1)
        {            
            Debug.DrawLine(ray.origin,ray.origin+ray.direction*1000f,Color.red,10f);
            return false;
        }
            

        foreach (var item in hit)
        {
            if (item.distance<distance)
            {
                result=item;
                distance = item.distance;
            }
        }
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 1000f, Color.green, 10f);
        */
        #endregion
    }

}
