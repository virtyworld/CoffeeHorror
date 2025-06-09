using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ScenaryHandler : MonoBehaviour
{
    [SerializeField] private GuestReplacer guestReplacers;
    [SerializeField] private BehindTheBack behindTheBack;
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

    public void Setup(UnityEvent onMusicValueUp, UnityEvent onMusicValueDown, UnityEvent onTurnOnRelaxMusic,
     UnityEvent onTurnOffRelaxMusic, UnityEvent onCafeNoiseVolumeUp, UnityEvent onCafeNoiseVolumeDown,
     UnityEvent onPlayerSwitchingLight, UnityEvent onTurnOffLight, UnityEvent onTurnAllLightsRed, UnityEvent onTurnAllLightsWhite)
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
    }
    void Start()
    {
        scenarioTimer = timeBetweenScenarios;
        behindTheBack.Setup(onMusicValueUp, onMusicValueDown, onTurnOnRelaxMusic, onTurnOffRelaxMusic, onCafeNoiseVolumeUp, onCafeNoiseVolumeDown);
        guestReplacers.Setup(onPlayerSwitchingLight, onTurnOffLight, onTurnAllLightsRed, onTurnAllLightsWhite,
        onMusicValueUp, onMusicValueDown, onTurnOnRelaxMusic, onTurnOffRelaxMusic, onCafeNoiseVolumeUp, onCafeNoiseVolumeDown);

    }

    // Update is called once per frame
    void Update()
    {
        scenarioTimer -= Time.deltaTime;

        if (scenarioTimer <= 0f)
        {
            TriggerNextScenario();
            scenarioTimer = timeBetweenScenarios;
        }
    }

    private void TriggerNextScenario()
    {
        int randomScenario = Random.Range(0, 2); // 0 or 1

        if (randomScenario == 0)
        {
            Debug.Log("StartLogic GuestReplacer");
            guestReplacers.StartLogic();
        }
        else
        {
            // Reset and activate the behind the back scenario
            Debug.Log("StartLogic BehindTheBack");
            behindTheBack.StartLogic();
        }
    }

}
