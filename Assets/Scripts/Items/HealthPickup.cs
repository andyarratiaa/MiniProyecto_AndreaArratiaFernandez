using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float healthPercentage = 0.2f; // Recupera un 20% de la vida
    private bool isPlayerNearby = false;
    private PlayerManager playerManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Aseg�rate de que el jugador tiene el tag "Player"
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
            // Aqu� se agregar� la l�gica para restaurar la vida cuando el sistema est� implementado
            Debug.Log("Vida aumentada en " + (healthPercentage * 100) + "%");

            Destroy(gameObject); // Elimina el objeto tras recogerlo
        }
    }
}


