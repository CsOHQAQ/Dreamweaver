using System;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class DreamBodyController : BaseControllable
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float jumpForce = 8f;
    public float gravity = -9.81f;
    protected Vector3 velocity;
    private bool isGrounded;
    protected Vector2 moveInput;
    private bool jumpInput;

    [SerializeField]
    private Vector3 origin;
    public event Action MissionAccomplished;    // Use as a local event, there will only some mechanics listen to this event
    public event Action OnBeforeDisable;        // similar as above, trigger before disable to ensure mechanics to unsub this object.

    void Awake()
    {
        controls = new InputSystem.PlayerInput();
        controller = GetComponent<CharacterController>();
        gameObject.layer = LayerMask.NameToLayer("Dream Body");
        lookAt = transform.Find("LookAtPoint");
        Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Passable Wall"));
    }

    protected override void Start()
    {
        controls.Disable();
        
    }

    protected virtual void OnEnable()
    {
        controls.Enable();
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Jump.performed += ctx => jumpInput = true;
        controls.Player.UseLeftSkill.performed += ctx =>
        {
            Debug.Log($"ctx interaction is {ctx.interaction}");
            if (ctx.interaction is MultiTapInteraction)
            {
                Debug.Log("Dreambody Left multiTaped");
                ReturnToOrigin();
            }
        };
        controls.Player.UseRightSkill.performed += ctx =>
        {
            if (ctx.interaction is TapInteraction)
            {
                Debug.Log("DreamBody Right taped");
                OnFinishMission();   // testing only
            }
        };
    }

    void OnDisable()
    {
        controls.Disable();
        OnBeforeDisable?.Invoke();
    }

    protected override void Update()
    {
        CheckIsGrounded();
        ApplyGravity();

        HandleMovement();
        HandleJump();

        ApplyMovement();
    }

    //Handle movement input
    protected virtual void HandleMovement()
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
    protected virtual void HandleJump()
    {
        if (isGrounded && jumpInput)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            jumpInput = false;
        }
    }

    //Apply gravity to the player
    protected virtual void ApplyGravity()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
    }

    //Apply movement to the player
    protected virtual void ApplyMovement()
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

    void ReturnToOrigin()
    {
        Debug.Log($"Current Location:{transform.position}");
        controller.enabled = false;
        transform.position = origin;
        transform.rotation = Quaternion.identity;
        controller.enabled = true;
        Debug.Log($"after: Current Location:{transform.position}");
    }

    public virtual void OnFinishMission()
    {
        EventManager.Instance.TriggerSwitchControl(ControllableManager.Instance.GetPlayerControllable());
        EventManager.Instance.TriggerMissionFinish();
        MissionAccomplished?.Invoke();
    }

    public void FinishDreambody()
    {
        gameObject.SetActive(false);
    }

}
