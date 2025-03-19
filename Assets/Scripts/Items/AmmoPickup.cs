using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 15; // Cantidad de balas que otorga
    public AudioClip pickupSound; // 🔊 Sonido al recoger la munición

    private bool isPlayerNearby = false;
    private PlayerManager playerManager;
    private AudioSource audioSource;

    private void Start()
    {
        //Si no hay un AudioSource en el objeto, lo añadimos
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configuración del AudioSource
        audioSource.volume = 1.0f;
        audioSource.spatialBlend = 0f; //Sonido 2D para que siempre se escuche bien
        audioSource.playOnAwake = false; //Evita que el sonido se reproduzca al inicio
    }

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
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E)) //Espera a que el jugador presione E
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

        Debug.Log("✅ Munición recogida: " + ammoAmount);

        //Reproducir sonido de recogida de munición antes de destruir el objeto
        if (pickupSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(pickupSound);
            GetComponent<Collider>().enabled = false; //Desactiva colisión para evitar recolecciones múltiples
            Destroy(gameObject, pickupSound.length); //Se destruye tras reproducir el sonido
        }
        else
        {
            Destroy(gameObject); //Si no hay sonido, se destruye inmediatamente
        }
    }
}

