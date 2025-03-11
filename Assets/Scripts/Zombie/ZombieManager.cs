using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieManager : MonoBehaviour
{
    public IdleState startingState;

    [Header("Current State")]
    [SerializeField] private State currentState;

    [Header("Current Target")]
    public PlayerManager currentTarget;

    [Header("Animator")]
    public Animator animator;

    [Header("Navmesh Agent")]
    NavMeshAgent zombieNavmesAgent;

    private void Awake()
    {
       currentState = startingState;
        animator = GetComponent<Animator>();
       zombieNavmesAgent = GetComponentInChildren<NavMeshAgent>();
    }
    private void FixedUpdate()
    {
        HandleStateMachine();
    }
    private void HandleStateMachine()
    {
        State nextState;

        if (currentState != null)
        {
            nextState = currentState.Tick(this);
            if (nextState != null)
            {
                currentState = nextState;
            }
        }
    }
}

