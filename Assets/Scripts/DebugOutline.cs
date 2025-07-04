using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugOutline : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        if (TryGetComponent<BoxCollider>(out var col))
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
        }
    }
}
