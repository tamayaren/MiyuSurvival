using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform target;

    [SerializeField] private Animator animator;
    [SerializeField] private Entity entity;

    [SerializeField] private GameObject character;
    [SerializeField] private Renderer renderer;
    [SerializeField] private CharacterController controller;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private Scores score;

    private IEnumerator DeadLain()
    {
        controller.detectCollisions = false;
        entity.enabled = false;
        foreach (Material material in renderer.materials)
        {
            material.DOFade(1f, 1.5f).SetEase(Ease.OutQuad);
        }

        yield return new WaitForSeconds(1.5f);
        character.SetActive(false);
        yield return new WaitForSeconds(3f);
        Destroy(this);
    }

    private IEnumerator Dead()
    {
        yield return new WaitForSeconds(60f);
        entity.Damage(9999f);
    }

    private void Start()
    {
        score = FindObjectOfType<Scores>();
        entity = GetComponent<Entity>();
        entity.OnDied.AddListener(() =>
        {
            agent.isStopped = true;
            animator.SetBool("Dead", true);
            StartCoroutine(DeadLain());
            score.SetScore(1000f);
        });

        entity.OnDamage.AddListener(() =>
        {
            audioSource.Play();
            score.SetScore(250f);
        });

        controller = GetComponent<CharacterController>();
        agent.speed = Random.Range(10f, 24f);
        this.gameObject.transform.localScale *= Random.Range(.8f, 1.2f);
    }

    private void Awake()
    {
        this.agent = GetComponent<NavMeshAgent>();
        this.target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        agent.SetDestination(this.target.position);
        animator.SetFloat("Movement", agent.velocity.normalized.magnitude);
    }
}
