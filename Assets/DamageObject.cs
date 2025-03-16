using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour
{
    public float damagePercentage = 10f; // 10% de la vida m�xima del jugador
    public ParticleSystem hitEffect; // Efecto visual (VFX) que se activar�

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el jugador ha tocado el objeto
        PlayerHealthManager playerHealth = other.GetComponent<PlayerHealthManager>();
        if (playerHealth != null)
        {
            // Calcula el da�o basado en el porcentaje
            float damageAmount = playerHealth.maxHealth * (damagePercentage / 100f);
            playerHealth.TakeDamage(damageAmount);

            // Activa el efecto visual si est� asignado
            if (hitEffect != null)
            {
                hitEffect.Play();
            }
        }
    }
}
