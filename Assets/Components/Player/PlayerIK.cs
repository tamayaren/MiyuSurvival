using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIK : MonoBehaviour
{
    [SerializeField] protected Animator animator;

    [Header("Main")]
    public bool IKActive = true;
    public bool IKLookCamera = true;

    public bool IKRightGrab = true;
    public bool IKLeftGrab = true;
    [SerializeField] private float lookWeight = 0f;
    [SerializeField] private float targetLookWeight = 0f;

    [Header("Bones")]
    [SerializeField] private Transform Root;
    [SerializeField] private Transform Head;

    [Header("Arms Grab")]
    [SerializeField] public Transform leftTarget;
    [SerializeField] public Transform rightTarget;
    [SerializeField] public Transform eyeTarget;

    private void OnAnimatorIK(int layerIndex)
    {
        if (!this.animator) return;
        if (!this.IKActive) return;

        this.CameraLookAt();
        this.LeftGrabHandle();
        this.RightGrabHandle();
    }

    private void LeftGrabHandle()
    {
        if (this.leftTarget == null) return;
        if (IKActive && IKLeftGrab)
        {
            if (this.leftTarget)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);

                animator.SetIKPosition(AvatarIKGoal.LeftHand, this.leftTarget.position);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, this.leftTarget.rotation);
                return;
            }
        }
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
    }

    private void RightGrabHandle()
    {
        if (this.rightTarget == null) return;
        if (IKActive && IKRightGrab)
        {
            if (this.rightTarget)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);

                animator.SetIKPosition(AvatarIKGoal.RightHand, this.rightTarget.position);
                animator.SetIKRotation(AvatarIKGoal.RightHand, this.rightTarget.rotation);
                return;
            }
        }
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
    }

    private void CameraLookAt()
    {
        if (this.IKLookCamera && animator.GetCurrentAnimatorStateInfo(0).IsName("Main"))
        {
            if (!this.eyeTarget)
            {
                Vector3 headForward = Head.transform.TransformDirection(Vector3.forward);
                Vector3 targetDirection = Head.transform.position - (Camera.main.transform.position + (Camera.main.transform.forward * 128f));

                float dot = Vector3.Dot(headForward, targetDirection);

                targetLookWeight = .6f;
                animator.SetLookAtPosition(Camera.main.transform.position + (Camera.main.transform.forward * 128f));
            }
            else
            {
                animator.SetLookAtPosition(this.eyeTarget.position);
                targetLookWeight = 1f;
            }
        }
        else
            targetLookWeight = 0f;

        lookWeight = Mathf.Lerp(lookWeight, targetLookWeight, 6 * Time.deltaTime);
        animator.SetLookAtWeight(lookWeight, lookWeight * .85f);
    }
}
