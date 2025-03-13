using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieStatManager : MonoBehaviour
{
    ZombieManager zombie;

    [Header("Damage Modifiers")]
    public float headShotDamageMultiplier = 1.5f;

    [Header("Overall Health")]
    public int overallHealth = 100; // If this health reaches 0, the zombie dies

    [Header("Head Health")]
    public int headHealth = 100; // If this health reaches below a certain %, the head will have a chance to explode, causing instant death

    [Header("Upperbody Health")]
    public int torsoHealth = 100; // Aside from the head, this is the best place to hit to lower overall health
    public int leftArmHealth = 100; // Does not detract from overall health, however, has a chance to destroy the limb after reaching a certain %
    public int rightArmHealth = 100; // Does not detract from overall health, however, has a chance to destroy the limb after reaching a certain %

    [Header("Lower Body Health")]
    public int leftLegHealth = 100; // Does not detract from overall health, however, has a chance to destroy the limb after reaching a certain %
    public int rightLegHealth = 100; // Does not detract from overall health, however, has a chance to destroy the limb after reaching a certain %

    private void Awake()
    {
        zombie = GetComponent<ZombieManager>();
    }

    public void DealHeadShotDamage(int damage)
    {
        headHealth -= Mathf.RoundToInt(damage * headShotDamageMultiplier);
        overallHealth -= Mathf.RoundToInt(damage * headShotDamageMultiplier);
        CheckForDeath();
    }

    public void DealTorsoDamage(int damage)
    {
        torsoHealth -= damage;
        overallHealth -= damage;
        CheckForDeath();
    }

    public void DealArmDamage(bool leftArmDamage, int damage)
    {
        // Arm damage does not subtract from actual zombie health
        if (leftArmDamage)
        {
            leftArmHealth -= damage;
        }
        else
        {
            rightArmHealth -= damage;
        }

        CheckForDeath();
    }

    public void DealLegDamage(bool leftLegDamage, int damage)
    {
        // Leg damage does not subtract from actual zombie health
        if (leftLegDamage)
        {
            leftLegHealth -= damage;
        }
        else
        {
            rightLegHealth -= damage;
        }

        CheckForDeath();
    }

    private void CheckForDeath()
    {
        if (overallHealth <= 0)
        {
            overallHealth = 0;
            if (!zombie.isDead)
            {
                zombie.Die(); // Llamamos a la función de muerte de ZombieManager
            }
        }
    }
}


