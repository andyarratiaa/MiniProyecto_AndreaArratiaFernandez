using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieManager : MonoBehaviour
{
    public ZombieAnimatorManager zombieAnimatorManager;
    public ZombieStatManager zombieStatManager;

    public IdleState startingState;

    [Header("Flags")]
    public bool isPerformingAction;
    public bool isDead;

    [Header("Current State")]
    [SerializeField] private State currentState;

    [Header("Current Target")]
    public PlayerManager currentTarget;
    public Vector3 targetsDirection;
    public float distanceFromCurrentTarget;
    public float viewableAngleFromCurrentTarget;

    [Header("Animator")]
    public Animator animator;

    [Header("Navmesh Agent")]
    public NavMeshAgent zombieNavmeshAgent;

    [Header("Rigidbody")]
    public Rigidbody zombieRigidbody;

    [Header("Locomotion")]
    public float rotationSpeed = 5;

    [Header("Attack")]
    public float attackCoolDownTimer;
    public float minimumAttacKDistance = 1f;
    public float maximumAttackDistance = 1.5f;

    [Header("Zombie Audio")] // 🔊 NUEVO
    public AudioSource audioSource;
    //public AudioClip walkSound;
    public AudioClip attackSound;
    public AudioClip deathSound;
    public AudioClip damageSound;

    //[Header("Key Drop")]
    //public GameObject keyObject; // Referencia a la llave en la jerarquía

    private void Awake()
    {
        currentState = startingState;
        animator = GetComponent<Animator>();
        zombieNavmeshAgent = GetComponentInChildren<NavMeshAgent>();
        zombieRigidbody = GetComponent<Rigidbody>();
        zombieAnimatorManager = GetComponent<ZombieAnimatorManager>();
        zombieStatManager = GetComponent<ZombieStatManager>();

        // 🔊 Inicializar el AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (!isDead)
        {
            HandleStateMachine();

            // Mantener al zombi pegado al suelo
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
            {
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            }
        }
    }

    private void Update()
    {
        zombieNavmeshAgent.transform.localPosition = Vector3.zero;

        if (attackCoolDownTimer > 0)
        {
            attackCoolDownTimer -= Time.deltaTime;
        }

        if (currentTarget != null)
        {
            targetsDirection = currentTarget.transform.position - transform.position;
            viewableAngleFromCurrentTarget = Vector3.SignedAngle(targetsDirection, transform.forward, Vector3.up);
            distanceFromCurrentTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
        }
    }

    private void HandleStateMachine()
    {
        State nextState;

        if (currentState != null)
        {
            nextState = currentState.Tick(this);
            if (nextState != null)
            {
                currentState = nextState;
            }
        }
    }

    private void HandleDeath()
    {
        if (!isDead)
            return;

        // Desactivar todos los colliders
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        // Deshabilitar el NavMeshAgent para que no interfiera con la caída
        if (zombieNavmeshAgent != null)
        {
            zombieNavmeshAgent.enabled = false;
        }

        // Activar la gravedad y permitir que el zombi caiga
        if (zombieRigidbody != null)
        {
            zombieRigidbody.useGravity = true;
            zombieRigidbody.isKinematic = false;
        }

        // Reproducir la animación de muerte
        animator.Play("Dead Zombie");

        //// Activar la llave si está asignada
        //if (keyObject != null)
        //{
        //    keyObject.SetActive(true);
        //}
    }

    //public void PlayWalkSound()
    //{
    //    if (audioSource != null && walkSound != null && !audioSource.isPlaying)
    //    {
    //        audioSource.PlayOneShot(walkSound);
    //    }
    //}

    public void PlayAttackSound()
    {
        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }
    }

    public void PlayDeathSound()
    {
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
    }

    public void PlayDamageSound() // 🔊 Nuevo método para reproducir sonido de daño
    {
        if (audioSource != null && damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }
    }

    public void Die()
    {
        if (isDead)
            return;

        isDead = true;
        PlayDeathSound();
        HandleDeath();
    }
}


