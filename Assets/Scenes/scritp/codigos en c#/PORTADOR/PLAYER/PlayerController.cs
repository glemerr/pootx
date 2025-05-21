using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement variables
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f; // Nueva variable para sprint
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float gravity = -19.62f; // Gravedad más realista (2x la normal)
    [SerializeField] private float rotationSpeed = 10f; // Aumentado para rotación más responsiva
    
    [Header("Movement Feel")]
    [SerializeField] private float accelerationTime = 0.1f; // Tiempo para alcanzar velocidad máxima
    [SerializeField] private float decelerationTime = 0.2f; // Tiempo para detenerse
    [SerializeField] private float airControlFactor = 0.5f; // Control en el aire (0-1)
    
    [Header("Jump Settings")]
    [SerializeField] private float coyoteTime = 0.15f; // Tiempo para saltar después de caer
    [SerializeField] private float jumpBufferTime = 0.1f; // Tiempo para registrar salto antes de tocar suelo
    [SerializeField] private float jumpCooldown = 0.1f; // Tiempo mínimo entre saltos
    [SerializeField] private float fallMultiplier = 2.5f; // Multiplicador de gravedad al caer

    // Ground check settings
    [Header("Ground Check")]
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private Vector3 groundCheckOffset = new Vector3(0, -0.05f, 0);
    [SerializeField] private LayerMask groundMask = ~0; // Todas las capas por defecto

    // Component references
    private CharacterController characterController;
    private Transform cameraTransform;
    private Vector3 velocity;
    private Vector3 currentMoveVelocity;
    private Vector3 targetMoveVelocity;
    private bool isGrounded;
    private float lastGroundedTime;
    private float lastJumpTime;
    private float lastJumpButtonTime;
    private bool isSprinting;

    // Input variables
    private float horizontalInput;
    private float verticalInput;
    private bool jumpInput;
    private bool sprintInput;

    void Start()
    {
        // Get component references
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        
        // Lock cursor for first-person control
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Check if character is grounded
        CheckGrounded();
        
        // Get input
        GetPlayerInput();
        
        // Handle movement
        MovePlayer();
        
        // Handle jumping
        HandleJump();
        
        // Apply gravity
        ApplyGravity();
        
        // Update timers
        UpdateTimers();
    }

    void GetPlayerInput()
    {
        // Get axis input for movement
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        
        // Get jump input
        jumpInput = Input.GetButtonDown("Jump");
        if (jumpInput)
        {
            lastJumpButtonTime = Time.time;
        }
        
        // Get sprint input
        sprintInput = Input.GetKey(KeyCode.LeftShift);
    }

    void CheckGrounded()
    {
        // Check if character is grounded using a sphere cast
        Vector3 spherePosition = transform.position + groundCheckOffset;
        isGrounded = Physics.CheckSphere(spherePosition, groundCheckRadius, groundMask, QueryTriggerInteraction.Ignore);
        
        // Update last grounded time for coyote time
        if (isGrounded)
        {
            lastGroundedTime = Time.time;
            
            // Reset vertical velocity when grounded
            if (velocity.y < 0)
            {
                velocity.y = -2f; // Small negative value to keep grounded
            }
        }
    }

    void MovePlayer()
    {
        // Calculate movement direction relative to camera
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        
        // Project vectors onto the horizontal plane
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Create movement direction vector
        Vector3 moveDirection = forward * verticalInput + right * horizontalInput;
        
        // Normalize movement if moving diagonally
        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }
        
        // Determine current speed based on sprint and grounded state
        float currentSpeed = moveSpeed;
        if (sprintInput && isGrounded)
        {
            currentSpeed = sprintSpeed;
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }
        
        // Apply air control factor when not grounded
        if (!isGrounded)
        {
            currentSpeed *= airControlFactor;
        }
        
        // Set target velocity
        targetMoveVelocity = moveDirection * currentSpeed;
        
        // Smooth acceleration/deceleration
        float smoothTime = moveDirection.magnitude > 0.01f ? accelerationTime : decelerationTime;
        currentMoveVelocity = Vector3.Lerp(
            currentMoveVelocity, 
            targetMoveVelocity, 
            Time.deltaTime / smoothTime
        );
        
        // Move the character
        characterController.Move(currentMoveVelocity * Time.deltaTime);
        
        // Rotate character to face movement direction
        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                targetRotation, 
                rotationSpeed * Time.deltaTime
            );
        }
    }

    void HandleJump()
    {
        // Check if we can jump (using coyote time and jump buffer)
        bool canJump = Time.time - lastGroundedTime <= coyoteTime;
        bool shouldJump = Time.time - lastJumpButtonTime <= jumpBufferTime;
        bool jumpCooldownPassed = Time.time - lastJumpTime >= jumpCooldown;
        
        // Jump when conditions are met
        if (canJump && shouldJump && jumpCooldownPassed)
        {
            // Apply jump force using the jump height formula
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            
            // Reset jump buffer
            lastJumpButtonTime = 0;
            
            // Set last jump time
            lastJumpTime = Time.time;
        }
        
        // Variable jump height - cut jump short if button released
        if (velocity.y > 0 && !Input.GetButton("Jump"))
        {
            velocity.y *= 0.5f;
        }
    }

    void ApplyGravity()
    {
        // Apply stronger gravity when falling for better feel
        if (velocity.y < 0)
        {
            velocity.y += gravity * fallMultiplier * Time.deltaTime;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
        
        // Move character vertically
        characterController.Move(velocity * Time.deltaTime);
    }
    
    void UpdateTimers()
    {
        // This function keeps track of time-based mechanics
        // Currently empty as timers are updated in their respective functions
    }
    
    // Visualize ground check in editor
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Vector3 spherePosition = transform.position + groundCheckOffset;
        Gizmos.DrawWireSphere(spherePosition, groundCheckRadius);
    }
}