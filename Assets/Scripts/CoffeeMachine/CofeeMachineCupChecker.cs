using UnityEngine;

/// <summary>
/// CofeeMachineCupChecker - Coffee Machine Cup Detection System
/// 
/// Main Logic:
/// This script detects when a coffee cup is placed in the coffee machine's
/// brewing area. It uses trigger colliders to monitor cup placement and
/// provides status information to the coffee machine system. The script
/// tracks both the presence of a cup and references to the specific cup
/// object for coffee brewing coordination.
/// 
/// Key Features:
/// - Trigger-based cup detection
/// - Real-time cup placement monitoring
/// - Integration with coffee machine system
/// - PaperCup component reference management
/// - Status tracking for brewing coordination
/// </summary>
public class CofeeMachineCupChecker : MonoBehaviour
{

    public bool isCupInPlace = false;

    public PaperCup paperCup;

    /// <summary>
    /// Initializes the cup checker system
    /// Placeholder for future initialization logic
    /// </summary>
    void Start()
    {

    }

    /// <summary>
    /// Detects when a cup enters the coffee machine trigger area
    /// Sets the cup as in place and stores reference to the PaperCup component
    /// </summary>
    /// <param name="other">The collider that entered the trigger</param>
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        if (other.gameObject.CompareTag("Glass"))
        {
            isCupInPlace = true;
            paperCup = other.gameObject.GetComponent<PaperCup>();
        }
    }

    /// <summary>
    /// Maintains cup detection while cup remains in trigger area
    /// Ensures continuous detection during brewing process
    /// </summary>
    /// <param name="other">The collider staying in the trigger</param>
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Glass"))
        {
            isCupInPlace = true;
        }
    }

    /// <summary>
    /// Detects when a cup leaves the coffee machine trigger area
    /// Clears the cup reference and marks as not in place
    /// </summary>
    /// <param name="other">The collider that exited the trigger</param>
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Glass"))
        {
            isCupInPlace = false;
            paperCup = null;
        }
    }

}
