using UnityEngine;
using System.Collections;
using UnityEngine.Events;

/// <summary>
/// Guest Replacer - Horror Scenario Controller
/// 
/// Main Logic:
/// This script manages a horror scenario where normal cafe guests are replaced with scary objects
/// when the player interacts with light switches. It creates a dynamic atmosphere change by
/// coordinating multiple systems: object replacement, lighting changes, music transitions,
/// and ambient sound adjustments. The scenario can only be triggered once per session and
/// automatically reverts back to normal after a delay.
/// 
/// Key Features:
/// - Object swapping between normal and scary versions
/// - Event-driven coordination with lighting, music, and audio systems
/// - One-time trigger mechanism with automatic reversion
/// - Coroutine-based timing control
/// - State management for scenario progression
/// </summary>
public class GuestReplacer : MonoBehaviour
{
    [SerializeField] private GameObject[] normalObjects;
    [SerializeField] private GameObject[] scaryObjects;
    [SerializeField] private AudioSource audioSource;
    private Coroutine replaceBackCoroutine;
    private UnityEvent onTurnOffLight;
    private UnityEvent onTurnAllLightsRed;
    private UnityEvent onTurnAllLightsWhite;
    private UnityEvent onMusicValueUp;
    private UnityEvent onMusicValueDown;
    private UnityEvent onTurnOnRelaxMusic;
    private UnityEvent onTurnOffRelaxMusic;
    private UnityEvent onCafeNoiseVolumeUp;
    private UnityEvent onCafeNoiseVolumeDown;
    private bool isSceneStarted = false;
    private bool isAlreadyDone = false;

    /// <summary>
    /// Initializes the GuestReplacer by subscribing to various game events for scenario coordination
    /// </summary>
    /// <param name="onPlayerSwitchingLight">Event triggered when player switches lights</param>
    /// <param name="onTurnOffLight">Event to turn off lights</param>
    /// <param name="onTurnAllLightsRed">Event to turn all lights red</param>
    /// <param name="onTurnAllLightsWhite">Event to turn all lights white</param>
    /// <param name="onMusicValueUp">Event to increase music volume</param>
    /// <param name="onMusicValueDown">Event to decrease music volume</param>
    /// <param name="onTurnOnRelaxMusic">Event to turn on relaxing music</param>
    /// <param name="onTurnOffRelaxMusic">Event to turn off relaxing music</param>
    /// <param name="onCafeNoiseVolumeUp">Event to increase cafe noise</param>
    /// <param name="onCafeNoiseVolumeDown">Event to decrease cafe noise</param>
    public void Setup(UnityEvent onPlayerSwitchingLight, UnityEvent onTurnOffLight, UnityEvent onTurnAllLightsRed,
    UnityEvent onTurnAllLightsWhite, UnityEvent onMusicValueUp, UnityEvent onMusicValueDown, UnityEvent onTurnOnRelaxMusic,
     UnityEvent onTurnOffRelaxMusic, UnityEvent onCafeNoiseVolumeUp, UnityEvent onCafeNoiseVolumeDown)
    {
        onPlayerSwitchingLight.AddListener(ActivateScenario);
        this.onTurnOffLight = onTurnOffLight;
        this.onTurnAllLightsRed = onTurnAllLightsRed;
        this.onTurnAllLightsWhite = onTurnAllLightsWhite;
        this.onMusicValueUp = onMusicValueUp;
        this.onMusicValueDown = onMusicValueDown;
        this.onTurnOnRelaxMusic = onTurnOnRelaxMusic;
        this.onTurnOffRelaxMusic = onTurnOffRelaxMusic;
        this.onCafeNoiseVolumeUp = onCafeNoiseVolumeUp;
        this.onCafeNoiseVolumeDown = onCafeNoiseVolumeDown;
    }

    /// <summary>
    /// Initializes the scene by replacing guests back to normal state
    /// </summary>
    void Start()
    {
        ReplaceGuestsBack();
    }

    /// <summary>
    /// Activates the scenario logic and turns off lights to prepare for horror sequence
    /// </summary>
    public void StartLogic()
    {
        isSceneStarted = true;
        onTurnOffLight.Invoke();
        isAlreadyDone = false;
    }

    /// <summary>
    /// Activates the horror scenario when player switches lights, creating atmosphere change
    /// </summary>
    public void ActivateScenario()
    {
        if (!isSceneStarted) return;
        if (isAlreadyDone) return;
        Debug.Log("StartLogic GuestReplacer");
        onTurnAllLightsRed.Invoke();
        onMusicValueUp.Invoke();
        onCafeNoiseVolumeDown.Invoke();
        onTurnOffRelaxMusic.Invoke();
        ReplaceGuests();
        audioSource.Play();
        if (replaceBackCoroutine != null)
        {
            StopCoroutine(replaceBackCoroutine);
        }

        // Start new coroutine
        replaceBackCoroutine = StartCoroutine(ReplaceBackAfterDelay());
    }

    /// <summary>
    /// Coroutine that waits 3 seconds then reverts the scenario back to normal state
    /// </summary>
    private IEnumerator ReplaceBackAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        onTurnAllLightsWhite.Invoke();
        onMusicValueDown.Invoke();
        onCafeNoiseVolumeUp.Invoke();
        onTurnOnRelaxMusic.Invoke();
        isAlreadyDone = true;
        ReplaceGuestsBack();
    }

    /// <summary>
    /// Replaces normal objects with scary objects to create horror atmosphere
    /// </summary>
    public void ReplaceGuests()
    {
        // Disable objects from first array
        foreach (GameObject obj in normalObjects)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }

        // Enable objects from second array
        foreach (GameObject obj in scaryObjects)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Replaces scary objects back to normal objects to restore peaceful atmosphere
    /// </summary>
    public void ReplaceGuestsBack()
    {
        foreach (GameObject obj in normalObjects)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }

        // Enable objects from second array
        foreach (GameObject obj in scaryObjects)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
}
