using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "A.I/Actions/Zombie Attack Action")]
public class ZombieAttackAction : ScriptableObject
{
    [Header("Attack Animation")]
    public string attackAnimation;

    [Header("Attack Cooldown")]
    public float attackCooldown = 5f; // The time before the zombie can perform another attack (After performing this attack)

    [Header("Attack Angles & Distances")]
    public float maximumAttackAngle = 30f;  // The maximum angle of sight needed to perform THIS attack
    public float minimumAttackAngle = -30f; // The minimum angle of sight needed to perform THIS attack
    public float minimumAttackDistance = 1f; // The minimum distance from the current target needed to perform THIS attack
    public float maximumAttackDistance = 3.5f; // The maximum distance from the current target needed to perform THIS attack
}

