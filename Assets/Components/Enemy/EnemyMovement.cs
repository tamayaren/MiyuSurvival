using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform target;


    private void Awake()
    {
        this.agent = GetComponent<NavMeshAgent>();
        this.target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        agent.SetDestination(this.target.position);
    }
}
