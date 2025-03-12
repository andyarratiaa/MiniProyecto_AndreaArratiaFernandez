using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerCamera playerCamera;
    InputManager inputManager;
    Animator animator;
    AnimatorManager animatorManager;
    PlayerLocomotionManager playerLocomotionManager;
    PlayerEquipmentManager playerEquipmentManager;

    [Header("Player Flags")]
    public bool disableRootMotion;
    public bool isPreformingAction;
    public bool isPreformingQuickTurn;
    public bool isAiming;

    private void Awake()
    {
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

        // In the future we will add the option to use knives also
        animatorManager.PlayAnimationWithOutRootMotion("Pistol_Shoot", true);

        // Se pasa el arma actual para que seleccione la animación correcta
        playerEquipmentManager.weaponAnimator.ShootWeapon(playerCamera, playerEquipmentManager.currentWeapon);
    }
}




