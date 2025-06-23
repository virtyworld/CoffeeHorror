/*
 * Music - Audio management system for the coffee shop horror game
 * 
 * Main Logic:
 * - Manages two audio sources: main background music and relaxing background music
 * - Handles volume transitions between different audio states
 * - Responds to UnityEvents to change music volume and switch between audio tracks
 * - Creates atmospheric audio changes for horror scenarios
 * - Controls the overall audio experience through volume manipulation
 * 
 * Key Features:
 * - Dual audio source management (main + relaxing)
 * - Event-driven volume control
 * - Atmospheric transitions for horror scenarios
 * - Configurable volume ranges
 * - Automatic background music playback
 */

using UnityEngine;
using UnityEngine.Events;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioSource mainBackground;
    [SerializeField] private AudioSource relaxBackground;
    [SerializeField] private float minVolume = 0.1f;
    [SerializeField] private float maxVolume = 1f;

    private int currentTrackIndex = -1;
    private float targetVolume = 1f;
    private float volumeChangeSpeed = 0.5f;

    /// <summary>
    /// Sets up event listeners for music control events
    /// Connects to the event system for external music control
    /// </summary>
    /// <param name="onTurnOnRelaxMusic">Event to start relaxing music</param>
    /// <param name="onTurnOffRelaxMusic">Event to stop relaxing music</param>
    /// <param name="onMusicValueUp">Event to increase main music volume</param>
    /// <param name="onMusicValueDown">Event to decrease main music volume</param>
    public void Setup(UnityEvent onTurnOnRelaxMusic, UnityEvent onTurnOffRelaxMusic, UnityEvent onMusicValueUp, UnityEvent onMusicValueDown)
    {
        onTurnOnRelaxMusic.AddListener(TurnOnRelaxMusic);
        onTurnOffRelaxMusic.AddListener(TurnOffRelaxMusic);
        onMusicValueUp.AddListener(MusicValueUp);
        onMusicValueDown.AddListener(MusicValueDown);
    }

    /// <summary>
    /// Initializes audio sources with default volumes and starts background music
    /// Sets up the initial audio state for the game
    /// </summary>
    void Start()
    {

        mainBackground.volume = 0.1f;
        relaxBackground.volume = 0f;

        PlayMusicBackground();
        TurnOnRelaxMusic();
    }

    /// <summary>
    /// Starts playing the main background music track
    /// Initiates the primary audio experience for the game
    /// </summary>
    private void PlayMusicBackground()
    {
        mainBackground.Play();
    }

    /// <summary>
    /// Activates the relaxing background music by setting its volume to a low level
    /// Creates a calm atmosphere for normal gameplay
    /// </summary>
    private void TurnOnRelaxMusic()
    {
        Debug.Log("TurnOnRelaxMusic");
        relaxBackground.volume = 0.1f;
    }

    /// <summary>
    /// Deactivates the relaxing background music by setting its volume to zero
    /// Removes calm atmosphere, often used during horror scenarios
    /// </summary>
    private void TurnOffRelaxMusic()
    {
        Debug.Log("TurnOffRelaxMusic");
        relaxBackground.volume = 0f;
    }

    /// <summary>
    /// Increases the main background music volume to create tension
    /// Used during horror scenarios to build atmosphere
    /// </summary>
    private void MusicValueUp()
    {
        mainBackground.volume = 0.5f;
    }

    /// <summary>
    /// Decreases the main background music volume to create calm atmosphere
    /// Returns to normal volume levels after horror scenarios
    /// </summary>
    private void MusicValueDown()
    {
        mainBackground.volume = 0.1f;
    }
}
