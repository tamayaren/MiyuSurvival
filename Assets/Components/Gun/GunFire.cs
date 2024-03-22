using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFire : MonoBehaviour
{
    private CinemachineImpulseSource impulseSource;
    [SerializeField] private GameObject player;

    [SerializeField] private Transform muzzle;
    [SerializeField] private ParticleSystem muzzleFire;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject head;

    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip clip;

    private void Start()
    {
        muzzleFire = muzzle.GetComponent<ParticleSystem>();
        impulseSource = muzzle.GetComponent<CinemachineImpulseSource>();

        audio = GetComponent<AudioSource>();
    }

    public void FireGun()
    {
        impulseSource.GenerateImpulse(Camera.main.transform.forward);
        muzzleFire.Emit(10);

        audio.pitch = Random.Range(.8f, 1.2f);
        audio.Play();
        //
        GameObject bullet = Instantiate(this.bullet);
        bullet.transform.position = muzzleFire.transform.position;
        bullet.transform.rotation = head.transform.rotation * Quaternion.Euler(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        BulletHandler handler = bullet.GetComponent<BulletHandler>();
        handler.Init(player, 2048f, 6f);
    }
}
