using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance { get; private set; }
    //
    [Header("MovementStats")]
    [Range(2f, 32f)] public float moveSpeed = 16f;
    [Range(0f, 35f)] public float rotSpeed = .6f;
    [Range(4f, 64f)] public float runSpeed = 32f;
    [Range(1f, 100f)] public float jumpPower = 1f;
    [Range(1f, 100f)] public float rollPower = 1f;
    [Range(0f, 1f)] public float rollDuration = .5f;

    [Header("ScriptsReference")]
    public PlayerMovement playerMovement;
    public PlayerLocomotion playerLocomotion;

    [Header("Components")]
    public Animator animator;

    [Header("DebugInput")]
    [SerializeField] private Vector2 movementInput;
    [SerializeField] private Vector2 velocityInput;
    [SerializeField] private Vector2 cameraMovement;

    [Header("InputMetadata")]
    public Vector2 normalizedInput;
    public Vector2 normalizedCameraMovement;
    public float movementWeight;
    public bool isSprint;
    public bool isAiming;
    public bool isFiring;
    public UnityEvent jump;
    public UnityEvent roll;

    private void Awake()
    {
        if (!ReferenceEquals(Instance, null) && !ReferenceEquals(Instance, this))
        {
            Destroy(Instance);
            Debug.Log("[From PlayerInputManager] There is another Singleton created from this Singleton. Failsafe already deleted it.");
        }

        Instance = this;
    }

    private void Jump()
    {

    }

    private void OnEnable()
    {
        if (ReferenceEquals(this.playerMovement, null))
        {
            this.playerMovement = new PlayerMovement();
            this.playerMovement.Movement.PlayerMovement.performed += i => this.movementInput = i.ReadValue<Vector2>();
            this.playerMovement.Movement.CameraMovement.performed += i => this.cameraMovement = i.ReadValue<Vector2>();
            this.playerMovement.Movement.Sprint.performed += i => this.isSprint = i.ReadValue<float>() > .5f;
            this.playerMovement.Movement.Jump.performed += context => this.jump.Invoke();
            this.playerMovement.Movement.Roll.performed += context => this.roll.Invoke();
            this.playerMovement.Movement.Aim.performed += i => this.isAiming = i.ReadValue<float>() > .5f;
            this.playerMovement.Movement.Fire.performed += i => this.isFiring = i.ReadValue<float>() > .5f && this.isAiming;

            Cursor.lockState = CursorLockMode.Locked;
        }

        this.playerMovement.Enable();
    }

    private void OnDisable()
    {
        if (ReferenceEquals(this.playerMovement, null))
        {
            this.playerMovement.Disable();
        }
    }

    private void OnHandleInput()
    {
        this.normalizedInput = this.movementInput;
        this.normalizedCameraMovement = this.cameraMovement.normalized;

        this.movementWeight = Mathf.Clamp01(Mathf.Abs(this.normalizedInput.y) + Mathf.Abs(this.normalizedInput.x));
    }

    private void Update()
    {
        this.OnHandleInput();
    }

    private void FixedUpdate()
    {

    }
}