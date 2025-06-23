using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerController : BaseControllable
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float jumpForce = 8f;
    public float gravity = -9.81f;

    private Vector3 velocity;
    private bool isGrounded;
    private GameObject groundObj;

    private Vector2 moveInput;
    private bool jumpInput;
    private EquipSkillBase[] equipSkills = new EquipSkillBase[2];
    private Dictionary<EquipSkillBase, bool> isUsingSkills = new Dictionary<EquipSkillBase, bool>();

    void Awake()
    {
        controls = new InputSystem.PlayerInput();
        controller = GetComponent<CharacterController>();
        lookAt = transform.Find("LookAtPoint");

        //DEBUG: Test add skill rope
        equipSkills[0] = new Skill_RopeTest();
        equipSkills[1] = new Skill_Scissor();
        isUsingSkills.Add(equipSkills[0], false);
        isUsingSkills.Add(equipSkills[1], false);
        equipSkills[0].OnEquip(this); equipSkills[1].OnEquip(this);
    }

    void OnEnable()
    {
        controls.Enable();
        SubscribeControls();
    }

    void OnDisable()
    {
        UnsubscribeControls();
        controls.Disable();
    }

    protected override void Update()
    {
        CheckIsGrounded();
        ApplyGravity();

        HandleMovement();
        HandleJump();

        foreach (var item in isUsingSkills)
        {
            if (item.Value)
            {
                item.Key.OnUse();
            }
        }

        ApplyMovement();
    }

    //Handle movement input
    void HandleMovement()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        move = Camera.main.transform.TransformDirection(move);
        move.y = 0;

        velocity.x = move.x * moveSpeed;
        velocity.z = move.z * moveSpeed;

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
        groundObj = isGrounded ? hit.collider.gameObject : null;
        if (groundObj != null)
        {
            if (groundObj.GetComponent<Rigidbody>() != null)
            {
                controller.SimpleMove(groundObj.GetComponent<Rigidbody>().velocity);
            }
        }
    }

    public GameObject GetGroundObject()
    {
        return groundObj;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody hitRb = hit.collider.attachedRigidbody;

    }



    #region Input Callback Naming & Sub/Unsubbing
    private void OnMovePerformed(InputAction.CallbackContext ctx) => moveInput = ctx.ReadValue<Vector2>();

    private void OnMoveCanceled(InputAction.CallbackContext ctx) => moveInput = Vector2.zero;

    private void OnJumpPerformed(InputAction.CallbackContext ctx) => jumpInput = true;

    private void OnLeftSkillPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log($"ctx interaction is {ctx.interaction}");
        if (ctx.interaction is HoldInteraction)
        {
            Debug.Log("Left holded");

            isUsingSkills[equipSkills[0]] = true;
            equipSkills[0].OnUse();
        }
        else if (ctx.interaction is TapInteraction)
        {
            Debug.Log("Left taped");
            RaycastHit hit;
            if (MouseClickDetector.GetClickObject(equipSkills[0].DetectLayer, out hit))
                equipSkills[0].OnBeginUse(hit);
        }
    }

    private void OnLeftSkillCanceled(InputAction.CallbackContext ctx)
    {
        if (ctx.interaction is HoldInteraction) isUsingSkills[equipSkills[0]] = false;
    }

    private void OnRightSkillPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log($"ctx interaction is {ctx.interaction}");
        if (ctx.interaction is HoldInteraction)
        {
            Debug.Log("Right holded");

            isUsingSkills[equipSkills[1]] = true;
            equipSkills[1].OnUse();
        }
        else if (ctx.interaction is TapInteraction)
        {
            Debug.Log("Right taped");
            RaycastHit hit;
            if (MouseClickDetector.GetClickObject(equipSkills[1].DetectLayer, out hit))
                equipSkills[1].OnBeginUse(hit);
        }
    }

    private void OnRightSkillCanceled(InputAction.CallbackContext ctx)
    {
        if (ctx.interaction is HoldInteraction)
        {
            isUsingSkills[equipSkills[1]] = false;
        }
    }
    
    private void SubscribeControls()
    {
        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCanceled;
        controls.Player.Jump.performed += OnJumpPerformed;  // for debug use only

        controls.Player.UseLeftSkill.performed += OnLeftSkillPerformed;
        controls.Player.UseLeftSkill.canceled += OnLeftSkillCanceled;

        controls.Player.UseRightSkill.performed += OnRightSkillPerformed;
        controls.Player.UseRightSkill.canceled += OnRightSkillCanceled;
    }

    private void UnsubscribeControls()
    {
        controls.Player.Move.performed -= OnMovePerformed;
        controls.Player.Move.canceled -= OnMoveCanceled;
        controls.Player.Jump.performed -= OnJumpPerformed;  // for debug use only

        controls.Player.UseLeftSkill.performed -= OnLeftSkillPerformed;
        controls.Player.UseLeftSkill.canceled -= OnLeftSkillCanceled;

        controls.Player.UseRightSkill.performed -= OnRightSkillPerformed;
        controls.Player.UseRightSkill.canceled -= OnRightSkillCanceled;
    }

    #endregion



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
