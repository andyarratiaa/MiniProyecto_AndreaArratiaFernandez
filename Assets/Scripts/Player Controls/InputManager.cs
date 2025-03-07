//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class InputManager : MonoBehaviour
//{
//    PlayerControls playerControls;
//    AnimatorManager animatorManager;

//    [Header("Player Movement")]
//    public float verticalMovementInput;
//    public float horizontalMovementInput;
//    private Vector2 movementInput;

//    [Header("Camera Rotation")]
//    public float verticalCameraInput;
//    public float horizontalCameraInput;
//    private Vector2 cameraInput;

//    [Header("Button Inputs")]
//    public bool runInput;

//    private void Awake()
//    {
//        animatorManager = GetComponent<AnimatorManager>();
//    }

//    private void OnEnable()
//    {
//        if (playerControls == null)
//        {
//            playerControls = new PlayerControls();

//            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
//            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
//            playerControls.PlayerMovement.Run.performed += i => runInput = true;
//            playerControls.PlayerMovement.Run.canceled += i => runInput = false;
//        }

//        playerControls.Enable();
//    }


//    private void OnDisable()
//    {
//        playerControls.Disable();
//    }

//    public void HandleAllInputs()
//    {
//        HandleMovementInput();
//        HandleCameraInput();
//    }

//    private void HandleMovementInput()
//    {
//        horizontalMovementInput = movementInput.x;
//        verticalMovementInput = movementInput.y;
//        animatorManager.HandleAnimatorValues(horizontalMovementInput, verticalMovementInput, runInput);
//    }

//    private void HandleCameraInput()
//    {
//        horizontalCameraInput = cameraInput.x;
//        verticalCameraInput = cameraInput.y;
//    }

//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    AnimatorManager animatorManager;
    CharacterController characterController;

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
    public bool runInput;
    public bool jumpInput;

    private Vector3 moveDirection;
    private float verticalVelocity = 0f;
    private bool isGrounded;


    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        characterController = GetComponent<CharacterController>();
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
            playerControls.PlayerMovement.Jump.performed += i => jumpInput = true;
            playerControls.PlayerMovement.Jump.canceled += i => jumpInput = false;
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
    }

    private void HandleMovementInput()
    {
        horizontalMovementInput = movementInput.x;
        verticalMovementInput = movementInput.y;
        animatorManager.HandleAnimatorValues(horizontalMovementInput, verticalMovementInput, runInput);
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
}


