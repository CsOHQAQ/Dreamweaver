using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MouseClickDetector
{
    public static bool GetClickObject(int LayerMask,out RaycastHit result)
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hit;
        hit=Physics.RaycastAll(ray,10000f,LayerMask);
        float distance = 19260817f;

        result=new RaycastHit();
        if(hit.Length<1)
            return false;

        foreach (var item in hit)
        {
            if (item.distance<distance)
            {
                result=item;
                distance = item.distance;
            }
        }

        return true;
    }

}
