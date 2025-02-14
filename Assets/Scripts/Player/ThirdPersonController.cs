using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ThirdPersonController : BaseControllable
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float jumpForce = 8f;
    public float gravity = -9.81f;
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private InputSystem.PlayerInput controls;
    private Vector2 moveInput;
    private bool jumpInput;

    void Awake()
    {
        controls = new InputSystem.PlayerInput();
        controller = GetComponent<CharacterController>();
    }

    void OnEnable()
    {
        controls.Enable();
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Jump.performed += ctx => jumpInput = true;
    }

    void OnDisable()
    {
        controls.Disable();
    }

    protected override void Update()
    {
        CheckIsGrounded();
        ApplyGravity();

        if (isControlled)
        {
            HandleMovement();
            HandleJump();
        }

        ApplyMovement();
    }

    //Handle movement input
    void HandleMovement()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        move = Camera.main.transform.TransformDirection(move);
        move.y = 0;

        if (isGrounded)
        {
            velocity.x = move.x * moveSpeed;
            velocity.z = move.z * moveSpeed;
        }

        if (isGrounded && move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    //Handle jump input
    void HandleJump()
    {
        if (isGrounded && jumpInput)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            jumpInput = false;
        }
    }

    //Apply gravity to the player
    void ApplyGravity()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
    }

    //Apply movement to the player
    void ApplyMovement()
    {
        controller.Move(velocity * Time.deltaTime);
    }

    //Check if the player is grounded
    void CheckIsGrounded()
    {
        float sphereRadius = 0.4f;
        float groundCheckDistance = 0.2f;
        LayerMask groundLayer = LayerMask.GetMask("Ground");

        float playerHeight = controller.height;
        Vector3 sphereOrigin = transform.position + Vector3.down * (playerHeight / 2 - sphereRadius);

        isGrounded = Physics.SphereCast(sphereOrigin, sphereRadius, Vector3.down, out RaycastHit hit, groundCheckDistance, groundLayer);
    }

    // void OnDrawGizmos()
    // {
    //     if (!Application.isPlaying) return;
    //     float sphereRadius = 0.4f;
    //     float groundCheckDistance = 0.2f;

    //     float playerHeight = controller.height;
    //     Vector3 sphereOrigin = transform.position + Vector3.down * (playerHeight / 2 - sphereRadius);

    //     Gizmos.color = Color.white;
    //     Gizmos.DrawWireSphere(sphereOrigin, sphereRadius);

    //     Vector3 castEnd = sphereOrigin + Vector3.down * groundCheckDistance;
    //     Gizmos.color = isGrounded ? Color.green : Color.red;
    //     Gizmos.DrawWireSphere(castEnd, sphereRadius);
    // }


}
