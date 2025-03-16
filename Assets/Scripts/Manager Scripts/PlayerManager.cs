using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public InputManager inputManager;
    Animator animator;
    public AnimatorManager animatorManager;

    public PlayerUIManager playerUIManager;
    public PlayerCamera playerCamera;
    PlayerLocomotionManager playerLocomotionManager;
    public PlayerEquipmentManager playerEquipmentManager;
    public PlayerInventoryManager playerInventoryManager;

    [Header("Player Flags")]
    public bool disableRootMotion;
    public bool isPreformingAction;
    public bool isPreformingQuickTurn;
    public bool isAiming;
    public bool canInteract;

    private AudioSource audioSource; // 🔊 AudioSource para sonidos de disparo

    private void Awake()
    {
        playerUIManager = FindObjectOfType<PlayerUIManager>();
        playerCamera = FindObjectOfType<PlayerCamera>();
        inputManager = GetComponent<InputManager>();
        animator = GetComponent<Animator>();
        animatorManager = GetComponent<AnimatorManager>();

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();

        // 🔊 Inicializar el AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        inputManager.HandleAllInputs();

        isPreformingAction = animator.GetBool("isPreformingAction");
        isPreformingQuickTurn = animator.GetBool("isPreformingQuickTurn");
        disableRootMotion = animator.GetBool("disableRootMotion");

        isAiming = animator.GetBool("isAiming");
    }

    private void FixedUpdate()
    {
        playerLocomotionManager.HandleAllLocomotion();
    }

    private void LateUpdate()
    {
        playerCamera.HandleAllCameraMovement();
    }

    public void UseCurrentWeapon()
    {
        if (isPreformingAction)
            return;

        if (playerEquipmentManager.currentWeapon.remainingAmmo > 0)
        {
            playerEquipmentManager.currentWeapon.remainingAmmo -= 1;
            playerUIManager.currentAmmoCountText.text = playerEquipmentManager.currentWeapon.remainingAmmo.ToString();

            // 🔹 Reproducir la animación de disparo
            animatorManager.PlayAnimationWithOutRootMotion("Pistol_Shoot", true);

            // 🔊 Reproducir sonido del disparo
            PlayWeaponSound();

            // 🔹 Disparar el arma con la animación correcta
            playerEquipmentManager.weaponAnimator.ShootWeapon(playerCamera, playerEquipmentManager.currentWeapon);
        }
        else
        {
            Debug.Log("CLICK (You are out of Ammo, RELOAD)");
        }
    }

    private void PlayWeaponSound()
    {
        if (playerEquipmentManager.currentWeapon.shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(playerEquipmentManager.currentWeapon.shootSound);
        }
    }
}




