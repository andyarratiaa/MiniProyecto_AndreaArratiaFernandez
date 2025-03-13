using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    AnimatorManager animatorManager;
    CharacterController characterController;
    Animator animator;
    PlayerManager playerManager;
    PlayerUIManager playerUIManager;

    [Header("Player Movement")]
    public float verticalMovementInput;
    public float horizontalMovementInput;
    private Vector2 movementInput;
    public float moveSpeed = 5f;
    public float gravity = 15f;
    public float jumpForce = 5f;
    public float groundCheckDistance = 1.2f;

    [Header("Camera Rotation")]
    public float verticalCameraInput;
    public float horizontalCameraInput;
    private Vector2 cameraInput;

    [Header("Button Inputs")]
    public bool shootInput; 
    public bool runInput;
    public bool quickTurnInput;
    public bool jumpInput;
    public bool aimingInput;
    public bool reloadInput;

    private Vector3 moveDirection;
    private float verticalVelocity = 0f;
    private bool isGrounded;

    
    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerManager = GetComponent<PlayerManager>();
        playerUIManager = FindObjectOfType<PlayerUIManager>();
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Run.performed += i => runInput = true;
            playerControls.PlayerMovement.Run.canceled += i => runInput = false;
            playerControls.PlayerMovement.QuickTurn.performed += i => quickTurnInput = true;
            playerControls.PlayerMovement.QuickTurn.canceled += i => quickTurnInput = false;
            playerControls.PlayerMovement.Jump.performed += i => jumpInput = true;
            playerControls.PlayerMovement.Jump.canceled += i => jumpInput = false;
            playerControls.PlayerActions.Aim.performed += i => aimingInput = true;
            playerControls.PlayerActions.Aim.canceled += i => aimingInput = false;
            playerControls.PlayerActions.Shoot.performed += i => shootInput = true;
            playerControls.PlayerActions.Shoot.canceled += i => shootInput = false;
            playerControls.PlayerActions.Reload.performed += i => reloadInput = true;
           

        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        HandleAllInputs();
        HandleGravityAndGrounding();
        ApplyMovement();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleCameraInput();
        HandleJumpInput();
        HandleQuickTurnInput();
        HandleAimingInput();
        HandleShootingInput();
        HandleReloadInput();
    }

    private void HandleMovementInput()
    {
        horizontalMovementInput = movementInput.x;
        verticalMovementInput = movementInput.y;
        animatorManager.HandleAnimatorValues(horizontalMovementInput, verticalMovementInput, runInput);

        //TEMP
        if (verticalMovementInput != 0 || horizontalMovementInput != 0)
        {
            animatorManager.rightHandIK.weight = 0;
            animatorManager.leftHandIK.weight = 0;
        }
        else
        {
            animatorManager.rightHandIK.weight = 1;
            animatorManager.leftHandIK.weight = 1;
        }
    }

    private void HandleCameraInput()
    {
        horizontalCameraInput = cameraInput.x;
        verticalCameraInput = cameraInput.y;
    }

    private void HandleJumpInput()
    {
        if (jumpInput && isGrounded)
        {
            animatorManager.TriggerJump();
            verticalVelocity = Mathf.Sqrt(2f * gravity * jumpForce); // Calcula la fuerza del salto
            isGrounded = false; // Marca que está en el aire
        }
    }

    private void HandleQuickTurnInput()
    {
        if (playerManager.isPreformingAction)
            return;

        if (quickTurnInput)
        {
            animator.SetBool("isPreformingQuickTurn", true);
            animatorManager.PlayAnimationWithOutRootMotion("Quick Turn", true);
        }
    }

    private void HandleGravityAndGrounding()
    {
        // Verificamos si el personaje está tocando el suelo con un raycast
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        isGrounded = Physics.Raycast(rayOrigin, Vector3.down, out hit, groundCheckDistance);

        if (isGrounded)
        {
            verticalVelocity = -2f; // Pequeña fuerza para mantener contacto con el suelo
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
    }

    private void ApplyMovement()
    {
        // Mantener el movimiento en el plano horizontal sin afectar la altura
        Vector3 moveDir = transform.right * horizontalMovementInput + transform.forward * verticalMovementInput;
        moveDir.Normalize();

        // Aplicar la velocidad y la gravedad
        Vector3 finalMove = moveDir * moveSpeed * Time.deltaTime;
        finalMove.y = verticalVelocity * Time.deltaTime;

        characterController.Move(finalMove);

        // Resetear el input de salto después de aplicarlo
        jumpInput = false;
    }

    private void HandleAimingInput()
    {
        if (verticalMovementInput != 0 || horizontalMovementInput != 0)
        {
            aimingInput = false;
            animator.SetBool("isAiming", false);
            playerUIManager.crossHair.SetActive(false);
            return;
        }

        if (aimingInput)
        {
            animator.SetBool("isAiming", true);
            playerUIManager.crossHair.SetActive(true);

        }
        else
        {
            animator.SetBool("isAiming", false);
            playerUIManager.crossHair.SetActive(false);

        }

        animatorManager.UpdateAimConstraints();
    }

    private void HandleShootingInput()
    {
        if(shootInput && aimingInput)
        {
            shootInput = false;
            //Debug.Log("Bang");
            playerManager.UseCurrentWeapon();
        }
    }

    private void HandleReloadInput()
    {
        // No permitir recargar si el jugador está realizando otra acción
        if (playerManager.isPreformingAction)
            return;

        if (reloadInput)
        {
            reloadInput = false;
           

            WeaponItem currentWeapon = playerManager.playerEquipmentManager.currentWeapon;

            if (currentWeapon == null)
            {
                Debug.Log("No hay arma equipada.");
                return;
            }

            // Obtener el límite de munición del arma actual
            int maxAmmo = currentWeapon.maxAmmo;
            int ammoNeeded = maxAmmo - currentWeapon.remainingAmmo;

            // Determinar la munición disponible en el inventario según el arma
            int ammoFromInventory = 0;

            if (currentWeapon.weaponName == "Pistola") // Si es la pistola
            {
                ammoFromInventory = playerManager.playerEquipmentManager.pistolAmmoInventory;
            }
            else if (currentWeapon.weaponName == "Rifle") // Si es el rifle
            {
                ammoFromInventory = playerManager.playerEquipmentManager.rifleAmmoInventory;
            }

            if (ammoNeeded > 0 && ammoFromInventory > 0) // Solo recargar si no está llena y hay balas en el inventario
            {
                playerManager.animatorManager.ClearHandIKWeights();
                playerManager.animatorManager.PlayAnimation("Reload", true);

                // Obtener la cantidad de balas a recargar sin exceder el inventario
                int ammoToReload = Mathf.Min(ammoNeeded, ammoFromInventory);
                currentWeapon.remainingAmmo += ammoToReload;

                // Restar las balas usadas del inventario
                if (currentWeapon.weaponName == "Pistola")
                {
                    playerManager.playerEquipmentManager.pistolAmmoInventory -= ammoToReload;
                }
                else if (currentWeapon.weaponName == "Rifle")
                {
                    playerManager.playerEquipmentManager.rifleAmmoInventory -= ammoToReload;
                }

                // Actualizar la UI con la nueva cantidad de munición en el cargador
                playerManager.playerUIManager.currentAmmoCountText.text = currentWeapon.remainingAmmo.ToString();
            }
            else
            {
                Debug.Log("No hay suficiente munición para recargar o el cargador ya está lleno.");
            }
        }
    }

}


