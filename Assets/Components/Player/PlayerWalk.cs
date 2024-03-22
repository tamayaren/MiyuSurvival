using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : MonoBehaviour
{
    [SerializeField] private AudioSource source;

    [SerializeField] private AudioClip walkSound;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void Walk()
    {
        source.volume = .1f;
        source.pitch = Random.Range(.8f, 1.2f);
        source.PlayOneShot(walkSound);
    }
}
