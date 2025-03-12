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

    public void DamageZombieHead()
    {
        // We ALWAYS stagger for a headshot
        zombie.isPerformingAction = true;
        zombie.animator.CrossFade("Damage Head", 0.2f);
    }

    public void DamageZombieTorso()
    {
        zombie.isPerformingAction = true;
        zombie.animator.CrossFade("Damage Torso", 0.2f);
    }

    public void DamageZombieRightArm()
    {
        zombie.isPerformingAction = true;
        zombie.animator.CrossFade("Damage Torso", 0.2f);
    }

    public void DamageZombieLeftArm()
    {
        zombie.isPerformingAction = true;
        zombie.animator.CrossFade("Damage Torso", 0.2f);
    }

    public void DamageZombieRightLeg()
    {
        zombie.isPerformingAction = true;
        zombie.animator.CrossFade("Damage Torso", 0.2f);
    }

    public void DamageZombieLeftLeg()
    {
        zombie.isPerformingAction = true;
        zombie.animator.CrossFade("Damage Torso", 0.2f);
    }

}
