using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieStatManager : MonoBehaviour
{
    ZombieManager zombie;

    [Header("Damage Modifiers")]
    public float headShotDamageMultiplier = 2.5f;  // Headshots más letales
    public float torsoDamageMultiplier = 1.0f;  // Daño normal
    public float armDamageMultiplier = 0.5f;  // Menos impacto en brazos
    public float legDamageMultiplier = 0.7f;  // Menos impacto en piernas

    [Header("Overall Health")]
    public int overallHealth = 100;

    [Header("Head Health")]
    public int headHealth = 100;

    [Header("Upper Body Health")]
    public int torsoHealth = 100;
    public int leftArmHealth = 100;
    public int rightArmHealth = 100;

    [Header("Lower Body Health")]
    public int leftLegHealth = 100;
    public int rightLegHealth = 100;

    private void Awake()
    {
        zombie = GetComponent<ZombieManager>();
    }

    public void DealHeadShotDamage(int damage)
    {
        int finalDamage = Mathf.RoundToInt(damage * headShotDamageMultiplier);
        headHealth -= finalDamage;
        overallHealth -= finalDamage; // Aplica daño directo a la salud general
        CheckForDeath();
    }

    public void DealTorsoDamage(int damage)
    {
        int finalDamage = Mathf.RoundToInt(damage * torsoDamageMultiplier);
        torsoHealth -= finalDamage;
        overallHealth -= finalDamage;
        CheckForDeath();
    }

    public void DealArmDamage(bool leftArmDamage, int damage)
    {
        int finalDamage = Mathf.RoundToInt(damage * armDamageMultiplier);

        if (leftArmDamage)
            leftArmHealth -= finalDamage;
        else
            rightArmHealth -= finalDamage;

        overallHealth -= finalDamage; // Aplica daño directamente a la salud general

        CheckForDeath();
    }

    public void DealLegDamage(bool leftLegDamage, int damage)
    {
        int finalDamage = Mathf.RoundToInt(damage * legDamageMultiplier);

        if (leftLegDamage)
            leftLegHealth -= finalDamage;
        else
            rightLegHealth -= finalDamage;

        overallHealth -= finalDamage; // Aplica daño directamente a la salud general

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



