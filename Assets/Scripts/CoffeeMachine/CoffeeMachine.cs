using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// CoffeeMachine - Coffee Machine State Management System
/// 
/// Main Logic:
/// This script manages the coffee machine's operation using a state machine pattern.
/// It controls the coffee brewing process, visual feedback through lamp colors,
/// sound effects, and integration with cup placement detection. The machine can
/// be in two states: idle (red lamp) or working (green lamp), with automatic
/// timing for coffee preparation.
/// 
/// Key Features:
/// - State machine-based operation control
/// - Visual feedback through colored lamps
/// - Automatic timing for coffee preparation
/// - Sound effect integration
/// - Cup placement detection integration
/// - Event-driven communication with other systems
/// </summary>
public class CoffeeMachine : StateMachine<CoffeeMachine.CoffeeMachineState>
{
    [SerializeField] private CofeeMachineCupChecker cupChecker;
    [SerializeField] private MeshRenderer lamp;
    [SerializeField] private float workingTime = 10f; // Время работы кофемашины в секундах
    [SerializeField] private SFX sfx;

    private float workingTimer;
    private bool isWorking;

    private UnityEvent onPlayerTurnOnCoffeeMachine;

    /// <summary>
    /// Initializes the coffee machine with player interaction event
    /// Connects to the event system for player turn-on notifications
    /// </summary>
    /// <param name="onPlayerTurnOnCoffeeMachine">Event triggered when player turns on coffee machine</param>
    public void Setup(UnityEvent onPlayerTurnOnCoffeeMachine)
    {
        this.onPlayerTurnOnCoffeeMachine = onPlayerTurnOnCoffeeMachine;
    }

    /// <summary>
    /// Enum defining the possible states of the coffee machine
    /// </summary>
    public enum CoffeeMachineState
    {
        Idle,    // Простаивает
        Working  // Работает
    }

    /// <summary>
    /// Starts the coffee brewing process if the machine is not already working
    /// Transitions the machine to the working state
    /// </summary>
    public void StartCoffee()
    {
        if (!isWorking)
        {
            SetState(CoffeeMachineState.Working);
        }
    }

    /// <summary>
    /// Returns the initial state of the coffee machine
    /// </summary>
    /// <returns>Idle state as the default starting state</returns>
    protected override CoffeeMachineState GetInitialState()
    {
        return CoffeeMachineState.Idle;
    }

    /// <summary>
    /// Handles state transitions and calls appropriate state handlers
    /// </summary>
    /// <param name="newState">The new state to transition to</param>
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

    /// <summary>
    /// Main update loop that manages the working timer
    /// Automatically transitions back to idle state when working time expires
    /// </summary>
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

    /// <summary>
    /// Handles the idle state of the coffee machine
    /// Sets red lamp color, stops sound effects, and resets timer
    /// </summary>
    private void HandleIdleState()
    {
        Debug.Log("Кофемашина простаивает");
        SetLampColor(Color.red);
        isWorking = false;
        workingTimer = 0f;
        if (sfx != null)
        {
            sfx.StopCoffeeMachineSound();
        }
    }

    /// <summary>
    /// Handles the working state of the coffee machine
    /// Sets green lamp color, plays sound effects, and starts coffee creation
    /// </summary>
    private void HandleWorkingState()
    {
        Debug.Log("Кофемашина работает");
        onPlayerTurnOnCoffeeMachine?.Invoke();
        SetLampColor(Color.green);
        isWorking = true;
        workingTimer = 0f;
        if (sfx != null)
        {
            sfx.PlayCoffeeMachineSound();
        }
        StartCreatingCoffee();
    }

    /// <summary>
    /// Toggles the coffee machine state between idle and working
    /// Can be called from other scripts to control the machine
    /// </summary>
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

    /// <summary>
    /// Changes the lamp color to provide visual feedback
    /// </summary>
    /// <param name="color">The color to set the lamp to</param>
    private void SetLampColor(Color color)
    {
        lamp.material.color = color;
    }

    /// <summary>
    /// Initiates the coffee creation process if a cup is in place
    /// Calls the cup's coffee change method to start brewing
    /// </summary>
    private void StartCreatingCoffee()
    {
        if (cupChecker.isCupInPlace)
        {
            cupChecker.paperCup.StartCoffeeChange();
        }
    }

}