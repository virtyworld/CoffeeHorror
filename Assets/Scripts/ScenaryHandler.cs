/*
 * ScenaryHandler - Main scenario management system for the coffee shop horror game
 * 
 * Main Logic:
 * - Manages different horror scenarios that trigger randomly at set intervals
 * - Coordinates between different scenario components (GuestReplacer, BehindTheBack, ThingsFromTheBack)
 * - Handles audio and lighting events through UnityEvents
 * - Controls the timing and sequencing of horror events
 * - Acts as a central hub for scenario coordination
 * 
 * Key Features:
 * - Timer-based scenario triggering system
 * - Random scenario selection for unpredictability
 * - Event-driven coordination with audio and lighting systems
 * - Centralized management of all horror scenario components
 * - Automatic scenario cycling at configurable intervals
 */

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ScenaryHandler : MonoBehaviour
{
    [SerializeField] private GuestReplacer guestReplacers;
    [SerializeField] private BehindTheBack behindTheBack;
    [SerializeField] private ThingsFromTheBack thingsFromTheBack;
    [SerializeField] private float timeBetweenScenarios = 60f; // Time in seconds between scenario changes

    private float scenarioTimer;
    private UnityEvent onMusicValueUp;
    private UnityEvent onMusicValueDown;
    private UnityEvent onTurnOnRelaxMusic;
    private UnityEvent onTurnOffRelaxMusic;
    private UnityEvent onCafeNoiseVolumeUp;
    private UnityEvent onCafeNoiseVolumeDown;
    private UnityEvent onPlayerSwitchingLight;
    private UnityEvent onTurnOffLight;
    private UnityEvent onTurnAllLightsRed;
    private UnityEvent onTurnAllLightsWhite;
    private UnityEvent onPlayerTurnOnCoffeeMachine;

    /// <summary>
    /// Initializes the scenario handler with all required UnityEvents for audio and lighting control
    /// Sets up the event system that allows scenarios to communicate with other game systems
    /// </summary>
    /// <param name="onMusicValueUp">Event triggered when music volume should increase</param>
    /// <param name="onMusicValueDown">Event triggered when music volume should decrease</param>
    /// <param name="onTurnOnRelaxMusic">Event triggered when relaxing music should start</param>
    /// <param name="onTurnOffRelaxMusic">Event triggered when relaxing music should stop</param>
    /// <param name="onCafeNoiseVolumeUp">Event triggered when cafe noise should increase</param>
    /// <param name="onCafeNoiseVolumeDown">Event triggered when cafe noise should decrease</param>
    /// <param name="onPlayerSwitchingLight">Event triggered when player switches lights</param>
    /// <param name="onTurnOffLight">Event triggered when lights should turn off</param>
    /// <param name="onTurnAllLightsRed">Event triggered when all lights should turn red</param>
    /// <param name="onTurnAllLightsWhite">Event triggered when all lights should turn white</param>
    /// <param name="onPlayerTurnOnCoffeeMachine">Event triggered when player turns on coffee machine</param>
    public void Setup(UnityEvent onMusicValueUp, UnityEvent onMusicValueDown, UnityEvent onTurnOnRelaxMusic,
     UnityEvent onTurnOffRelaxMusic, UnityEvent onCafeNoiseVolumeUp, UnityEvent onCafeNoiseVolumeDown,
     UnityEvent onPlayerSwitchingLight, UnityEvent onTurnOffLight, UnityEvent onTurnAllLightsRed, UnityEvent onTurnAllLightsWhite,
     UnityEvent onPlayerTurnOnCoffeeMachine)
    {
        this.onMusicValueUp = onMusicValueUp;
        this.onMusicValueDown = onMusicValueDown;
        this.onTurnOnRelaxMusic = onTurnOnRelaxMusic;
        this.onTurnOffRelaxMusic = onTurnOffRelaxMusic;
        this.onCafeNoiseVolumeUp = onCafeNoiseVolumeUp;
        this.onCafeNoiseVolumeDown = onCafeNoiseVolumeDown;
        this.onPlayerSwitchingLight = onPlayerSwitchingLight;
        this.onTurnOffLight = onTurnOffLight;
        this.onTurnAllLightsRed = onTurnAllLightsRed;
        this.onTurnAllLightsWhite = onTurnAllLightsWhite;
        this.onPlayerTurnOnCoffeeMachine = onPlayerTurnOnCoffeeMachine;
    }

    /// <summary>
    /// Initializes the scenario system and sets up all scenario components
    /// Configures each scenario component with the appropriate events and starts the timer
    /// </summary>
    void Start()
    {
        scenarioTimer = timeBetweenScenarios;
        behindTheBack.Setup(onMusicValueUp, onMusicValueDown, onTurnOnRelaxMusic, onTurnOffRelaxMusic, onCafeNoiseVolumeUp, onCafeNoiseVolumeDown);
        guestReplacers.Setup(onPlayerSwitchingLight, onTurnOffLight, onTurnAllLightsRed, onTurnAllLightsWhite,
        onMusicValueUp, onMusicValueDown, onTurnOnRelaxMusic, onTurnOffRelaxMusic, onCafeNoiseVolumeUp, onCafeNoiseVolumeDown);
        thingsFromTheBack.Setup(onPlayerTurnOnCoffeeMachine);

    }

    /// <summary>
    /// Main update loop that manages scenario timing and triggers new scenarios
    /// Counts down the timer and automatically triggers the next scenario when it reaches zero
    /// </summary>
    void Update()
    {
        scenarioTimer -= Time.deltaTime;

        if (scenarioTimer <= 0f)
        {
            TriggerNextScenario();
            scenarioTimer = timeBetweenScenarios;
        }
    }

    /// <summary>
    /// Randomly selects and triggers the next horror scenario
    /// Currently alternates between guest replacement and things from the back scenarios
    /// Creates unpredictability in the horror experience
    /// </summary>
    private void TriggerNextScenario()
    {
        int randomScenario = Random.Range(0, 2); // 0 or 1

        if (randomScenario == 0)
        {
            guestReplacers.StartLogic();
        }
        else
        {
            thingsFromTheBack.StartLogic();
            // behindTheBack.StartLogic();
        }
    }

}
