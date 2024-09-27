using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{   
    
    [Header("Ref")]
    public CharacterController controller;  // Reference to the CharacterController
    public Transform cameraTransform;
    public static Player instance;

    [Header("Player Speeds && Mouse Sens")]
    public float moveSpeed = 5f;            // Movement speed
    public float mouseSensitivity = 100f; 
    public float mouseSensitivityScope = 60f; 
    public float mouseSensitivityNormal = 60f; 
    [Header("Gravity && Jump")]  
    public float gravity = -9.81f;          // Gravity value
    public float jumpHeight = 2f;           // How high the player jumps
    public float landingRecoil = 1f;  

    

    private GunManager gunManager;

    private Vector2 movementInput;          // Store movement input from new Input System
    private Vector2 lookInput;              // Store mouse look input
    private Vector3 velocity;               // Store velocity to apply gravity and jumping
    private float xRotation = 0f;           // Vertical rotation for looking up and down

    private bool isGrounded;                // Check if player is on the ground
    private bool wasGrounded;               // Check if the player was grounded in the previous frame
    private bool hasJumped;    
    

private void OnEnable()
{
    var playerInput = new Controller();  // Assuming PlayerInput is generated from Input Action Asset
    gunManager = FindObjectOfType<GunManager>();

    instance = this;

    // Movement
    playerInput.Main.Movement.performed += OnMovePerformed;
    playerInput.Main.Movement.canceled += OnMoveCanceled;
    playerInput.Main.Movement.Enable();

    // Mouse look
    playerInput.Main.Aim.performed += OnLookPerformed;
    playerInput.Main.Aim.canceled += OnLookCanceled;
    playerInput.Main.Aim.Enable();

    // Jump
    playerInput.Main.Jump.performed += OnJumpPerformed;
    playerInput.Main.Jump.Enable();

    // Shooting
    playerInput.Main.Shooting.performed += context => gunManager.ShootGun();
    playerInput.Main.Shooting.Enable();

    // Reload
    playerInput.Main.Reload.performed += context => gunManager.ReloadGun();
    playerInput.Main.Reload.Enable();

    // Weapon switching
    playerInput.Main.SwitchWeapon.performed += context => FindObjectOfType<WeaponSwitcher>().OnSwitchWeapon(context);
    playerInput.Main.SwitchWeapon.Enable();

    playerInput.Main.Scope.performed += context => gunManager.Scope();
    playerInput.Main.Scope.Enable();

}


    private void OnDisable()
    {
        var playerInput = new Controller();
        playerInput.Main.Movement.Disable();
        playerInput.Main.Aim.Disable();
        playerInput.Main.Jump.Disable();
        playerInput.Main.Shooting.Disable();
        playerInput.Main.SwitchWeapon.Disable();
        playerInput.Main.Scope.Disable();
    }

    // Called when movement keys are pressed
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    // Called when movement keys are released
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        movementInput = Vector2.zero;
    }

    // Called when the mouse is moved
    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    // Called when mouse movement stops
    private void OnLookCanceled(InputAction.CallbackContext context)
    {
        lookInput = Vector2.zero;
    }

    // Called when the jump button is pressed
    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            // Calculate jump velocity based on jump height and gravity
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            hasJumped = true; // Mark that the player has jumped
        }
    }



    private void Update()
    {
        if(gunManager.scopbool())
        {
            mouseSensitivity = mouseSensitivityScope;
        }
        else if(!gunManager.scopbool())
        {
            mouseSensitivity = mouseSensitivityNormal;
        }

        wasGrounded = isGrounded;
        isGrounded = controller.isGrounded;

        // If the player was not grounded in the previous frame but is grounded now, apply recoil only if they jumped
        if (isGrounded && !wasGrounded && hasJumped)
        {
            ApplyLandingRecoil();
            hasJumped = false; // Reset jump state after applying recoil
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Ensures the player stays grounded
        }

        // Handle player movement
        Vector3 moveDirection = transform.right * movementInput.x + transform.forward * movementInput.y;
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Handle mouse look/aim
        HandleMouseLook();
    }

    private void HandleMouseLook()
    {
        // Get mouse movement and apply sensitivity
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        // Rotate the player horizontally
        transform.Rotate(Vector3.up * mouseX);

        // Rotate the camera vertically (clamping to prevent flipping)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit up/down rotation

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    // Function to apply recoil when landing after a jump
    private void ApplyLandingRecoil()
    {
        // Apply upward recoil when landing after jumping
        velocity.y = Mathf.Sqrt(landingRecoil * -2f * gravity);
    }
}
