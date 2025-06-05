using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugOutline : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
        }
    }
}
