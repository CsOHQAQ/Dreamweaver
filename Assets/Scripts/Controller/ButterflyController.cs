using UnityEngine;

public class ButterflyController : DreamBodyController
{
    public float acensionSpeed = 5f; 
    private Vector2 omniMoveInput;
    protected override void OnEnable()
    {
        controls.Enable();
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.OmniMove.performed += ctx => omniMoveInput = ctx.ReadValue<Vector2>();
        controls.Player.OmniMove.canceled += ctx => omniMoveInput = Vector2.zero;
    }

    /// <summary>
    /// Override the dreambody update, only checks Omni movement and ignore gravity
    /// </summary>
    protected override void Update()
    {
        HandleMovement();
        HandleOmniMovement();
        ApplyMovement();
    }

    protected override void HandleMovement()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        move = Camera.main.transform.TransformDirection(move);
        move.y = 0;

        velocity.x = move.x * moveSpeed;
        velocity.z = move.z * moveSpeed;

        if (move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void HandleOmniMovement()
    {
        Vector3 move = new(0, omniMoveInput.y, 0);
        move = Camera.main.transform.TransformDirection(move);
        move.x = move.z = 0;

        velocity.y = move.y * acensionSpeed;
        if (move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Triggerable"))
            OnFinishMission();
    }

    public override void OnFinishMission()
    {
        base.OnFinishMission();
        FinishDreambody();
    }
}
