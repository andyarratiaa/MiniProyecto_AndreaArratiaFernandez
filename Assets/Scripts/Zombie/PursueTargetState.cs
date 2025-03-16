using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueTargetState : State
{
    AttackState attackState;

    private void Awake()
    {
        attackState = GetComponent<AttackState>();
    }
    public override State Tick(ZombieManager zombieManager)
    {
        if (zombieManager.isPerformingAction)
        {
            zombieManager.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
            return this;
        }

        //Debug.Log("RUNNING the pursue target state");

        MoveTowardsCurrentTarget(zombieManager);
        RotateTowardsTarget(zombieManager);

        if (zombieManager.distanceFromCurrentTarget <= zombieManager.maximumAttackDistance)
        {
            zombieManager.zombieNavmeshAgent.enabled = false;
            return attackState;
        }
        else
        {
            return this;
        }
    }

    private void MoveTowardsCurrentTarget(ZombieManager zombieManager)
    {
        zombieManager.animator.SetFloat("Vertical", 1, 0.2f, Time.deltaTime);

        //zombieManager.PlayWalkSound();
    }

    private void RotateTowardsTarget(ZombieManager zombieManager)
    {
        zombieManager.zombieNavmeshAgent.enabled = true;
        zombieManager.zombieNavmeshAgent.SetDestination(zombieManager.currentTarget.transform.position);
        zombieManager.transform.rotation = Quaternion.Slerp(zombieManager.transform.rotation, 
                                                            zombieManager.zombieNavmeshAgent.transform.rotation, 
                                                            zombieManager.rotationSpeed / Time.deltaTime);
    }
}

