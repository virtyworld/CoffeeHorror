using UnityEngine;
using System.Collections;
using UnityEngine.Events;

/// <summary>
/// LightSwitcher - Dynamic Lighting Control System
/// 
/// Main Logic:
/// This script manages all lighting in the coffee shop, providing both manual control
/// and automatic random light flickering for horror atmosphere. It controls multiple
/// light sources simultaneously and can switch between normal white lighting and
/// atmospheric red lighting. The system includes visual feedback through switch
/// animations and audio feedback through sound effects.
/// 
/// Key Features:
/// - Manual light switching (on/off) with visual and audio feedback
/// - Automatic random light flickering for horror atmosphere
/// - Color control (white/red) for mood changes
/// - Coroutine-based timing system for random events
/// - Event-driven integration with other game systems
/// </summary>
public class LightSwitcher : MonoBehaviour
{
    [SerializeField] private Light[] lights;
    [SerializeField] private float minTimeInterval = 10f;
    [SerializeField] private float maxTimeInterval = 50f;
    [SerializeField] private int minLightsToSwitch = 1;
    [SerializeField] private int maxLightsToSwitch = 22;
    [SerializeField] private GameObject switcher;
    [SerializeField] private SFX sfx;

    private bool isItWorking = false;
    private bool isRandomSwitching = false;
    public bool IsTurnedOn { get => isItWorking; }

    /// <summary>
    /// Initializes the light switcher with event listeners for external control
    /// Connects to events for turning lights off, setting red color, and setting white color
    /// </summary>
    /// <param name="onTurnOffLight">Event triggered when lights should turn off</param>
    /// <param name="onTurnAllLightsRed">Event triggered when all lights should turn red</param>
    /// <param name="onTurnAllLightsWhite">Event triggered when all lights should turn white</param>
    public void Setup(UnityEvent onTurnOffLight, UnityEvent onTurnAllLightsRed, UnityEvent onTurnAllLightsWhite)
    {
        onTurnOffLight.AddListener(SwitchLightOff);
        onTurnAllLightsRed.AddListener(SetAllLightsRed);
        onTurnAllLightsWhite.AddListener(SetAllLightsWhite);
    }

    /// <summary>
    /// Initializes the lighting system and starts random light flickering
    /// Called automatically when the script starts
    /// </summary>
    void Start()
    {
        isItWorking = true;
        StartRandomSwitching();
    }

    /// <summary>
    /// Toggles the lighting system on/off with full functionality
    /// Includes visual animations, audio feedback, and random flickering control
    /// </summary>
    public void SwitchLight()
    {
        Debug.Log("SwitchLight " + isItWorking);

        if (isItWorking)
        {
            TurnAllLightsOff();
            LightSwitcherTurnOffAnimation();
            isItWorking = false;
            StopRandomSwitching();
            sfx.PlayLightSwitchOffSound();
        }
        else
        {
            TurnAllLightsOn();
            StartRandomSwitching();
            LightSwitcherTurnOnAnimation();
            isItWorking = true;
            sfx.PlayLightSwitchOnSound();
        }
    }

    /// <summary>
    /// Forces all lights to turn on with visual and audio feedback
    /// Activates the system and starts random flickering
    /// </summary>
    public void SwitchLightOn()
    {
        TurnAllLightsOn();
        LightSwitcherTurnOnAnimation();
        isItWorking = true;
        sfx.PlayLightSwitchOnSound();
    }

    /// <summary>
    /// Forces all lights to turn off with visual and audio feedback
    /// Deactivates the system and stops random flickering
    /// </summary>
    public void SwitchLightOff()
    {
        TurnAllLightsOff();
        LightSwitcherTurnOffAnimation();
        isItWorking = false;
        StopRandomSwitching();
        sfx.PlayLightSwitchOffSound();
    }

    /// <summary>
    /// Enables all light sources in the array
    /// </summary>
    public void TurnAllLightsOn()
    {
        foreach (Light light in lights)
        {
            light.enabled = true;
        }
    }

    /// <summary>
    /// Disables all light sources in the array
    /// </summary>
    public void TurnAllLightsOff()
    {
        foreach (Light light in lights)
        {
            light.enabled = false;
        }
    }

    /// <summary>
    /// Starts the automatic random light flickering system
    /// Activates the coroutine that randomly switches lights for horror atmosphere
    /// </summary>
    public void StartRandomSwitching()
    {
        isRandomSwitching = true;
        isItWorking = true;
        StartCoroutine(RandomLightSwitching());
    }

    /// <summary>
    /// Stops the automatic random light flickering system
    /// Deactivates the coroutine that randomly switches lights
    /// </summary>
    public void StopRandomSwitching()
    {
        isRandomSwitching = false;
    }

    /// <summary>
    /// Coroutine that handles automatic random light flickering
    /// Creates horror atmosphere by randomly turning lights on/off at intervals
    /// </summary>
    /// <returns>IEnumerator for coroutine execution</returns>
    private IEnumerator RandomLightSwitching()
    {
        while (isRandomSwitching)
        {
            if (!isItWorking) break;

            float delay = Random.Range(minTimeInterval, maxTimeInterval);
            yield return new WaitForSeconds(delay);

            if (!isItWorking) break;

            // Determine how many lights to switch
            int lightsToSwitch = Random.Range(minLightsToSwitch, lights.Length);

            // Create array of indices and shuffle it
            int[] indices = new int[lightsToSwitch];
            for (int i = 0; i < lightsToSwitch; i++)
            {
                indices[i] = Random.Range(minLightsToSwitch, lights.Length);
            }

            // Switch random lights
            for (int i = 0; i < lightsToSwitch; i++)
            {
                lights[indices[i]].enabled = !lights[indices[i]].enabled;
            }

            yield return new WaitForSeconds(0.1f);

            if (isItWorking)
            {
                TurnAllLightsOn();
            }
        }
    }

    /// <summary>
    /// Animates the light switch to the "on" position
    /// Rotates the switch visual to indicate the system is active
    /// </summary>
    private void LightSwitcherTurnOnAnimation()
    {
        Vector3 currentRotation = switcher.transform.rotation.eulerAngles;
        switcher.transform.rotation = Quaternion.Euler(0, currentRotation.y, currentRotation.z);
    }

    /// <summary>
    /// Animates the light switch to the "off" position
    /// Rotates the switch visual to indicate the system is inactive
    /// </summary>
    private void LightSwitcherTurnOffAnimation()
    {
        Vector3 currentRotation = switcher.transform.rotation.eulerAngles;
        switcher.transform.rotation = Quaternion.Euler(13, currentRotation.y, currentRotation.z);
    }

    /// <summary>
    /// Changes all lights to red color for horror atmosphere
    /// Creates a dramatic mood change in the environment
    /// </summary>
    private void SetAllLightsRed()
    {
        foreach (Light light in lights)
        {
            light.color = Color.red;
        }
    }

    /// <summary>
    /// Changes all lights back to white color for normal atmosphere
    /// Restores the default lighting environment
    /// </summary>
    private void SetAllLightsWhite()
    {//ECE2B0
        foreach (Light light in lights)
        {
            light.color = Color.white;
        }
    }
}
