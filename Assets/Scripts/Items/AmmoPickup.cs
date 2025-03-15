using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 15; // Cantidad de balas que otorga
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
            PickupAmmo();
        }
    }

    private void PickupAmmo()
    {
        if (playerManager != null && playerManager.playerInventoryManager.currentAmmoInInventory != null)
        {
            playerManager.playerInventoryManager.currentAmmoInInventory.ammoRemaining += ammoAmount;
            playerManager.playerUIManager.reservedAmmoCountText.text = playerManager.playerInventoryManager.currentAmmoInInventory.ammoRemaining.ToString();
        }

        Destroy(gameObject); // Elimina el objeto después de recogerlo
    }
}

