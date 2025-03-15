using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float healthPercentage = 0.2f; // Recupera un 20% de la vida
    private bool isPlayerNearby = false;
    private PlayerManager playerManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que el jugador tiene el tag "Player"
        {
            isPlayerNearby = true;
            playerManager = other.GetComponent<PlayerManager>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            playerManager = null;
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
        if (playerManager != null)
        {
            // Aquí se agregará la lógica para restaurar la vida cuando el sistema esté implementado
            Debug.Log("Vida aumentada en " + (healthPercentage * 100) + "%");

            Destroy(gameObject); // Elimina el objeto tras recogerlo
        }
    }
}


