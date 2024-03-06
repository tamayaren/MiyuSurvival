using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance { get; private set; }
    //
    [Header("MovementStats")]
    [Range(2f, 32f)] public float moveSpeed = 16f;
    [Range(0f, 35f)] public float rotSpeed = .6f;
    [Range(4f, 64f)] public float runSpeed = 32f;

    [Header("ScriptsReference")]
    public PlayerMovement playerMovement;
    public PlayerLocomotion playerLocomotion;

    [Header("Components")]
    public Animator animator;


    private Vector2 movementInput;
    private Vector2 velocityInput;
    private Vector2 cameraMovement;

    [Header("InputMetadata")]
    public Vector2 normalizedInput;
    public Vector2 normalizedCameraMovement;
    public float movementWeight;
    public bool isSprint;

    private void Awake()
    {
        if (!ReferenceEquals(Instance, null) && !ReferenceEquals(Instance, this))
        {
            Destroy(Instance);
            Debug.Log("[From PlayerInputManager] There is another Singleton created from this Singleton. Failsafe already deleted it.");
        }

        Instance = this;
    }

    private void OnEnable()
    {
        if (ReferenceEquals(this.playerMovement, null))
        {
            this.playerMovement = new PlayerMovement();
            this.playerMovement.Movement.PlayerMovement.performed += i => this.movementInput = i.ReadValue<Vector2>();
            this.playerMovement.Movement.CameraMovement.performed += i => this.cameraMovement = i.ReadValue<Vector2>();
            this.playerMovement.Movement.Sprint.performed += i => this.isSprint = i.ReadValue<float>() > .5f;
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
        this.normalizedCameraMovement = this.cameraMovement;

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