using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseControllable
{

    public float acclerate;
    public float MaxSpeed;

    bool isMoving;
    Vector2 movingDirection = Vector2.zero;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!isControlled) return;

        UpdateMovingState();

        if (movingDirection != Vector2.zero)
        {
            Debug.Log("Moving!");
            OnMove();
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
