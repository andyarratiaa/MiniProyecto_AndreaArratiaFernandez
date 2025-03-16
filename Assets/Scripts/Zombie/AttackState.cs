using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackState : State
{
    PursueTargetState pursueTargetState;

    [Header("Zombie Attacks")]
    public ZombieAttackAction[] zombieAttackActions;

    [Header("Potential Attacks Performable Right Now")]
    public List<ZombieAttackAction> potentialAttacks;

    [Header("Current Attack Being Performed")]
    public ZombieAttackAction currentAttack;

    [Header("State Flags")]
    public bool hasPerformedAttack;

    private void Awake()
    {
        pursueTargetState = GetComponent<PursueTargetState>();
    }

    public override State Tick(ZombieManager zombieManager)
    {
        zombieManager.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);

        // If the zombie is being hurt, or is in some action, pause the state
        if (zombieManager.isPerformingAction)
        {
            zombieManager.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
            return this;
        }

        if (!hasPerformedAttack && zombieManager.attackCoolDownTimer <= 0)
        {
            if (currentAttack == null)
            {
                GetNewAttack(zombieManager);
            }
            else
            {
                AttackTarget(zombieManager);
            }
        }

        if (hasPerformedAttack)
        {
           ResetStateFlags();
           return pursueTargetState;
        }
        else
        {
           return this;
            
        }
    }

    private void GetNewAttack(ZombieManager zombieManager)
    {
        for (int i = 0; i < zombieAttackActions.Length; i++)
        {
            ZombieAttackAction zombieAttack = zombieAttackActions[i];

            // Check for attack distances needed to perform the potential attack
            if (zombieManager.distanceFromCurrentTarget <= zombieAttack.maximumAttackDistance &&
                zombieManager.distanceFromCurrentTarget >= zombieAttack.minimumAttackDistance)
            {
                // Check for attack angles needed to perform the potential attack
                if (zombieManager.viewableAngleFromCurrentTarget <= zombieAttack.maximumAttackAngle &&
                    zombieManager.viewableAngleFromCurrentTarget >= zombieAttack.minimumAttackAngle)
                {
                    // If the attack passes the distance and angle check, add it to the list of attacks we MAY perform right now
                    potentialAttacks.Add(zombieAttack);
                }
            }
        }

        int randomValue = Random.Range(0, potentialAttacks.Count);

        if (potentialAttacks.Count > 0)
        {
            currentAttack = potentialAttacks[randomValue];
            potentialAttacks.Clear();
        }
    }

    private void AttackTarget(ZombieManager zombieManager)
    {
        if (currentAttack != null)
        {
            hasPerformedAttack = true;
            zombieManager.attackCoolDownTimer = currentAttack.attackCooldown;
            zombieManager.zombieAnimatorManager.PlayTargetAttackAnimation(currentAttack.attackAnimation);

            if (zombieManager.currentTarget != null)
            {
                PlayerHealthManager playerHealth = zombieManager.currentTarget.GetComponent<PlayerHealthManager>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(15f); // Reduce un 15% de la vida total
                }
            }
        }
        else
        {
            Debug.LogWarning("WARNING: Zombie is attempting to perform an attack, but has no attack");
        }
    }

    private void ResetStateFlags()
    {
        hasPerformedAttack = false;
    }

}


