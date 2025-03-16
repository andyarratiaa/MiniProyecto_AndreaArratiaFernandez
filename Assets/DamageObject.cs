using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour
{
    public float damagePercentage = 2f; // 10% de la vida máxima del jugador
    public ParticleSystem hitEffect; // Efecto visual (VFX) que se activará
    public Transform vfxSpawnPoint; // Punto donde se activará el efecto visual

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el jugador ha tocado el objeto
        PlayerHealthManager playerHealth = other.GetComponent<PlayerHealthManager>();
        if (playerHealth != null)
        {
            // Calcula el daño basado en el porcentaje
            float damageAmount = playerHealth.maxHealth * (damagePercentage / 100f);
            playerHealth.TakeDamage(damageAmount);

            // Activa el efecto visual si está asignado
            if (hitEffect != null)
            {
                PlayVFX();
            }
        }
    }

    void PlayVFX()
    {
        if (hitEffect != null)
        {
            // Si el VFX está dentro del objeto, lo instanciamos en una nueva posición
            ParticleSystem vfxInstance = Instantiate(hitEffect, vfxSpawnPoint ? vfxSpawnPoint.position : transform.position, Quaternion.identity);
            vfxInstance.Play();

            // Destruir la instancia después de su duración
            Destroy(vfxInstance.gameObject, vfxInstance.main.duration);
        }
    }
}

