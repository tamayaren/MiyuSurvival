using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerInputManager playerInputManager;

    [SerializeField] private GameObject CineCamera;
    [SerializeField] private Animator animator;

    [Header("MovementStats")]
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private Vector3 targetDirection;

    [SerializeField] private Vector2 movementDelta = new Vector2(0, 0);
    [SerializeField] private float movementSpeed = 0;

    [SerializeField] private Vector3 velocity;
    [SerializeField] private float rollVelocity;

    private void Start()
    {
        this.characterController = GetComponent<CharacterController>();
        this.playerInputManager = PlayerInputManager.Instance;
    }

    public void Jump()
    {
        if (!characterController.isGrounded) return;

        velocity.y = 0;
        velocity.y = velocity.y + playerInputManager.jumpPower;
        animator.SetTrigger("Jump");
    }

    private IEnumerator RollPhysics()
    {
        float startTime = Time.time;

        rollVelocity = this.playerInputManager.rollPower;
        while (rollVelocity > 0f)
        {
            rollVelocity -= this.playerInputManager.rollDuration - Time.deltaTime;
            yield return null;
        }
        rollVelocity = 0f;
    }

    public void Roll()
    {
        if (!characterController.isGrounded) return;
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (!state.IsName("Main")) return;

        StartCoroutine(RollPhysics());
        animator.SetTrigger("Roll");
    }

    private void FixedUpdate()
    {
        this.OnMovementHandle();
        this.OnRotationHandle();

        animator.SetFloat("X", this.movementDelta.x);
        animator.SetFloat("Y", this.movementDelta.y);
        animator.SetFloat("Movement", this.movementSpeed);
    }

    private void OnMovementHandle()
    {
        this.moveDirection = new Vector3(CineCamera.transform.forward.x, 0, CineCamera.transform.forward.z) * PlayerInputManager.Instance.normalizedInput.y;
        this.moveDirection = this.moveDirection + (new Vector3(CineCamera.transform.right.x, 0, CineCamera.transform.right.z) * PlayerInputManager.Instance.normalizedInput.x);

        float deltaMultiplier = 1f;

        float speedTheta = 0f;
        float theta = 5f;

        this.moveDirection.Normalize();

        if (PlayerInputManager.Instance.movementWeight > 0f)
        {
            float Speed = ((PlayerInputManager.Instance.movementWeight >= 3f) ? PlayerInputManager.Instance.runSpeed : PlayerInputManager.Instance.moveSpeed);

            this.moveDirection = this.moveDirection * ((PlayerInputManager.Instance.movementWeight >= 3f) ? PlayerInputManager.Instance.runSpeed : PlayerInputManager.Instance.moveSpeed);
        }

        Vector3 movementVelocity = new(this.moveDirection.x, 0, this.moveDirection.z);

        if (this.movementDelta.magnitude > .5f)
        {
            deltaMultiplier = playerInputManager.isSprint ? 3f : 1f;
            speedTheta = playerInputManager.isSprint ? 3f : 1f;
            movementVelocity = movementVelocity * (playerInputManager.isSprint ? playerInputManager.runSpeed : playerInputManager.moveSpeed);

            theta = playerInputManager.isSprint ? 15f : 25f;
        }
        else
        {
            speedTheta = 0f;
        }

        this.movementDelta = new Vector2(Mathf.Lerp(animator.GetFloat("X"), (PlayerInputManager.Instance.normalizedInput.x * deltaMultiplier), 5 * Time.deltaTime), 
            Mathf.Lerp(animator.GetFloat("Y"), Mathf.Abs(PlayerInputManager.Instance.normalizedInput.y * deltaMultiplier), 5 * Time.deltaTime));

        if (rollVelocity > 0f)
        {
            movementVelocity = transform.forward.normalized * rollVelocity;
        }
            
        this.movementSpeed = Mathf.Lerp(animator.GetFloat("Movement"), speedTheta, theta * Time.deltaTime);
        velocity.y += (!this.characterController.isGrounded) ? Physics.gravity.y * Time.fixedDeltaTime : 0;
        characterController.Move((movementVelocity + new Vector3(0, velocity.y, 0)) * Time.fixedDeltaTime);
        //
    }

    private void OnRotationHandle()
    {
        this.targetDirection = CineCamera.transform.forward * PlayerInputManager.Instance.normalizedInput.y;
        this.targetDirection = this.moveDirection + (CineCamera.transform.right * PlayerInputManager.Instance.normalizedInput.x);

        this.targetDirection.Normalize();

        if (!playerInputManager.isAiming) if (this.targetDirection.magnitude == 0f) return;
        if (playerInputManager.isAiming) this.targetDirection = new Vector3(CineCamera.transform.forward.x, 0, CineCamera.transform.forward.z) * 64f;
        Quaternion targetRotation = Quaternion.LookRotation(this.targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, PlayerInputManager.Instance.rotSpeed * Time.fixedDeltaTime);

        transform.rotation = playerRotation;
    }
}