using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioSource mainBackground;
    [SerializeField] private AudioSource relaxBackground;
    [SerializeField] private float minVolume = 0.1f;
    [SerializeField] private float maxVolume = 1f;

    private int currentTrackIndex = -1;
    private float targetVolume = 1f;
    private float volumeChangeSpeed = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        mainBackground.volume = 0.1f;
        relaxBackground.volume = 0f;

        PlayMusicBackground();
    }
    private void PlayMusicBackground()
    {
        mainBackground.Play();
    }
    private void PlayRandomTrack()
    {

    }
}
