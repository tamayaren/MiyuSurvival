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
    [Range(4f, 32f)] public float moveSpeed = 16f;
    [Range(0f, 5f)] public float rotSpeed = .6f;
    [Range(16f, 64f)] public float runSpeed = 32f;

    [Header("ScriptsReference")]
    public PlayerInput playerMovement;
    public PlayerLocomotion playerLocomotion;

    [Header("Components")]
    public Rigidbody rigidbody;
    public Animator animator;

    private Vector2 movementInput;
    private Vector2 velocityInput;

    [SerializeField] public Vector2 normalizedInput;
    [SerializeField] public float movementWeight;

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
            this.playerMovement = new PlayerInput();
            this.playerMovement.PlayerMovement.Keyboard.performed += i => this.movementInput = i.ReadValue<Vector2>();
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