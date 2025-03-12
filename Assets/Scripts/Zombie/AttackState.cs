using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public override State Tick(ZombieManager zombieManager)
    {
        if (zombieManager.isPerformingAction)
        {
            zombieManager.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
            return this;
        }

        zombieManager.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);

        return this;
    }
}

