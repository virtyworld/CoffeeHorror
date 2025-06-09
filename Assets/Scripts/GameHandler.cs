using UnityEngine;
using UnityEngine.Events;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private LightSwitcher lightSwitcher;
    [SerializeField] private ScenaryHandler scenaryHandler;
    [SerializeField] private PlayerInteraction playerInteraction;
    [SerializeField] private Music music;
    [SerializeField] private SFX sfx;


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

    void Awake()
    {
        InitEvents();
        InitLightSwitcher();
        InitScenary();
        InitPlayerInteraction();
        InitMusic();
        InitSFX();
    }

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
    }

    private void InitLightSwitcher()
    {
        lightSwitcher.Setup(onTurnOffLight, onTurnAllLightsRed, onTurnAllLightsWhite);
    }

    private void InitScenary()
    {
        scenaryHandler.Setup(onMusicValueUp, onMusicValueDown, onTurnOnRelaxMusic, onTurnOffRelaxMusic,
        onCafeNoiseVolumeUp, onCafeNoiseVolumeDown, onPlayerSwitchingLight, onTurnOffLight, onTurnAllLightsRed,
        onTurnAllLightsWhite);
    }

    private void InitPlayerInteraction()
    {
        playerInteraction.Setup(onPlayerSwitchingLight);
    }

    private void InitMusic()
    {
        music.Setup(onTurnOnRelaxMusic, onTurnOffRelaxMusic, onMusicValueUp, onMusicValueDown);
    }

    private void InitSFX()
    {
        sfx.Setup(onCafeNoiseVolumeUp, onCafeNoiseVolumeDown);
    }

}
