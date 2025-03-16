using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    AnimatorManager animatorManager;
    CharacterController characterController;
    Animator animator;
    PlayerManager player;
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
        player = GetComponent<PlayerManager>();
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
        if (player.isPreformingAction)
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
        if (shootInput && aimingInput)
        {
            shootInput = false;
            //Debug.Log("Bang");
            player.UseCurrentWeapon();
        }
    }

    private void HandleReloadInput()
    {
        // We do not want to be able to reload while being damaged, shooting, quick turning etc
        if (player.isPreformingAction)
            return;

        if (reloadInput)
        {
            reloadInput = false;

            // Check to see if our weapon is currently full, if it is return
            if (player.playerEquipmentManager.currentWeapon.remainingAmmo == player.playerEquipmentManager.currentWeapon.maxAmmo)
            {
                Debug.Log("AMMO ALREADY FULL");
                return;
            }

            // Check to see if we have ammo in our inventory for this particular weapon, if we do not, return
            if (player.playerInventoryManager.currentAmmoInInventory != null)
            {
                if (player.playerInventoryManager.currentAmmoInInventory.ammoType == player.playerEquipmentManager.currentWeapon.ammoType)
                {
                    if (player.playerInventoryManager.currentAmmoInInventory.ammoRemaining == 0)
                    {
                        return;
                    }
                    int amountOfAmmoToReload;
                    amountOfAmmoToReload = player.playerEquipmentManager.currentWeapon.maxAmmo - player.playerEquipmentManager.currentWeapon.remainingAmmo;

                    //If we have more ammo remaining than we need to drop into our weapon, we substract the amount needed from out TOTAL AMOUNT in our players inventory
                    if (player.playerInventoryManager.currentAmmoInInventory.ammoRemaining >= amountOfAmmoToReload)
                    {
                        player.playerEquipmentManager.currentWeapon.remainingAmmo = player.playerEquipmentManager.currentWeapon.remainingAmmo + amountOfAmmoToReload;
                        player.playerInventoryManager.currentAmmoInInventory.ammoRemaining =
                            player.playerInventoryManager.currentAmmoInInventory.ammoRemaining - amountOfAmmoToReload;
                    }
                    else
                    {
                        player.playerEquipmentManager.currentWeapon.remainingAmmo = player.playerInventoryManager.currentAmmoInInventory.ammoRemaining;
                        player.playerInventoryManager.currentAmmoInInventory.ammoRemaining = 0;
                    }

                    player.animatorManager.ClearHandIKWeights();
                    player.animatorManager.PlayAnimation("Reload", true);
                    player.PlayReloadSound(); // 🔊 Reproduce el sonido de recarga

                    // PLACE MORE AMMO IN THE WEAPON
                    player.playerUIManager.currentAmmoCountText.text = player.playerEquipmentManager.currentWeapon.remainingAmmo.ToString();

                    player.playerUIManager.reservedAmmoCountText.text = player.playerInventoryManager.currentAmmoInInventory.ammoRemaining.ToString();

                    // UPDATE RESERVED AMMO COUNT
                    // SUBTRACT THE PLACED AMMO FROM OUR INVENTORY
                }
            }
        }
    }

    public void ResetInputs()
    {
        movementInput = Vector2.zero;
        horizontalMovementInput = 0f;
        verticalMovementInput = 0f;
        horizontalCameraInput = 0f;
        verticalCameraInput = 0f;

        shootInput = false;
        runInput = false;
        quickTurnInput = false;
        jumpInput = false;
        aimingInput = false;
        reloadInput = false;
    }
}


