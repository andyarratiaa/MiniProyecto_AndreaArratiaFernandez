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

    private Vector3 startPosition; // Guardará la posición inicial del jugador
    private Quaternion startRotation; // Guardará la rotación inicial del jugador
    private bool isDead = false;

    private Color greenColor = new Color(0f, 1f, 0f);
    private Color yellowColor = new Color(1f, 1f, 0f);
    private Color redColor = new Color(1f, 0f, 0f);

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        // Guardar la posición y rotación inicial del jugador
        startPosition = transform.position;
        startRotation = transform.rotation;

        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        if (healthBar != null)
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
        GetComponent<PlayerManager>().enabled = false;
        GetComponent<PlayerLocomotionManager>().enabled = false;
        GetComponent<CharacterController>().enabled = false;

        // Deshabilitar InputManager para bloquear controles completamente
        if (GetComponent<InputManager>() != null)
        {
            GetComponent<InputManager>().enabled = false;
        }

        // Opcional: Desactivar la cámara si deseas un efecto más dramático
        // if (Camera.main != null) Camera.main.GetComponent<PlayerCamera>().enabled = false;

        // Esperar unos segundos antes de revivir
        StartCoroutine(RespawnPlayer());
    }

    IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(5f); // Esperar 6 segundos antes de revivir

        // Restaurar salud y la barra de vida
        currentHealth = maxHealth;
        UpdateHealthBar();

        // Restaurar posición y rotación del jugador
        transform.position = startPosition;
        transform.rotation = startRotation;

        // Habilitar nuevamente los scripts del jugador
        GetComponent<PlayerManager>().enabled = true;
        GetComponent<PlayerLocomotionManager>().enabled = true;
        GetComponent<CharacterController>().enabled = true;

        // Reactivar InputManager
        if (GetComponent<InputManager>() != null)
        {
            GetComponent<InputManager>().enabled = true;
        }

        // Opcional: Reactivar la cámara si se había desactivado
        // if (Camera.main != null) Camera.main.GetComponent<PlayerCamera>().enabled = true;

        // Resetear el estado de muerte
        isDead = false;

        Debug.Log("El jugador ha revivido en el punto de inicio.");
    }
}





