using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieManager : MonoBehaviour
{
    public IdleState startingState;

    [Header("Flags")]
    public bool isPerformingAction;

    [Header("Current State")]
    [SerializeField] private State currentState;

    [Header("Current Target")]
    public PlayerManager currentTarget;
    public float distanceFromCurrentTarget;

    [Header("Animator")]
    public Animator animator;

    [Header("Navmesh Agent")]
    public NavMeshAgent zombieNavmeshAgent;

    [Header("Rigidbody")]
    public Rigidbody zombieRigidbody;

    [Header("Locomotion")]
    public float rotationSpeed = 5;

    [Header("Attack")]
    public float minimumAttacKDistance = 1;

    private void Awake()
    {
       currentState = startingState;
       animator = GetComponent<Animator>();
       zombieNavmeshAgent = GetComponentInChildren<NavMeshAgent>();
       zombieRigidbody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        HandleStateMachine();
    }

    private void Update()
    {
        zombieNavmeshAgent.transform.localPosition = Vector3.zero;


        if (currentTarget != null)
        {
            distanceFromCurrentTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
        }
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

