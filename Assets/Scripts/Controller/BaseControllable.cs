using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseControllable : MonoBehaviour
{
    [SerializeField]
    protected bool isControlled = false;    // A bool to indicate whether the playing is controlling or not
    public Rigidbody rb;                 // Rigidbody, assuming every class has a rb

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    public virtual void SetControl(bool controlled) {
        isControlled = controlled;
    }
}
