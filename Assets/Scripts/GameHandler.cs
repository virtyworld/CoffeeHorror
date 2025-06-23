using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// GameHandler - Central Game Management System
/// 
/// Main Logic:
/// This script serves as the main coordinator for the entire coffee shop horror game.
/// It initializes and connects all major game systems through a UnityEvent-based architecture,
/// creating a decoupled communication system between different components. The script acts
/// as the central hub that manages the initialization sequence and event routing between
/// lighting, scenarios, player interactions, audio systems, and coffee machine mechanics.
/// 
/// Key Features:
/// - Centralized system initialization and coordination
/// - Event-driven architecture for loose coupling between systems
/// - Manages all major game components (lighting, scenarios, audio, player, coffee machine)
/// - Provides clean separation of concerns through event delegation
/// </summary>
public class GameHandler : MonoBehaviour
{
    [SerializeField] private LightSwitcher lightSwitcher;
    [SerializeField] private ScenaryHandler scenaryHandler;
    [SerializeField] private PlayerInteraction playerInteraction;
    [SerializeField] private Music music;
    [SerializeField] private SFX sfx;
    [SerializeField] private CoffeeMachine coffeeMachine;


    private UnityEvent onPlayerSwitchingLight;
    private UnityEvent onTurnOffLight;
    private UnityEvent onTurnAllLightsRed;
    private UnityEvent onTurnAllLightsWhite;
    private UnityEvent onTurnOnRelaxMusic;
    private UnityEvent onTurnOffRelaxMusic;
    private UnityEvent onMusicValueUp;
    private UnityEvent onMusicValueDown;
    private UnityEvent onCafeNoiseVolumeUp;
    private UnityEvent onCafeNoiseVolumeDown;
    private UnityEvent onPlayerTurnOnCoffeeMachine;

    /// <summary>
    /// Initializes all game systems and sets up event connections
    /// Called automatically when the game object awakens
    /// </summary>
    void Awake()
    {
        InitEvents();
        InitLightSwitcher();
        InitScenary();
        InitPlayerInteraction();
        InitMusic();
        InitSFX();
        InitCoffeeMachine();
    }

    /// <summary>
    /// Creates all UnityEvents used for inter-system communication
    /// These events enable decoupled communication between different game components
    /// </summary>
    private void InitEvents()
    {
        onPlayerSwitchingLight = new UnityEvent();
        onTurnOffLight = new UnityEvent();
        onTurnAllLightsRed = new UnityEvent();
        onTurnAllLightsWhite = new UnityEvent();
        onTurnOnRelaxMusic = new UnityEvent();
        onTurnOffRelaxMusic = new UnityEvent();
        onMusicValueUp = new UnityEvent();
        onMusicValueDown = new UnityEvent();
        onCafeNoiseVolumeUp = new UnityEvent();
        onCafeNoiseVolumeDown = new UnityEvent();
        onPlayerTurnOnCoffeeMachine = new UnityEvent();
    }

    /// <summary>
    /// Initializes the light switching system with lighting control events
    /// Connects light switcher to events for turning lights off, red, and white
    /// </summary>
    private void InitLightSwitcher()
    {
        lightSwitcher.Setup(onTurnOffLight, onTurnAllLightsRed, onTurnAllLightsWhite);
    }

    /// <summary>
    /// Initializes the scenario system with all required events for horror scenarios
    /// Connects scenario handler to audio, lighting, and player interaction events
    /// </summary>
    private void InitScenary()
    {
        scenaryHandler.Setup(onMusicValueUp, onMusicValueDown, onTurnOnRelaxMusic, onTurnOffRelaxMusic,
        onCafeNoiseVolumeUp, onCafeNoiseVolumeDown, onPlayerSwitchingLight, onTurnOffLight, onTurnAllLightsRed,
        onTurnAllLightsWhite, onPlayerTurnOnCoffeeMachine);
    }

    /// <summary>
    /// Initializes player interaction system with light switching event
    /// Connects player interactions to the light switching system
    /// </summary>
    private void InitPlayerInteraction()
    {
        playerInteraction.Setup(onPlayerSwitchingLight);
    }

    /// <summary>
    /// Initializes the music system with audio control events
    /// Connects music system to events for relaxing music and volume control
    /// </summary>
    private void InitMusic()
    {
        music.Setup(onTurnOnRelaxMusic, onTurnOffRelaxMusic, onMusicValueUp, onMusicValueDown);
    }

    /// <summary>
    /// Initializes the sound effects system with cafe noise control events
    /// Connects SFX system to events for cafe ambient noise volume control
    /// </summary>
    private void InitSFX()
    {
        sfx.Setup(onCafeNoiseVolumeUp, onCafeNoiseVolumeDown);
    }

    /// <summary>
    /// Initializes the coffee machine system with player interaction event
    /// Connects coffee machine to player turn-on events
    /// </summary>
    private void InitCoffeeMachine()
    {
        coffeeMachine.Setup(onPlayerTurnOnCoffeeMachine);
    }

}
