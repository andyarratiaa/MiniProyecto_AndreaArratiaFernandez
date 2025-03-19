using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float healthPercentage = 10f; // Recupera un porcentaje de la vida
    public AudioClip pickupSound; // 🔊 Sonido al recoger la salud

    private bool isPlayerNearby = false;
    private PlayerHealthManager playerHealthManager;
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
        audioSource.spatialBlend = 0f; // 🔹 Asegura que el sonido se escuche igual en todas partes
        audioSource.playOnAwake = false; // 🔹 Evita que el sonido se reproduzca al inicio
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que el jugador tiene el tag "Player"
        {
            isPlayerNearby = true;
            playerHealthManager = other.GetComponent<PlayerHealthManager>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            playerHealthManager = null;
        }
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E)) // Espera a que el jugador presione Enter
        {
            PickupHealth();
        }
    }

    private void PickupHealth()
    {
        if (playerHealthManager != null)
        {
            float healthToRestore = playerHealthManager.maxHealth * (healthPercentage / 100f);
            playerHealthManager.currentHealth += healthToRestore;

            // Asegurar que la salud no exceda el máximo permitido
            if (playerHealthManager.currentHealth > playerHealthManager.maxHealth)
            {
                playerHealthManager.currentHealth = playerHealthManager.maxHealth;
            }

            playerHealthManager.UpdateHealthBar(); // Actualizar la UI de la barra de vida

            Debug.Log("✅ Vida aumentada en " + healthToRestore + " puntos.");

            // 🔊 Reproducir sonido de recogida de salud antes de destruir el objeto
            if (pickupSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(pickupSound);
                GetComponent<Collider>().enabled = false; // 🔹 Desactivar colisión para evitar recolecciones múltiples
                Destroy(gameObject, pickupSound.length); // 🔹 Se destruye tras reproducir el sonido
            }
            else
            {
                Destroy(gameObject); // Si no hay sonido, destruye inmediatamente
            }
        }
    }
}




