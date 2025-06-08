using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class LightSwitcher : MonoBehaviour
{
    [SerializeField] private Light[] lights;
    [SerializeField] private float minTimeInterval = 10f;
    [SerializeField] private float maxTimeInterval = 50f;
    [SerializeField] private int minLightsToSwitch = 1;
    [SerializeField] private int maxLightsToSwitch = 22;
    [SerializeField] private GameObject switcher;
    [SerializeField] private SFX sfx;

    private bool isItWorking = false;
    private bool isRandomSwitching = false;
    public bool IsTurnedOn { get => isItWorking; }

    public void Setup(UnityEvent onTurnOffLight, UnityEvent onTurnAllLightsRed, UnityEvent onTurnAllLightsWhite)
    {
        onTurnOffLight.AddListener(SwitchLightOff);
        onTurnAllLightsRed.AddListener(SetAllLightsRed);
        onTurnAllLightsWhite.AddListener(SetAllLightsWhite);
    }
    void Start()
    {
        isItWorking = true;
        StartRandomSwitching();
    }

    public void SwitchLight()
    {
        Debug.Log("SwitchLight " + isItWorking);

        if (isItWorking)
        {
            TurnAllLightsOff();
            LightSwitcherTurnOffAnimation();
            isItWorking = false;
            StopRandomSwitching();
            sfx.PlayLightSwitchOffSound();
        }
        else
        {
            TurnAllLightsOn();
            StartRandomSwitching();
            LightSwitcherTurnOnAnimation();
            isItWorking = true;
            sfx.PlayLightSwitchOnSound();
        }
    }
    public void SwitchLightOn()
    {
        TurnAllLightsOn();
        LightSwitcherTurnOnAnimation();
        isItWorking = true;
        sfx.PlayLightSwitchOnSound();
    }
    public void SwitchLightOff()
    {
        TurnAllLightsOff();
        LightSwitcherTurnOffAnimation();
        isItWorking = false;
        StopRandomSwitching();
        sfx.PlayLightSwitchOffSound();
    }
    public void TurnAllLightsOn()
    {
        foreach (Light light in lights)
        {
            light.enabled = true;
        }
    }
    public void TurnAllLightsOff()
    {
        foreach (Light light in lights)
        {
            light.enabled = false;
        }
    }
    public void StartRandomSwitching()
    {
        isRandomSwitching = true;
        isItWorking = true;
        StartCoroutine(RandomLightSwitching());
    }

    public void StopRandomSwitching()
    {
        isRandomSwitching = false;
    }

    private IEnumerator RandomLightSwitching()
    {
        while (isRandomSwitching)
        {
            if (!isItWorking) break;

            float delay = Random.Range(minTimeInterval, maxTimeInterval);
            yield return new WaitForSeconds(delay);

            if (!isItWorking) break;

            // Determine how many lights to switch
            int lightsToSwitch = Random.Range(minLightsToSwitch, lights.Length);

            // Create array of indices and shuffle it
            int[] indices = new int[lightsToSwitch];
            for (int i = 0; i < lightsToSwitch; i++)
            {
                indices[i] = Random.Range(minLightsToSwitch, lights.Length);
            }

            // Switch random lights
            for (int i = 0; i < lightsToSwitch; i++)
            {
                lights[indices[i]].enabled = !lights[indices[i]].enabled;
            }

            yield return new WaitForSeconds(0.1f);

            if (isItWorking)
            {
                TurnAllLightsOn();
            }
        }
    }

    private void LightSwitcherTurnOnAnimation()
    {
        Vector3 currentRotation = switcher.transform.rotation.eulerAngles;
        switcher.transform.rotation = Quaternion.Euler(0, currentRotation.y, currentRotation.z);
    }
    private void LightSwitcherTurnOffAnimation()
    {
        Vector3 currentRotation = switcher.transform.rotation.eulerAngles;
        switcher.transform.rotation = Quaternion.Euler(13, currentRotation.y, currentRotation.z);
    }

    private void SetAllLightsRed()
    {
        foreach (Light light in lights)
        {
            light.color = Color.red;
        }
    }
    private void SetAllLightsWhite()
    {//ECE2B0
        foreach (Light light in lights)
        {
            light.color = Color.white;
        }
    }
}
