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

    public void Setup(UnityEvent onPlayerSwitchingLight, UnityEvent onTurnOffLight, UnityEvent onTurnAllLightsRed, UnityEvent onTurnAllLightsWhite)
    {
        onPlayerSwitchingLight.AddListener(ActivateScenario);
        this.onTurnOffLight = onTurnOffLight;
        this.onTurnAllLightsRed = onTurnAllLightsRed;
        this.onTurnAllLightsWhite = onTurnAllLightsWhite;
    }
    void Start()
    {
        ReplaceGuestsBack();
    }

    public void StartLogic()
    {
        onTurnOffLight.Invoke();
    }
    public void ActivateScenario()
    {
        Debug.Log("StartLogic GuestReplacer");
        onTurnAllLightsRed.Invoke();
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
