using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// CameraHandler - First-Person Camera Control System
/// 
/// Main Logic:
/// This script handles the vertical camera rotation (pitch) for first-person view.
/// It processes mouse input for looking up and down, applies sensitivity settings,
/// and clamps the rotation to prevent over-rotation. The horizontal rotation is
/// handled by the PlayerMovement script, while this focuses on vertical movement.
/// 
/// Key Features:
/// - Vertical camera rotation (pitch) control
/// - Mouse sensitivity configuration
/// - Rotation clamping to prevent over-rotation
/// - Cursor locking for immersive experience
/// - Integration with player movement system
/// </summary>
public class CameraHandler : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform playerBody;

    private float xRotation = 0f;

    /// <summary>
    /// Initializes the camera system
    /// Locks and hides the cursor for immersive first-person experience
    /// </summary>
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Handles vertical camera rotation based on mouse input
    /// Processes mouse Y-axis input and applies clamped rotation to the camera
    /// </summary>
    private void FixedUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Handle camera up/down rotation
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply camera rotation
        camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
