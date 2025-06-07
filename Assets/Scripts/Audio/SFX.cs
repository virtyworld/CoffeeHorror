using UnityEngine;

public class SFX : MonoBehaviour
{
    [SerializeField] private AudioSource coffeeMachineSFX;
    [SerializeField] private AudioSource coffeeDreepSFX;
    [SerializeField] private AudioSource paperCollectSFX;
    [SerializeField] private AudioSource capAttachSFX;
    [SerializeField] private AudioSource coinsSFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

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

    public void StopCoffeeMachineSound()
    {
        if (coffeeMachineSFX != null)
        {
            coffeeMachineSFX.Stop();
        }
    }

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
    public void PlayCoinsSound()
    {
        if (coinsSFX != null)
        {
            coinsSFX.Play();
        }
    }
}
