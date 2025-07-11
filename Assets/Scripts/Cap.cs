using UnityEngine;

/// <summary>
/// Cap - Coffee Cup Cap Attachment System
/// 
/// Main Logic:
/// This script manages the automatic attachment of caps to coffee cups. It detects
/// when a cap is near a completed coffee cup and automatically attaches it with
/// smooth animations. The attachment process includes a two-phase animation:
/// first moving up, then attaching to the cup's snap point with proper rotation.
/// 
/// Key Features:
/// - Automatic cap detection and attachment
/// - Two-phase attachment animation (up then attach)
/// - Physics-based proximity detection
/// - Smooth position and rotation interpolation
/// - Automatic physics component disabling after attachment
/// - Integration with player interaction system
/// </summary>
public class Cap : MonoBehaviour
{
    [SerializeField] private PlayerInteraction playerInteraction;
    [SerializeField] private SFX sfx;

    private PaperCup targetCup;
    private bool isAttaching = false;
    private float attachSpeed = 5f;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    public float overlapRadius = 0.05f;
    private Vector3 initialPosition;
    private float attachmentProgress = 0f;
    private bool isMovingUp = true;
    public bool isOnTheCup = false;

    /// <summary>
    /// Initializes the cap system
    /// Stores the initial position for attachment animations
    /// </summary>
    void Start()
    {
        // Store initial position when attachment starts
        initialPosition = transform.position;
    }

    /// <summary>
    /// Main update loop that handles cap attachment logic
    /// Manages the two-phase attachment animation and proximity detection
    /// </summary>
    void Update()
    {
        if (isAttaching && !isOnTheCup)
        {
            attachmentProgress += Time.deltaTime * attachSpeed;

            if (isMovingUp)
            {
                sfx.PlayPaperPlaceSound();
                // First move up
                Vector3 raisedPosition = initialPosition + Vector3.up * 0.1f;
                transform.position = Vector3.Lerp(initialPosition, raisedPosition, attachmentProgress);

                if (attachmentProgress >= 1f)
                {
                    isMovingUp = false;
                    attachmentProgress = 0f;
                }
            }
            else
            {

                // Then move to final position
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * attachSpeed);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * attachSpeed);

                // Check if we're close enough to the target
                if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
                {
                    // Finalize attachment
                    transform.position = targetPosition;
                    transform.rotation = targetRotation;
                    isAttaching = false;

                    // Reset local position to zero
                    transform.localPosition = Vector3.zero;

                    // Disable collider and rigidbody to prevent further interaction
                    if (GetComponent<Collider>() != null)
                        GetComponent<Collider>().enabled = false;
                    if (GetComponent<Rigidbody>() != null)
                        GetComponent<Rigidbody>().isKinematic = true;

                    isOnTheCup = true;
                }
            }
        }
        else
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, overlapRadius);

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Glass"))
                {
                    PaperCup cup = collider.GetComponent<PaperCup>();
                    if (cup != null && cup.SnapPoint != null && cup.IsCoffeeDone)
                    {
                        targetCup = cup;
                        targetPosition = cup.SnapPoint.position;
                        targetRotation = Quaternion.Euler(cup.CapRotatedPosition);
                        isAttaching = true;
                        initialPosition = transform.position;
                        attachmentProgress = 0f;
                        isMovingUp = true;
                        transform.SetParent(cup.SnapPoint);
                        playerInteraction.HeldObject = null;
                        playerInteraction.IsHoldingObject = false;
                        // Disable interaction immediately when attachment starts
                        if (GetComponent<Collider>() != null)
                            GetComponent<Collider>().enabled = false;
                        if (GetComponent<Rigidbody>() != null)
                            GetComponent<Rigidbody>().isKinematic = true;
                        break;
                    }
                }
            }
        }
    }
}
