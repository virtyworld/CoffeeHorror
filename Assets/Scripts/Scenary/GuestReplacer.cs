using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GuestReplacer : MonoBehaviour
{
    [SerializeField] private GameObject[] normalObjects;
    [SerializeField] private GameObject[] scaryObjects;
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
    void Start()
    {
        ReplaceGuestsBack();
    }

    public void StartLogic()
    {
        isSceneStarted = true;
        onTurnOffLight.Invoke();
    }
    public void ActivateScenario()
    {
        if (!isSceneStarted) return;
        Debug.Log("StartLogic GuestReplacer");
        onTurnAllLightsRed.Invoke();
        onMusicValueUp.Invoke();
        onCafeNoiseVolumeDown.Invoke();
        onTurnOffRelaxMusic.Invoke();
        ReplaceGuests();
        if (replaceBackCoroutine != null)
        {
            StopCoroutine(replaceBackCoroutine);
        }

        // Start new coroutine
        replaceBackCoroutine = StartCoroutine(ReplaceBackAfterDelay());
    }

    private IEnumerator ReplaceBackAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        onTurnAllLightsWhite.Invoke();
        onMusicValueDown.Invoke();
        onCafeNoiseVolumeUp.Invoke();
        onTurnOnRelaxMusic.Invoke();
        ReplaceGuestsBack();
    }

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
