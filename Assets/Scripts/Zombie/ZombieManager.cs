using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieManager : MonoBehaviour
{
    public ZombieAnimatorManager zombieAnimatorManager;

    public IdleState startingState;

    [Header("Flags")]
    public bool isPerformingAction;

    [Header("Current State")]
    [SerializeField] private State currentState;

    [Header("Current Target")]
    public PlayerManager currentTarget;
    public Vector3 targetsDirection;
    public float distanceFromCurrentTarget;
    public float viewableAngleFromCurrentTarget;

    [Header("Animator")]
    public Animator animator;

    [Header("Navmesh Agent")]
    public NavMeshAgent zombieNavmeshAgent;

    [Header("Rigidbody")]
    public Rigidbody zombieRigidbody;

    [Header("Locomotion")]
    public float rotationSpeed = 5;

    [Header("Attack")]
    public float attackCoolDownTimer;
    public float minimumAttacKDistance = 1;
    public float maximumAttackDistance = 3.5f;

    private void Awake()
    {
       currentState = startingState;
       animator = GetComponent<Animator>();
       zombieNavmeshAgent = GetComponentInChildren<NavMeshAgent>();
       zombieRigidbody = GetComponent<Rigidbody>();
       zombieAnimatorManager = GetComponent<ZombieAnimatorManager>();
    }
    private void FixedUpdate()
    {
        HandleStateMachine();
    }

    private void Update()
    {
        zombieNavmeshAgent.transform.localPosition = Vector3.zero;

        if(attackCoolDownTimer > 0 )
        {
            attackCoolDownTimer = attackCoolDownTimer - Time.deltaTime;
        }


        if (currentTarget != null)
        {
            targetsDirection = currentTarget.transform.position - transform.position;
            viewableAngleFromCurrentTarget = Vector3.SignedAngle(targetsDirection, transform.forward, Vector3.up);
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

