using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEditor.ShaderGraph;
using System.Globalization;
using Unity.VisualScripting;

public class PlayerController : BaseControllable
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
    private EquipSkillBase[] equipSkills = new EquipSkillBase[2];
    private Dictionary<EquipSkillBase, bool> isUsingSkills=new Dictionary<EquipSkillBase,bool>();

    void Awake()
    {
        controls = new InputSystem.PlayerInput();
        controller = GetComponent<CharacterController>();

        //DEBUG: Test add skill rope
        equipSkills[0]=new Skill_RopeTest();
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
        controls.Player.UseLeftSkill.started += ctx => OnBeginUseSkill(0);
        //controls.Player.UseLeftSkill.performed += ctx => OnUseSkill(0);
        controls.Player.UseLeftSkill.canceled += ctx => OnEndUseSkill(0);

        controls.Player.UseRightSkill.started += ctx => OnBeginUseSkill(1);
        controls.Player.UseRightSkill.performed += ctx => OnUseSkill(1);
        controls.Player.UseRightSkill.canceled += ctx => OnEndUseSkill(1);
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

        foreach (var item in isUsingSkills)
        {
            Debug.Log($"{item.Key} is {item.Value}");
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
    void OnBeginUseSkill(int index)
    {
        if (index > 2)
        {
            Debug.Log("Skill index probably too big?");
            
        }
        isUsingSkills[equipSkills[index]] = true;
        equipSkills[index].OnBeginUse();
    }
    void OnUseSkill(int index)
    {
        Debug.Log($"Skill {index} is being used!");
        equipSkills[index].OnUse();


    }
    void OnEndUseSkill(int index)
    {
        isUsingSkills[equipSkills[index]] = false;
        equipSkills[index].OnEndUse();
    }

}
