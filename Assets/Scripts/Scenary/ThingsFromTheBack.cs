using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ThingsFromTheBack - Horror Scenario Controller
/// 
/// Main Logic:
/// This script manages a horror scenario where objects appear from behind the player
/// when they interact with the coffee machine. It creates jump-scare moments by
/// randomly spawning objects and playing audio effects. The scenario is triggered
/// by coffee machine activation and automatically cleans up after a delay.
/// 
/// Key Features:
/// - Random object spawning for unpredictability
/// - Audio effect integration for jump-scares
/// - Coffee machine interaction trigger
/// - Automatic cleanup after scenario completion
/// - One-time trigger mechanism per scenario cycle
/// </summary>
public class ThingsFromTheBack : MonoBehaviour
{
    [SerializeField] private GameObject objectFromTheBack;
    [SerializeField] private AudioSource audioSource;
    private bool isScenarioStarted = false;

    /// <summary>
    /// Initializes the scenario with coffee machine interaction event
    /// Connects to the coffee machine system to trigger the horror scenario
    /// </summary>
    /// <param name="onPlayerTurnOnCoffeeMachine">Event triggered when player turns on coffee machine</param>
    public void Setup(UnityEvent onPlayerTurnOnCoffeeMachine)
    {
        onPlayerTurnOnCoffeeMachine.AddListener(ActivateScenario);
    }

    /// <summary>
    /// Starts the horror scenario sequence
    /// Activates the scenario tracking for potential object spawning
    /// </summary>
    public void StartLogic()
    {
        Debug.Log("StartLogic");
        isScenarioStarted = true;
    }

    /// <summary>
    /// Activates the horror scenario when coffee machine is used
    /// Randomly decides whether to spawn the object for jump-scare effect
    /// </summary>
    private void ActivateScenario()
    {
        if (!isScenarioStarted) return;

        int random = Random.Range(0, 4);

        if (random == 0)
        {
            objectFromTheBack.SetActive(true);
            StartCoroutine(ReplaceBackAfterDelay());
        }
        else
        {
            objectFromTheBack.SetActive(false);
        }
    }

    /// <summary>
    /// Coroutine that handles the scenario cleanup after object spawning
    /// Plays audio effect and removes the object after a delay
    /// </summary>
    /// <returns>IEnumerator for coroutine execution</returns>
    private IEnumerator ReplaceBackAfterDelay()
    {
        audioSource.Play();
        yield return new WaitForSeconds(3f);
        isScenarioStarted = false;
        objectFromTheBack.SetActive(false);
    }
}
