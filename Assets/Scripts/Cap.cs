using UnityEngine;

public class Cap : MonoBehaviour
{
    [SerializeField] private PlayerInteraction playerInteraction;

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

    void Start()
    {
        // Store initial position when attachment starts
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttaching && !isOnTheCup)
        {
            attachmentProgress += Time.deltaTime * attachSpeed;

            if (isMovingUp)
            {
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
                    if (cup != null && cup.SnapPoint != null)
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
