using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class TestP : PlayerController
{
    private Vector3 velocity;
    private bool isGrounded;
    private GameObject groundObj;
    private InputSystem.PlayerInput controls;
    private Vector2 moveInput;
    private bool jumpInput;
    private EquipSkillBase[] equipSkills = new EquipSkillBase[2];
    private Dictionary<EquipSkillBase, bool> isUsingSkills = new Dictionary<EquipSkillBase, bool>();

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new InputSystem.PlayerInput();

        //DEBUG: Test add skill rope
        equipSkills[0] = new Skill_RopeTest();
        equipSkills[1] = new Skill_RopeTest();
        isUsingSkills.Add(equipSkills[0], false);
        isUsingSkills.Add(equipSkills[1], false);
        equipSkills[0].OnEquip(this); equipSkills[1].OnEquip(this);
    }

    void OnEnable()
    {
        controls.Enable();
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Jump.performed += ctx => jumpInput = true;

        controls.Player.UseLeftSkill.performed += ctx =>
        {
            if (ctx.interaction is MultiTapInteraction)
            {
                equipSkills[0].OnCanceled();
            }
            else if (ctx.interaction is HoldInteraction)
            {

                isUsingSkills[equipSkills[0]] = true;
                equipSkills[0].OnUse();
            }
            else if (ctx.interaction is TapInteraction)
            {
                equipSkills[0].OnBeginUse();
            }
        };
        controls.Player.UseLeftSkill.canceled += ctx =>
        {
            if (ctx.interaction is HoldInteraction)
            {
                isUsingSkills[equipSkills[0]] = false;
            }
        };

        controls.Player.UseRightSkill.performed += ctx =>
        {
            Debug.Log($"ctx interaction is {ctx.interaction}");
            if (ctx.interaction is MultiTapInteraction)
            {
                Debug.Log("Left multiTaped");
                equipSkills[1].OnCanceled();
            }
            else if (ctx.interaction is HoldInteraction)
            {
                Debug.Log("Left holded");

                isUsingSkills[equipSkills[1]] = true;
                equipSkills[1].OnUse();
            }
            else if (ctx.interaction is TapInteraction)
            {
                Debug.Log("Left taped");
                equipSkills[1].OnBeginUse();
            }
        };
        controls.Player.UseRightSkill.canceled += ctx =>
        {
            if (ctx.interaction is HoldInteraction)
            {
                isUsingSkills[equipSkills[1]] = false;
            }
        };
    }

    void OnDisable()
    {
        controls.Disable();
    }

    protected override void Update()
    {
        if (isControlled)
        {
            HandleMovement();
            HandleJump();
        }

        foreach (var item in isUsingSkills)
        {
            //Debug.Log($"{item.Key} is {item.Value}");
            if (item.Value)
            {
                item.Key.OnUse();
            }
        }
    }

    //Handle movement input
    void HandleMovement()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        move = Camera.main.transform.TransformDirection(move);
        move.y = 0;

        if (moveInput!=Vector2.zero)
        {
            rb.velocity = new Vector3(move.x * moveSpeed, rb.velocity.y,move.z* moveSpeed);
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


    /*
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
    */

    public GameObject GetGroundObject()
    {
        return groundObj;
    }
}
