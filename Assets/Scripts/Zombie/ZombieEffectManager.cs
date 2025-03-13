using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieEffectManager : MonoBehaviour
{
    ZombieManager zombie;
    private void Awake()
    {
        zombie = GetComponent<ZombieManager>();
    }

    public void DamageZombieHead(int damage)
    {
        // We ALWAYS stagger for a headshot
        zombie.isPerformingAction = true;
        zombie.animator.CrossFade("Damage Head", 0.2f);
        zombie.zombieStatManager.DealHeadShotDamage(damage);
    }

    public void DamageZombieTorso(int damage)
    {
        zombie.isPerformingAction = true;
        zombie.animator.CrossFade("Damage Torso", 0.2f);
        zombie.zombieStatManager.DealTorsoDamage(damage);
    }

    public void DamageZombieRightArm(int damage)
    {
        zombie.isPerformingAction = true;
        zombie.animator.CrossFade("Damage Torso", 0.2f);
        zombie.zombieStatManager.DealArmDamage(false, damage);
    }

    public void DamageZombieLeftArm(int damage)
    {
        zombie.isPerformingAction = true;
        zombie.animator.CrossFade("Damage Torso", 0.2f);
        zombie.zombieStatManager.DealArmDamage(true, damage);
    }

    public void DamageZombieRightLeg(int damage)
    {
        zombie.isPerformingAction = true;
        zombie.animator.CrossFade("Damage Torso", 0.2f);
        zombie.zombieStatManager.DealLegDamage(false, damage);  
    }

    public void DamageZombieLeftLeg(int damage)
    {
        zombie.isPerformingAction = true;
        zombie.animator.CrossFade("Damage Torso", 0.2f);
        zombie.zombieStatManager.DealLegDamage(true, damage);
    }

}
