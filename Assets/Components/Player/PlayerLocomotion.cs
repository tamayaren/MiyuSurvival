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

    private Vector2 movementDelta = new Vector2(0, 0);

    private void Start()
    {
        this.characterController = GetComponent<CharacterController>();
        this.playerInputManager = PlayerInputManager.Instance;
    }

    private void FixedUpdate()
    {
        this.OnMovementHandle();
        this.OnRotationHandle();

        animator.SetFloat("X", this.movementDelta.x);
        animator.SetFloat("Y", this.movementDelta.y);
    }

    private void OnMovementHandle()
    {
        this.moveDirection = new Vector3(CineCamera.transform.forward.x, 0, CineCamera.transform.forward.z) * PlayerInputManager.Instance.normalizedInput.y;
        this.moveDirection = this.moveDirection + (new Vector3(CineCamera.transform.right.x, 0, CineCamera.transform.right.z) * PlayerInputManager.Instance.normalizedInput.x);

        this.movementDelta = new Vector2(Mathf.Lerp(animator.GetFloat("X"), (PlayerInputManager.Instance.normalizedInput.x), 5 * Time.deltaTime), Mathf.Lerp(animator.GetFloat("Y"), Mathf.Abs(PlayerInputManager.Instance.normalizedInput.y), 5 * Time.deltaTime));
        this.moveDirection.Normalize();

        if (PlayerInputManager.Instance.movementWeight > 0f)
        {
            float Speed = ((PlayerInputManager.Instance.movementWeight >= 3f) ? PlayerInputManager.Instance.runSpeed : PlayerInputManager.Instance.moveSpeed);

            this.moveDirection = this.moveDirection * ((PlayerInputManager.Instance.movementWeight >= 3f) ? PlayerInputManager.Instance.runSpeed : PlayerInputManager.Instance.moveSpeed);
        }

        Vector3 velocity = new Vector3(this.moveDirection.x, 0, this.moveDirection.z);
        velocity.y = characterController.isGrounded ? Physics.gravity.y : 0;

        if (playerInputManager.isSprint) {
            this.movementDelta *= 3;

            velocity = velocity * playerInputManager.runSpeed;
        } else { velocity = velocity * playerInputManager.moveSpeed; }

        characterController.Move(velocity * Time.deltaTime);
    }

    private void OnRotationHandle()
    {
        this.targetDirection = CineCamera.transform.forward * PlayerInputManager.Instance.normalizedInput.y;
        this.targetDirection = this.moveDirection + (CineCamera.transform.right * PlayerInputManager.Instance.normalizedInput.x);

        this.targetDirection.Normalize();

        if (this.targetDirection.magnitude == 0f) return;
        Quaternion targetRotation = Quaternion.LookRotation(this.targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, PlayerInputManager.Instance.rotSpeed * Time.fixedDeltaTime);

        transform.rotation = playerRotation;
    }
}