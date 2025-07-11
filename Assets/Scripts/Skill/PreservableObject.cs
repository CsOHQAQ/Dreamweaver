using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreservableObject : MonoBehaviour
{
    [SerializeField] string id;
    [SerializeField] private Transform originalPos;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = originalPos.position;
    }

    public void OnPreserved()
    {
        gameObject.SetActive(false);
    }
}
