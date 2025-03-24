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
    PlayerCamera playerCamera; // Debe estar declarada aquí

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

    [Header("Pause System")]
    public GameObject PausePanel;
    private bool isPaused = false;

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
        playerCamera = FindObjectOfType<PlayerCamera>();
        //Asegurar que el menú de pausa está oculto al inicio
        PausePanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
            playerControls.PlayerActions.Pause.performed += i => TogglePause(); //Detecta el input de pausa
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        if (isPaused) return; //Bloqueamos inputs mientras está pausado

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

    public void TogglePause()
    {
        if (isPaused)
        {
            ContinueGame(); //Llama a la función de continuar si está pausado
        }
        else
        {
            PauseGame(); //Pausa el juego si no está pausado
        }
    }

    public void PauseGame()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;

        // Mostrar cursor en el menú de pausa
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //Deshabilitar la cámara para evitar que siga rotando
        if (playerCamera != null)
        {
            playerCamera.enabled = false;
        }

        //Resetear inputs de la cámara para evitar que siga en movimiento
        horizontalCameraInput = 0f;
        verticalCameraInput = 0f;
        cameraInput = Vector2.zero;

        playerControls.Disable();
    }


    public void ContinueGame()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;

        // Ocultar cursor al volver al juego
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (playerCamera != null)
        {
            playerCamera.enabled = true;
        }

        //También reiniciamos los valores de entrada al volver al juego
        cameraInput = Vector2.zero;
        horizontalCameraInput = 0f;
        verticalCameraInput = 0f;

        playerControls.Enable();
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

    private void HandleShootingInput()
    {
        if (!aimingInput) //EVITA DISPARAR SI NO ESTÁ APUNTANDO
        {
            Debug.Log("No puedes disparar sin apuntar.");
            return;
        }

        if (shootInput)
        {
            shootInput = false; //Desactiva el input inmediatamente

            WeaponItem currentWeapon = player.playerEquipmentManager.currentWeapon;

            if (currentWeapon.remainingAmmo > 0)
            {
                Debug.Log("Disparando con " + currentWeapon.remainingAmmo + " balas restantes.");

                player.UseCurrentWeapon();

                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot"))
                {
                    animator.Play("Shoot");
                }
            }
            else
            {
                Debug.Log("Sin balas. Intentando recargar...");
                HandleReloadInput();
            }
        }

    }


    //Método para evitar que `shootInput` se desactive instantáneamente
    private IEnumerator ResetShootInput()
    {
        yield return new WaitForSeconds(0.1f); //Pequeña espera para procesar la acción
        shootInput = false; //Ahora sí, se cancela el input
    }


    private void HandleReloadInput()
    {
        if (player.isPreformingAction) return;

        if (reloadInput)
        {
            reloadInput = false;

            WeaponItem currentWeapon = player.playerEquipmentManager.currentWeapon;
            BoxOfAmmoItem ammoInventory = player.playerInventoryManager.currentAmmoInInventory;

            if (currentWeapon.remainingAmmo == currentWeapon.maxAmmo)
            {
                Debug.Log("El arma ya está completamente cargada.");
                return;
            }

            if (ammoInventory != null && ammoInventory.ammoType == currentWeapon.ammoType)
            {
                if (ammoInventory.ammoRemaining == 0)
                {
                    Debug.Log("No hay munición en el inventario.");
                    return;
                }

                int ammoNeeded = currentWeapon.maxAmmo - currentWeapon.remainingAmmo;

                if (ammoInventory.ammoRemaining >= ammoNeeded)
                {
                    currentWeapon.remainingAmmo += ammoNeeded;
                    ammoInventory.ammoRemaining -= ammoNeeded;
                }
                else
                {
                    currentWeapon.remainingAmmo += ammoInventory.ammoRemaining;
                    ammoInventory.ammoRemaining = 0;
                }

                //Reproduce la animación de recarga pero no bloquea el disparo
                player.animatorManager.PlayAnimation("Reload", false);
                player.PlayReloadSound();

                // Forzar actualización inmediata del contador de balas
                player.playerUIManager.currentAmmoCountText.text = currentWeapon.remainingAmmo.ToString();
                player.playerUIManager.reservedAmmoCountText.text = ammoInventory.ammoRemaining.ToString();

                Debug.Log("Recarga completada. Balas en el arma: " + currentWeapon.remainingAmmo);

                //NO ACTIVAR `shootInput = true;` después de recargar
            }
        }
    }

    private void HandleMovementInput()
    {
        horizontalMovementInput = movementInput.x;
        verticalMovementInput = movementInput.y;
        animatorManager.HandleAnimatorValues(horizontalMovementInput, verticalMovementInput, runInput);

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

    private void HandleGravityAndGrounding()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        isGrounded = Physics.Raycast(rayOrigin, Vector3.down, out hit, groundCheckDistance);

        verticalVelocity = isGrounded ? -2f : verticalVelocity - (gravity * Time.deltaTime);
    }

    private void ApplyMovement()
    {
        Vector3 moveDir = transform.right * horizontalMovementInput + transform.forward * verticalMovementInput;
        moveDir.Normalize();

        Vector3 finalMove = moveDir * moveSpeed * Time.deltaTime;
        finalMove.y = verticalVelocity * Time.deltaTime;

        characterController.Move(finalMove);
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

        animator.SetBool("isAiming", aimingInput);
        playerUIManager.crossHair.SetActive(aimingInput);
        animatorManager.UpdateAimConstraints();
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
