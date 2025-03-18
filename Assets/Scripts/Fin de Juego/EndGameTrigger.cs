using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{
    public GameObject endGamePanel; // Panel de fin de partida
    public GameObject player; // Referencia al jugador

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // ✅ Solo el jugador puede activar el fin de la partida
        {
            Debug.Log("Fin de la partida alcanzado.");
            endGamePanel.SetActive(true); // 🔹 Muestra el panel de fin de partida
            DisablePlayerControls(); // 🔹 Bloquea los controles del jugador
            ShowCursor(); // 🔹 Muestra el cursor
        }
    }

    private void DisablePlayerControls()
    {
        if (player != null)
        {
            // 🔹 Bloquear la animación y dejar al jugador en Idle
            Animator animator = player.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play("Idle"); // 🔹 Forzar al estado Idle
                animator.speed = 0;    // 🔹 Congelar cualquier animación en curso
            }

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

            // 🔹 Detener cualquier movimiento acumulado
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true; // 🔹 Evita que fuerzas externas lo muevan
            }

            // 🔹 Bloquear la cámara si es necesario
            PlayerCamera playerCamera = player.GetComponent<PlayerCamera>();
            if (playerCamera != null)
            {
                playerCamera.enabled = false;
            }

            Debug.Log("Controles del jugador y animaciones deshabilitados.");
        }
    }

    private void ShowCursor()
    {
        Cursor.visible = true; // 🔹 Hace visible el cursor
        Cursor.lockState = CursorLockMode.None; // 🔹 Desbloquea el cursor
        Debug.Log("Cursor activado.");
    }
}




