using UnityEngine;
using UnityEngine.InputSystem;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform playerBody;

    private float xRotation = 0f;
    private float yRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Handle Y rotation (left/right)
        yRotation += mouseX;

        if (yRotation > 90f)
        {
            float excessRotation = yRotation - 90f;
            yRotation = 90f;
            playerBody.Rotate(Vector3.up * excessRotation);
        }
        else if (yRotation < -90f)
        {
            float excessRotation = yRotation + 90f;
            yRotation = -90f;
            playerBody.Rotate(Vector3.up * excessRotation);
        }

        // Handle X rotation (up/down)
        float nextXRotation = xRotation - mouseY;

        if (nextXRotation <= 65f && nextXRotation >= -90f)
        {
            xRotation = nextXRotation;
        }

        // Apply rotations
        camera.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
