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

        // Si el zombi está en medio de una animación o herido, no realiza ataques
        if (zombieManager.isPerformingAction)
        {
            zombieManager.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
            return this;
        }

        // Si el jugador no está lo suficientemente cerca, regresa a perseguirlo
        if (zombieManager.distanceFromCurrentTarget > zombieManager.minimumAttacKDistance)
        {
            return pursueTargetState;
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

            // Verificar que el jugador está dentro del rango de ataque
            if (zombieManager.distanceFromCurrentTarget <= zombieAttack.maximumAttackDistance &&
                zombieManager.distanceFromCurrentTarget >= zombieAttack.minimumAttackDistance)
            {
                // Verificar el ángulo de visión para permitir el ataque
                if (zombieManager.viewableAngleFromCurrentTarget <= zombieAttack.maximumAttackAngle &&
                    zombieManager.viewableAngleFromCurrentTarget >= zombieAttack.minimumAttackAngle)
                {
                    potentialAttacks.Add(zombieAttack);
                }
            }
        }

        if (potentialAttacks.Count > 0)
        {
            int randomValue = Random.Range(0, potentialAttacks.Count);
            currentAttack = potentialAttacks[randomValue];
            potentialAttacks.Clear();
        }
    }

    private void AttackTarget(ZombieManager zombieManager)
    {
        // ⚠️ Verificar nuevamente la distancia antes de atacar
        if (zombieManager.distanceFromCurrentTarget > zombieManager.minimumAttacKDistance)
        {
            return; // Si está demasiado lejos, no ataca
        }

        if (currentAttack != null)
        {
            hasPerformedAttack = true;
            zombieManager.attackCoolDownTimer = currentAttack.attackCooldown;
            zombieManager.zombieAnimatorManager.PlayTargetAttackAnimation(currentAttack.attackAnimation);

            // 🔊 Reproducir sonido de ataque
            zombieManager.PlayAttackSound();

            if (zombieManager.currentTarget != null)
            {
                PlayerHealthManager playerHealth = zombieManager.currentTarget.GetComponent<PlayerHealthManager>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(15f);
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



