using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EventTrigger : MonoBehaviour
{
    //或许该用事件广播?
    public List<TriggerEventBase> triggerEvent=new List<TriggerEventBase>();
    public LayerMask DetectLayer;
    public GameObject DetectObject;

    public bool IsOneTimeTrigger;
    private bool isTriggered = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == DetectLayer||other.gameObject==DetectObject)
        {
            if (!IsOneTimeTrigger || isTriggered == false)
            {
                isTriggered = true;
                foreach (TriggerEventBase triggerEvent in triggerEvent)
                {
                    triggerEvent.OnTriggered(this);
                }
            }
        }
    }
}
