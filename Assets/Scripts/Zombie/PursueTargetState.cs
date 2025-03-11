using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueTargetState : State
{
    public override State Tick(ZombieManager zombieManager)
    {
        Debug.Log("RUNNING the pursue target state");
        MoveTowardsCurrentTarget(zombieManager);
        return this;
    }

    private void MoveTowardsCurrentTarget(ZombieManager zombieManager)
    {
        zombieManager.animator.SetFloat("Vertical", 1, 0.2f, Time.deltaTime);
    }
}

