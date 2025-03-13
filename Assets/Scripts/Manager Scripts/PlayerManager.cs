using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    Animator animator;
    public AnimatorManager animatorManager;

    public PlayerUIManager playerUIManager;
    PlayerCamera playerCamera;
    PlayerLocomotionManager playerLocomotionManager;
    public PlayerEquipmentManager playerEquipmentManager;

    [Header("Player Flags")]
    public bool disableRootMotion;
    public bool isPreformingAction;
    public bool isPreformingQuickTurn;
    public bool isAiming;

    private void Awake()
    {
        playerUIManager = FindObjectOfType<PlayerUIManager>();
        playerCamera = FindObjectOfType<PlayerCamera>();
        inputManager = GetComponent<InputManager>();
        animator = GetComponent<Animator>();
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
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
            playerEquipmentManager.currentWeapon.remainingAmmo = playerEquipmentManager.currentWeapon.remainingAmmo - 1;
            playerUIManager.currentAmmoCountText.text = playerEquipmentManager.currentWeapon.remainingAmmo.ToString();
            // In the future we will add the option to use knives also
            animatorManager.PlayAnimationWithOutRootMotion("Pistol_Shoot", true);

            // Se pasa el arma actual para que seleccione la animación correcta
            playerEquipmentManager.weaponAnimator.ShootWeapon(playerCamera, playerEquipmentManager.currentWeapon);
        }
        else
        {
            Debug.Log("CLICK (You are out of Ammo, RELOAD");
        }
    }
}




