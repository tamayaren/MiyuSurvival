using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAiming : MonoBehaviour
{
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private PlayerIK ik;

    [SerializeField] private Transform aimTarget;
    [SerializeField] private Transform gun;
    private Animator gunAnimator;

    private void Start()
    {
        playerInputManager = PlayerInputManager.Instance;
        gunAnimator = gun.GetComponent<Animator>();
    }

    private void Update()
    {
        gunAnimator.SetFloat("Movement", Mathf.Round(playerAnimator.GetFloat("Movement")));
        gunAnimator.SetBool("Aiming", playerInputManager.isAiming);

        gunAnimator.SetBool("Grounded", characterController.isGrounded);
        gunAnimator.SetBool("Fire", playerInputManager.isFiring);

        ik.IKLeftGrab = !playerInputManager.isSprint || playerInputManager.isAiming;
    }
}
