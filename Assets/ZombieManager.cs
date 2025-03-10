using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    public IdleState startingState;

    [Header("Current State")]
    [SerializeField] private State currentState;

    [Header("Current Target")]
    public PlayerManager currentTarget;
    private void Awake()
    {
       currentState = startingState;
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

