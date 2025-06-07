using UnityEngine;

public class CoffeeMachine : StateMachine<CoffeeMachine.CoffeeMachineState>
{
    [SerializeField] private CofeeMachineCupChecker cupChecker;
    [SerializeField] private MeshRenderer lamp;
    [SerializeField] private float workingTime = 10f; // Время работы кофемашины в секундах

    private float workingTimer;
    private bool isWorking;


    private void Start()
    {

    }

    // Enum для состояний кофемашины
    public enum CoffeeMachineState
    {
        Idle,    // Простаивает
        Working  // Работает
    }

    public void StartCoffee()
    {
        if (!isWorking)
        {
            SetState(CoffeeMachineState.Working);
        }
    }

    protected override CoffeeMachineState GetInitialState()
    {
        return CoffeeMachineState.Idle;
    }

    protected override void HandleStateChange(CoffeeMachineState newState)
    {
        switch (newState)
        {
            case CoffeeMachineState.Idle:
                HandleIdleState();
                break;
            case CoffeeMachineState.Working:
                HandleWorkingState();
                break;
        }
    }

    private void Update()
    {
        if (isWorking)
        {
            workingTimer += Time.deltaTime;

            if (workingTimer >= workingTime)
            {
                isWorking = false;
                workingTimer = 0f;
                SetState(CoffeeMachineState.Idle);
            }
        }
    }

    // Обработка состояния "Простаивает"
    private void HandleIdleState()
    {
        Debug.Log("Кофемашина простаивает");
        SetLampColor(Color.red);
        isWorking = false;
        workingTimer = 0f;
    }

    // Обработка состояния "Работает"
    private void HandleWorkingState()
    {
        Debug.Log("Кофемашина работает");
        SetLampColor(Color.green);
        isWorking = true;
        workingTimer = 0f;
        StartCreatingCoffee();
    }

    // Метод для переключения состояния (можно вызывать из других скриптов)
    public void ToggleState()
    {
        if (!isWorking)
        {
            if (currentState == CoffeeMachineState.Idle)
            {
                SetState(CoffeeMachineState.Working);
            }
            else
            {
                SetState(CoffeeMachineState.Idle);
            }
        }
    }

    private void SetLampColor(Color color)
    {
        lamp.material.color = color;
    }

    private void StartCreatingCoffee()
    {
        if (cupChecker.isCupInPlace)
        {
            cupChecker.paperCup.StartCoffeeChange();
        }
    }

}