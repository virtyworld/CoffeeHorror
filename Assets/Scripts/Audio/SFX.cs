using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// SFX (Sound Effects) Manager
/// 
/// Main Logic:
/// This script manages all sound effects in the game. It provides methods to play and stop
/// various audio clips for different game events like coffee machine sounds, coin collection,
/// light switches, and ambient cafe noise. It also handles volume control for cafe background noise
/// through UnityEvents that can be triggered by other game systems.
/// 
/// Key Features:
/// - Centralized sound effect management
/// - Volume control for ambient cafe noise
/// - Event-driven audio system integration
/// - Null safety checks for all audio sources
/// </summary>
public class SFX : MonoBehaviour
{
    [SerializeField] private AudioSource coffeeMachineSFX;
    [SerializeField] private AudioSource coffeeDreepSFX;
    [SerializeField] private AudioSource paperCollectSFX;
    [SerializeField] private AudioSource capAttachSFX;
    [SerializeField] private AudioSource coinsSFX;
    [SerializeField] private AudioSource lightSwitchOnSFX;
    [SerializeField] private AudioSource lightSwitchOffSFX;
    [SerializeField] private AudioSource cafeNoiseSFX;

    /// <summary>
    /// Initializes the SFX system by subscribing to cafe noise volume control events
    /// </summary>
    /// <param name="onCafeNoiseVolumeUp">Event triggered when cafe noise volume should increase</param>
    /// <param name="onCafeNoiseVolumeDown">Event triggered when cafe noise volume should decrease</param>
    public void Setup(UnityEvent onCafeNoiseVolumeUp, UnityEvent onCafeNoiseVolumeDown)
    {
        Debug.Log("Setup SFX");
        Debug.Log(onCafeNoiseVolumeDown);
        onCafeNoiseVolumeUp.AddListener(CafeNoiseVolumeUp);
        onCafeNoiseVolumeDown.AddListener(CafeNoiseVolumeDown);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Plays the coffee machine sound effect
    /// </summary>
    public void PlayCoffeeMachineSound()
    {
        if (coffeeMachineSFX != null)
        {
            coffeeMachineSFX.Play();
        }
        else
        {
            Debug.LogWarning("Coffee machine sound effect AudioSource is not assigned!");
        }
    }

    /// <summary>
    /// Stops the coffee machine sound effect
    /// </summary>
    public void StopCoffeeMachineSound()
    {
        if (coffeeMachineSFX != null)
        {
            coffeeMachineSFX.Stop();
        }
    }

    /// <summary>
    /// Plays the coffee drip sound effect with increased pitch (2x speed)
    /// </summary>
    public void PlayCoffeeDreepSound()
    {
        if (coffeeDreepSFX != null)
        {
            coffeeDreepSFX.pitch = 2.0f; // Set pitch to 2.0 to play twice as fast
            coffeeDreepSFX.Play();
        }
        else
        {
            Debug.LogWarning("Coffee dreep sound effect AudioSource is not assigned!");
        }
    }

    /// <summary>
    /// Plays the paper collection sound effect
    /// </summary>
    public void PlayPaperCollectSound()
    {
        if (paperCollectSFX != null)
        {
            paperCollectSFX.Play();
        }
        else
        {
            Debug.LogWarning("Coffee dreep sound effect AudioSource is not assigned!");
        }
    }

    /// <summary>
    /// Plays the paper placement/cap attachment sound effect
    /// </summary>
    public void PlayPaperPlaceSound()
    {
        if (capAttachSFX != null)
        {
            capAttachSFX.Play();
        }
        else
        {
            Debug.LogWarning("Paper place sound effect AudioSource is not assigned!");
        }
    }

    /// <summary>
    /// Plays the coin collection sound effect
    /// </summary>
    public void PlayCoinsSound()
    {
        if (coinsSFX != null)
        {
            coinsSFX.Play();
        }
    }

    /// <summary>
    /// Plays the light switch turn on sound effect
    /// </summary>
    public void PlayLightSwitchOnSound()
    {
        if (lightSwitchOnSFX != null)
        {
            lightSwitchOnSFX.Play();
        }
    }

    /// <summary>
    /// Plays the light switch turn off sound effect
    /// </summary>
    public void PlayLightSwitchOffSound()
    {
        if (lightSwitchOffSFX != null)
        {
            lightSwitchOffSFX.Play();
        }
    }

    /// <summary>
    /// Increases cafe background noise volume to 0.2
    /// </summary>
    private void CafeNoiseVolumeUp()
    {
        Debug.Log("CafeNoiseVolumeUp");
        cafeNoiseSFX.volume = 0.2f;
    }

    /// <summary>
    /// Mutes cafe background noise volume to 0
    /// </summary>
    private void CafeNoiseVolumeDown()
    {
        Debug.Log("CafeNoiseVolumeDown");
        cafeNoiseSFX.volume = 0f;
    }
}
