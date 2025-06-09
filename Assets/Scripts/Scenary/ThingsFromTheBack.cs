using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ThingsFromTheBack : MonoBehaviour
{
    [SerializeField] private GameObject objectFromTheBack;
    [SerializeField] private AudioSource audioSource;
    private bool isScenarioStarted = false;
    public void Setup(UnityEvent onPlayerTurnOnCoffeeMachine)
    {
        onPlayerTurnOnCoffeeMachine.AddListener(ActivateScenario);
    }
    public void StartLogic()
    {
        Debug.Log("StartLogic");
        isScenarioStarted = true;
    }

    private void ActivateScenario()
    {
        if (!isScenarioStarted) return;

        int random = Random.Range(0, 4);

        if (random == 0)
        {
            objectFromTheBack.SetActive(true);
            StartCoroutine(ReplaceBackAfterDelay());
        }
        else
        {
            objectFromTheBack.SetActive(false);
        }
    }

    private IEnumerator ReplaceBackAfterDelay()
    {
        audioSource.Play();
        yield return new WaitForSeconds(3f);
        isScenarioStarted = false;
        objectFromTheBack.SetActive(false);
    }
}
