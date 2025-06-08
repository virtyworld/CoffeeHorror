using UnityEngine;

public class ScenaryHandler : MonoBehaviour
{
    [SerializeField] private GuestReplacer guestReplacers;
    [SerializeField] private BehindTheBack behindTheBack;
    [SerializeField] private float timeBetweenScenarios = 60f; // Time in seconds between scenario changes

    private float scenarioTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scenarioTimer = timeBetweenScenarios;
        behindTheBack.StartLogic();
    }

    // Update is called once per frame
    void Update()
    {
        // scenarioTimer -= Time.deltaTime;

        // if (scenarioTimer <= 0f)
        // {
        //     TriggerNextScenario();
        //     scenarioTimer = timeBetweenScenarios;
        // }
    }

    private void TriggerNextScenario()
    {
        Debug.Log("TriggerNextScenario");
        // Randomly choose between scenarios
        int randomScenario = Random.Range(0, 2); // 0 or 1

        if (randomScenario == 0)
        {
            guestReplacers.StartLogic();
        }
        else
        {
            // Reset and activate the behind the back scenario
            behindTheBack.StartLogic();
        }
    }

}
