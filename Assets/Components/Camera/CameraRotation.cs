using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    private PlayerInputManager playerInputManager;
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineOrbitalTransposer transposer;

    [SerializeField] [Range(3f, 6f)] private float distanceMax = 3f;
    [SerializeField] private float currentDistance = 0f;

    private void Start() {
        playerInputManager = PlayerInputManager.Instance;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        transposer = virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    private void Update()
    {
        currentDistance = Mathf.Clamp(currentDistance + (((playerInputManager.normalizedCameraMovement.y) * Time.fixedDeltaTime) * (distanceMax*2)), -distanceMax, distanceMax);

        transposer.m_FollowOffset.y = Mathf.Lerp(transposer.m_FollowOffset.y, currentDistance, 5 * Time.deltaTime);
    }
}
