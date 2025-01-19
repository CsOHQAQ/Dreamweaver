using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float acclerate;
    public float MaxSpeed;

    bool isMoving;
    Rigidbody rb;
    Vector2 movingDirection = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovingState();

        if (movingDirection != Vector2.zero)
        {
            Debug.Log("Moving!");
            OnMove();
        }
        else
        {

        }

    }

    void UpdateMovingState()
    {
        movingDirection = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            movingDirection.y = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movingDirection.y = -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movingDirection.x = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movingDirection.x = 1;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJump();
        }


    }

    void OnMove()
    {
        Vector3 targetVel = new Vector3(movingDirection.x, 0, movingDirection.y) * MaxSpeed;
        rb.velocity = Vector3.MoveTowards(rb.velocity, targetVel, acclerate * Time.deltaTime);
    }
    
     void OnJump()
     {
        rb.AddForce(new Vector3(0,300, 0));
     }


}
