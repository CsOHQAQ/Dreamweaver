using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This is to control AirWall activating when certain object Enter
/// </summary>
public class AirWallTrigger_ObjectEnter : MonoBehaviour
{
    public GameObject DetectObject;//Maybe list later on?
    public List<Collider> ActivateOnEnter;
    public List<Collider> DeactivateOnEnter;
    public List<Collider> ActivateOnLeave;
    public List<Collider> DeactivateOnLeave;

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
        if (other.transform.gameObject== DetectObject)
        {
            foreach (Collider c in ActivateOnEnter)
            {
                c.gameObject.SetActive(true);
            }
            foreach(Collider c in DeactivateOnEnter)
            {
                c.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject == DetectObject)
        {
            foreach (Collider c in DeactivateOnLeave)
            {
                c.gameObject.SetActive(false);
            }
            foreach (Collider c in ActivateOnLeave)
            {
                c.gameObject.SetActive(true);
            }
        }
    }
}
