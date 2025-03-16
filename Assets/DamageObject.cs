using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour
{
    public float damagePercentage = 2f; // 2% de la vida máxima del jugador
    public ParticleSystem hitEffect; // Efecto visual (VFX) que se activará
    public Transform vfxSpawnPoint; // Punto donde se activará el efecto visual
    public AudioClip explosionSound; // 🔊 Sonido de explosión

    private AudioSource audioSource;

    private void Start()
    {
        // Si no hay un AudioSource en el objeto, lo añadimos
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configuración del AudioSource
        audioSource.volume = 1.0f;
        audioSource.spatialBlend = 0f; // 🔹 Sonido 2D para que siempre se escuche igual
        audioSource.playOnAwake = false; // 🔹 Evita que el sonido se reproduzca al inicio
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el jugador ha tocado el objeto
        PlayerHealthManager playerHealth = other.GetComponent<PlayerHealthManager>();
        if (playerHealth != null)
        {
            // Calcula el daño basado en el porcentaje
            float damageAmount = playerHealth.maxHealth * (damagePercentage / 100f);
            playerHealth.TakeDamage(damageAmount);

            Debug.Log("💥 ¡El jugador ha recibido daño!");

            // 🔊 Reproducir sonido de explosión
            if (explosionSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(explosionSound);
            }

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
            // Instanciamos el VFX en la posición deseada
            ParticleSystem vfxInstance = Instantiate(hitEffect, vfxSpawnPoint ? vfxSpawnPoint.position : transform.position, Quaternion.identity);
            vfxInstance.Play();

            // Destruir la instancia después de su duración
            Destroy(vfxInstance.gameObject, vfxInstance.main.duration);
        }
    }
}


