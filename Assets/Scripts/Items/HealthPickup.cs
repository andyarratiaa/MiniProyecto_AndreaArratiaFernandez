using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float healthPercentage = 10f; // Recupera un 20% de la vida
    private bool isPlayerNearby = false;
    private PlayerHealthManager playerHealthManager;

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
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.Return)) // Espera a que el jugador presione Enter
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

            Debug.Log("Vida aumentada en " + healthToRestore + " puntos.");

            Destroy(gameObject); // Eliminar el objeto después de recogerlo
        }
    }
}


