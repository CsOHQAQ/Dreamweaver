using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamBodyController : BaseControllable
{
    public float Acclerate;
    public float MaxSpeed;
    Vector2 movingDirection = Vector2.zero;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!isControlled) {
            Debug.Log("I'm not being controlled." + gameObject.name);
            return;
        }

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
            Debug.Log("I can't Jump, you Idiot.");
        }
    }

    void OnMove()
    {
        Vector3 targetVel = new Vector3(movingDirection.x, 0, movingDirection.y) * MaxSpeed;
        rb.velocity = Vector3.MoveTowards(rb.velocity, targetVel, Acclerate * Time.deltaTime);
    }
}
