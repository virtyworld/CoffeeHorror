using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private Transform cameraTransform;
    private Rigidbody rb;
    private CameraHandler cameraHandler;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Ensure the Rigidbody is kinematic       

        cameraHandler = GetComponent<CameraHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        // Handle movement
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Используем локальные координаты transform'а
        Vector3 movement = transform.right * moveX + transform.forward * moveY;
        movement = movement.normalized;

        // Перемещаем персонажа в локальных координатах
        Vector3 newPosition = transform.position + movement * moveSpeed * Time.deltaTime;
        rb.MovePosition(newPosition);

        // Handle rotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(Vector3.up * mouseX);
    }
}
