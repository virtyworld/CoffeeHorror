using UnityEngine;

/// <summary>
/// PlayerMovement - First-Person Character Controller
/// 
/// Main Logic:
/// This script handles the player's movement and camera rotation in first-person view.
/// It provides smooth character movement using Rigidbody physics and mouse-look
/// functionality for camera rotation. The movement system uses velocity-based
/// physics for realistic movement while maintaining smooth camera control.
/// 
/// Key Features:
/// - WASD movement with configurable speed
/// - Mouse-look camera rotation with sensitivity control
/// - Physics-based movement using Rigidbody
/// - Smooth movement interpolation
/// - Collision detection for realistic movement
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float mouseSensitivity = 1f;
    [SerializeField] private Transform cameraTransform;
    private Rigidbody rb;
    private CameraHandler cameraHandler;
    private Vector3 moveDirection;

    /// <summary>
    /// Initializes the player movement system
    /// Sets up Rigidbody physics and camera handler for smooth movement
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraHandler = GetComponent<CameraHandler>();

        // Configure Rigidbody for smoother movement
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    /// <summary>
    /// Update method - currently unused but available for future input processing
    /// </summary>
    void Update()
    {

    }

    /// <summary>
    /// Handles physics-based movement and camera rotation
    /// Processes input for movement and rotation, applies physics-based movement
    /// </summary>
    void FixedUpdate()
    {
        // Handle movement input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Calculate movement direction in local space
        moveDirection = (transform.right * moveX + transform.forward * moveY).normalized;

        // Handle rotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(Vector3.up * mouseX);
        // Apply movement using velocity instead of MovePosition
        Vector3 targetVelocity = moveDirection * moveSpeed;
        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
    }
}
