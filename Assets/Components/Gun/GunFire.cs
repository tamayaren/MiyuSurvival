using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFire : MonoBehaviour
{
    private CinemachineImpulseSource impulseSource;

    [SerializeField] private Transform muzzle;

    [SerializeField] private ParticleSystem muzzleFire;
    private void Start()
    {
        muzzleFire = muzzle.GetComponent<ParticleSystem>();
        impulseSource = muzzle.GetComponent<CinemachineImpulseSource>();
    }

    public void FireGun()
    {
        impulseSource.GenerateImpulse(Camera.main.transform.forward);
        muzzleFire.Emit(10);
    }
}
