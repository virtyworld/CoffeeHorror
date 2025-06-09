using UnityEngine;
using UnityEngine.Events;

public class BehindTheBack : MonoBehaviour
{
    [SerializeField] private GameObject spawnedObject; // The crab object
    [SerializeField] private Transform parentToSpawn; // The crab object
    [SerializeField] private Camera camera; // The camera to track rotation
    [SerializeField] private float rotationThreshold = 90f; // Threshold for total rotation in degrees
    [SerializeField] private float moveSpeed = 5f; // How fast the object moves
    [SerializeField] private float frontDistance = 3f; // How far in front of the player the object should appear
    [SerializeField] private float returnSpeed = 5f; // How fast the object returns to original position

    private float initialRotation;
    private float totalRotation;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isMoving = false;
    private UnityEvent onMusicValueUp;
    private UnityEvent onMusicValueDown;
    private UnityEvent onTurnOnRelaxMusic;
    private UnityEvent onTurnOffRelaxMusic;
    private UnityEvent onCafeNoiseVolumeUp;
    private UnityEvent onCafeNoiseVolumeDown;
    private bool isScenarioStarted = false;
    private GameObject newObject;
    public void Setup(UnityEvent onMusicValueUp, UnityEvent onMusicValueDown, UnityEvent onTurnOnRelaxMusic, UnityEvent onTurnOffRelaxMusic, UnityEvent onCafeNoiseVolumeUp, UnityEvent onCafeNoiseVolumeDown)
    {
        this.onMusicValueUp = onMusicValueUp;
        this.onMusicValueDown = onMusicValueDown;
        this.onTurnOnRelaxMusic = onTurnOnRelaxMusic;
        this.onTurnOffRelaxMusic = onTurnOffRelaxMusic;
        this.onCafeNoiseVolumeUp = onCafeNoiseVolumeUp;
        this.onCafeNoiseVolumeDown = onCafeNoiseVolumeDown;
    }
    void Start()
    {
        originalPosition = new Vector3(0, 0, 0);
        originalRotation = spawnedObject.transform.rotation;
    }
    public void StartLogic()
    {
        isScenarioStarted = true;
        onMusicValueUp?.Invoke();
        onTurnOffRelaxMusic?.Invoke();
        onCafeNoiseVolumeDown?.Invoke();
        spawnedObject.SetActive(true);
        initialRotation = camera.transform.eulerAngles.y;
        totalRotation = 0f;
        spawnedObject.transform.localPosition = originalPosition;
        Debug.Log("originalPosition " + originalPosition);
    }
    // Update is called once per frame
    void Update()
    {
        if (!isScenarioStarted) return;

        // Calculate total rotation from initial position
        float currentRotation = camera.transform.eulerAngles.y;
        float rotationDelta = Mathf.DeltaAngle(initialRotation, currentRotation);
        totalRotation = Mathf.Abs(rotationDelta);

        // Check if total rotation exceeds threshold
        if (totalRotation > rotationThreshold && !isMoving)
        {
            spawnedObject.SetActive(false);
            isMoving = true;

            // Calculate position in front of the camera
            Vector3 spawnPosition = camera.transform.position + camera.transform.forward * frontDistance;
            spawnPosition.y -= 1f;
            spawnPosition.x += 0.6f;
            // Spawn new object
            newObject = Instantiate(spawnedObject, spawnPosition, Quaternion.identity);
            newObject.transform.SetParent(parentToSpawn);

            // Make the new object face towards the original object
            Vector3 directionToCamera = (camera.transform.position - spawnPosition).normalized;
            newObject.transform.rotation = Quaternion.LookRotation(directionToCamera);
        }
        else if (isMoving)
        {
            spawnedObject.SetActive(false);
            // Return to original position when not moving
            newObject.SetActive(true);
            newObject.transform.position = Vector3.Lerp(newObject.transform.position, spawnedObject.transform.position, returnSpeed * Time.deltaTime);

            // Check if we're close enough to original position to stop moving
            if (Vector3.Distance(newObject.transform.position, spawnedObject.transform.position) < 0.1f)
            {
                isMoving = false;
                spawnedObject.transform.position = originalPosition;
                onTurnOnRelaxMusic?.Invoke();
                onMusicValueDown?.Invoke();
                onCafeNoiseVolumeUp?.Invoke();
                isScenarioStarted = false;
                Destroy(newObject);
            }
        }
    }
}
