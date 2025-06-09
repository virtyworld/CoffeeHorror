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

    public void Setup(UnityEvent onTurnOnRelaxMusic, UnityEvent onTurnOffRelaxMusic, UnityEvent onMusicValueUp, UnityEvent onMusicValueDown)
    {
        onTurnOnRelaxMusic.AddListener(TurnOnRelaxMusic);
        onTurnOffRelaxMusic.AddListener(TurnOffRelaxMusic);
        onMusicValueUp.AddListener(MusicValueUp);
        onMusicValueDown.AddListener(MusicValueDown);
    }

    void Start()
    {

        mainBackground.volume = 0.1f;
        relaxBackground.volume = 0f;

        PlayMusicBackground();
        TurnOnRelaxMusic();
    }
    private void PlayMusicBackground()
    {
        mainBackground.Play();
    }
    private void TurnOnRelaxMusic()
    {
        Debug.Log("TurnOnRelaxMusic");
        relaxBackground.volume = 0.1f;
    }
    private void TurnOffRelaxMusic()
    {
        Debug.Log("TurnOffRelaxMusic");
        relaxBackground.volume = 0f;
    }
    private void MusicValueUp()
    {
        mainBackground.volume = 0.5f;
    }
    private void MusicValueDown()
    {
        mainBackground.volume = 0.1f;
    }
}
