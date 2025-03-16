using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{
    public GameObject endGamePanel; // Panel de fin de partida
    public GameObject player; // Referencia al jugador

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // ✅ Solo el jugador puede activar el fin de la partida
        {
            Debug.Log("🎉 Fin de la partida alcanzado.");
            endGamePanel.SetActive(true); // 🔹 Muestra el panel de fin de partida
            DisablePlayerControls(); // 🔹 Bloquea los controles del jugador
        }
    }

    private void DisablePlayerControls()
    {
        if (player != null)
        {
            // 🔹 Deshabilitar el InputManager para bloquear controles
            InputManager inputManager = player.GetComponent<InputManager>();
            if (inputManager != null)
            {
                inputManager.enabled = false;
            }

            // 🔹 Deshabilitar CharacterController para evitar movimiento
            CharacterController characterController = player.GetComponent<CharacterController>();
            if (characterController != null)
            {
                characterController.enabled = false;
            }

            // 🔹 Bloquear la cámara si es necesario
            PlayerCamera playerCamera = player.GetComponent<PlayerCamera>();
            if (playerCamera != null)
            {
                playerCamera.enabled = false;
            }

            Debug.Log("🛑 Controles del jugador deshabilitados.");
        }
    }
}


