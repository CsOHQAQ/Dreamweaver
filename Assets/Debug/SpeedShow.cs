using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedShow : MonoBehaviour
{
    Rigidbody rb;
    Rigidbody2D rb2d;
    TMP_Text text;

    [SerializeField]
    private Vector3 speed
    {
        get
        {
            if (rb != null)
            {
                return rb.velocity;
            }
            else
            {
                if (rb2d != null)
                    return rb2d.velocity;
            }
            return Vector3.zero;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        text=GetComponentInChildren<TMP_Text>();
        if (transform.parent != null)
        {
            rb = transform.parent.GetComponent<Rigidbody>();
            rb2d = transform.parent.GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        text.text = speed.ToString();
    }
}
