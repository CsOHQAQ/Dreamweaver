using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Pipeline;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private List<PreservableObject> preservedObjects = new();

    public void AddPreservableObject(PreservableObject preservable)
    {
        if (preservable == null) return;
        preservedObjects.Add(preservable);
    }

    /// <summary>
    /// Place preservable object in the inventory into the scene
    /// </summary>
    /// <param name="index">object index in the array, TODO: we can change to using object ID later</param>
    /// <param name="position">The position that we need to place the object</param>
    public void PlacePreservableObject(int index, Vector3 position,Transform parent=null)
    {
        if (index < 0 || index >= preservedObjects.Count) return;

        PreservableObject obj = preservedObjects[index];
        preservedObjects.RemoveAt(index);

        obj.transform.position = position;
        if (parent != null)
        {
            obj.transform.parent = parent;

        }
        obj.gameObject.SetActive(true);
    }
    
    public bool HasPreservableObject() => preservedObjects.Count > 0;

}
