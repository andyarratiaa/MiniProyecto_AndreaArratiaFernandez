using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    [Header("Player Health")]
    public float maxHealth = 100f;
    public float currentHealth;
    public Image healthBar;
    public Animator animator;

    private Vector3 startPosition; // Posición inicial del jugador
    private Quaternion startRotation; // Rotación inicial del jugador
    private bool isDead = false;

    private Color greenColor = new Color(0f, 1f, 0f);
    private Color yellowColor = new Color(1f, 1f, 0f);
    private Color redColor = new Color(1f, 0f, 0f);

    private CharacterController characterController;
    private PlayerManager playerManager;
    private PlayerLocomotionManager locomotionManager;
    private InputManager inputManager;

    private void Start()
    {
        // Si hay una salud guardada, la recuperamos
        if (PlayerPrefs.HasKey("PlayerHealth"))
        {
            currentHealth = PlayerPrefs.GetFloat("PlayerHealth");
        }
        else
        {
            currentHealth = maxHealth; // Si no hay registro, iniciar con salud máxima
        }

        animator = GetComponent<Animator>();

        startPosition = transform.position;
        startRotation = transform.rotation;

        characterController = GetComponent<CharacterController>();
        playerManager = GetComponent<PlayerManager>();
        locomotionManager = GetComponent<PlayerLocomotionManager>();
        inputManager = GetComponent<InputManager>();

        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        playerManager.PlayHurtSound();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        if(healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
            healthBar.color = GetHealthColor();
        }
    }

    Color GetHealthColor()
    {
        float healthPercentage = currentHealth / maxHealth;

        if (healthPercentage > 0.5f)
        {
            return Color.Lerp(yellowColor, greenColor, (healthPercentage - 0.5f) * 2);
        }
        else
        {
            return Color.Lerp(redColor, yellowColor, healthPercentage * 2);
        }
    }


    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("El jugador ha muerto.");

        // Reproducir la animación de muerte
        animator.SetTrigger("Die");

        // Deshabilitar el control del jugador temporalmente
        playerManager.enabled = false;
        locomotionManager.enabled = false;
        characterController.enabled = false;
        inputManager.enabled = false;

        // Detener cualquier movimiento residual
        if (characterController != null) characterController.SimpleMove(Vector3.zero);
        if (GetComponent<Rigidbody>() != null)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }

        // Iniciar el respawn
        StartCoroutine(RespawnPlayer());
    }

    IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(2f); // Esperar antes de revivir

        // Desactivar el Animator antes de mover al personaje
        animator.enabled = false;

        // Restaurar posición y rotación
        transform.position = startPosition;
        transform.rotation = startRotation;

        // Restaurar salud y la barra de vida
        currentHealth = maxHealth;
        UpdateHealthBar();

        // Reactivar el Animator
        animator.enabled = true;
        animator.ResetTrigger("Die");
        animator.SetTrigger("Respawn"); // Transición de Death a Stand Up
        animator.Play("Stand Up");

        // Esperar a que termine la animación de Stand Up
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Reactivar control del jugador
        playerManager.enabled = true;
        locomotionManager.enabled = true;
        characterController.enabled = true;
        inputManager.enabled = true;

        // Resetear input para que no se mueva solo
        ResetPlayerInput();

        // Cambiar a la animación de locomotion
        animator.SetTrigger("StandUpFinished");

        // Resetear el estado de muerte
        isDead = false;

        Debug.Log("El jugador ha revivido correctamente.");
    }

    void ResetPlayerInput()
    {
        if (inputManager != null)
        {
            inputManager.ResetInputs(); // Debes implementar este método en InputManager
        }
    }
}







