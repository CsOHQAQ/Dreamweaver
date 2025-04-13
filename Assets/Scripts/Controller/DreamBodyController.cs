using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UIElements;

public class DreamBodyController : BaseControllable
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float jumpForce = 8f;
    public float gravity = -9.81f;
    private Vector3 velocity;
    private bool isGrounded;
    private Vector2 moveInput;
    private bool jumpInput;

    [SerializeField]    
    private Vector3 origin;

    void Awake()
    {
        controls = new InputSystem.PlayerInput();
        controller = GetComponent<CharacterController>();
        gameObject.layer = LayerMask.NameToLayer("Dream Body");

        //DEBUG: Test add skill rope
        Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Passable Wall"));
    }

    void OnEnable()
    {
        controls.Enable();
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Jump.performed += ctx => jumpInput = true;
        controls.Player.UseLeftSkill.performed += ctx =>
        {
            Debug.Log($"ctx interaction is {ctx.interaction}");
            if(ctx.interaction is MultiTapInteraction)
            {
                Debug.Log("Dreambody Left multiTaped");
                ReturnToOrigin();
            }
        };
        controls.Player.UseRightSkill.performed += ctx =>
        {
            if(ctx.interaction is TapInteraction)
            {
                Debug.Log("Right taped");
                EventManager.TriggerSwitchControl(GameManager.Instance.PrevControllable);
            }
        };
        
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

    void ReturnToOrigin() {
        Debug.Log($"Current Location:{transform.position}");
        controller.enabled = false;
        transform.position = origin;
        transform.rotation = Quaternion.identity;
        controller.enabled = true;
        Debug.Log($"after: Current Location:{transform.position}");
    }

}
