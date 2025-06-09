using UnityEngine;
using UnityEngine.Events;

public class BehindTheBack : MonoBehaviour
{
    [SerializeField] private GameObject spawnedObject; // The crab object
    [SerializeField] private Camera camera; // The camera to track rotation
    [SerializeField] private float rotationThreshold = 100f; // Speed threshold for quick rotation
    [SerializeField] private float moveSpeed = 5f; // How fast the object moves
    [SerializeField] private float frontDistance = 20f; // How far in front of the player the object should appear
    [SerializeField] private float returnSpeed = 3f; // How fast the object returns to original position

    private float previousRotation;
    private float currentRotationSpeed;
    private Vector3 originalPosition;
    private bool isMoving = false;
    private UnityEvent onMusicValueUp;
    private UnityEvent onMusicValueDown;
    private UnityEvent onTurnOnRelaxMusic;
    private UnityEvent onTurnOffRelaxMusic;
    private UnityEvent onCafeNoiseVolumeUp;
    private UnityEvent onCafeNoiseVolumeDown;
    private bool isScenarioStarted = false;
    public void Setup(UnityEvent onMusicValueUp, UnityEvent onMusicValueDown, UnityEvent onTurnOnRelaxMusic, UnityEvent onTurnOffRelaxMusic, UnityEvent onCafeNoiseVolumeUp, UnityEvent onCafeNoiseVolumeDown)
    {
        this.onMusicValueUp = onMusicValueUp;
        this.onMusicValueDown = onMusicValueDown;
        this.onTurnOnRelaxMusic = onTurnOnRelaxMusic;
        this.onTurnOffRelaxMusic = onTurnOffRelaxMusic;
        this.onCafeNoiseVolumeUp = onCafeNoiseVolumeUp;
        this.onCafeNoiseVolumeDown = onCafeNoiseVolumeDown;
    }

    public void StartLogic()
    {
        isScenarioStarted = true;
        spawnedObject.SetActive(false);
        onMusicValueUp?.Invoke();
        onTurnOffRelaxMusic?.Invoke();
        onCafeNoiseVolumeDown?.Invoke();
        spawnedObject.SetActive(true);
        previousRotation = camera.transform.eulerAngles.y;
        originalPosition = spawnedObject.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (!isScenarioStarted) return;
        // Calculate current rotation speed using camera rotation
        float currentRotation = camera.transform.eulerAngles.y;
        currentRotationSpeed = Mathf.Abs(currentRotation - previousRotation);
        previousRotation = currentRotation;

        // Check if rotation speed exceeds threshold
        if (currentRotationSpeed > rotationThreshold)
        {
            isMoving = true;
            // Calculate position in front of the camera
            Vector3 forwardPosition = camera.transform.position + camera.transform.forward * frontDistance;
            // Move the object to the new position
            spawnedObject.transform.position = Vector3.Lerp(spawnedObject.transform.position, forwardPosition, moveSpeed * Time.deltaTime);
            spawnedObject.transform.rotation = Quaternion.Euler(0, 180, 0);

        }
        else if (isMoving)
        {
            // Return to original position when not moving
            spawnedObject.transform.position = Vector3.Lerp(spawnedObject.transform.position, originalPosition, returnSpeed * Time.deltaTime);

            // Check if we're close enough to original position to stop moving
            if (Vector3.Distance(spawnedObject.transform.position, originalPosition) < 0.1f)
            {
                isMoving = false;
                spawnedObject.transform.position = originalPosition;
                spawnedObject.SetActive(false);
                onTurnOnRelaxMusic?.Invoke();
                onMusicValueDown?.Invoke();
                onCafeNoiseVolumeUp?.Invoke();
                isScenarioStarted = false;
            }
        }
    }
}
