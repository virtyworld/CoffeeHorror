using UnityEngine;
using System;

/// <summary>
/// StateMachine - Generic State Machine Base Class
/// 
/// Main Logic:
/// This abstract class provides a generic state machine implementation that can be
/// inherited by any MonoBehaviour to create state-driven behavior. It manages state
/// transitions, provides event notifications for state changes, and enforces the
/// implementation of state-specific logic through abstract methods.
/// 
/// Key Features:
/// - Generic state management with type safety
/// - Event-driven state change notifications
/// - Abstract method enforcement for state handling
/// - Automatic initial state setup
/// - State transition validation
/// </summary>
/// <typeparam name="T">The enum type that defines the possible states</typeparam>
public abstract class StateMachine<T> : MonoBehaviour where T : Enum
{
    // Текущее состояние
    protected T currentState;

    // Событие для изменения состояния
    public delegate void StateChangeHandler(T newState);
    public event StateChangeHandler OnStateChanged;

    /// <summary>
    /// Initializes the state machine and sets the initial state
    /// Called automatically when the MonoBehaviour starts
    /// </summary>
    protected virtual void Start()
    {
        // Устанавливаем начальное состояние
        SetState(GetInitialState());
    }

    /// <summary>
    /// Abstract method that must be implemented to return the initial state
    /// </summary>
    /// <returns>The initial state for the state machine</returns>
    protected abstract T GetInitialState();

    /// <summary>
    /// Changes the current state to the new state if they are different
    /// Triggers state change events and calls the state handler
    /// </summary>
    /// <param name="newState">The new state to transition to</param>
    public void SetState(T newState)
    {
        if (!currentState.Equals(newState))
        {
            currentState = newState;
            OnStateChanged?.Invoke(currentState);

            // Выполняем действия в зависимости от состояния
            HandleStateChange(currentState);
        }
    }

    /// <summary>
    /// Abstract method that must be implemented to handle state-specific logic
    /// Called whenever the state changes
    /// </summary>
    /// <param name="newState">The new state that was just set</param>
    protected abstract void HandleStateChange(T newState);

    /// <summary>
    /// Returns the current state of the state machine
    /// </summary>
    /// <returns>The current state</returns>
    public T GetCurrentState()
    {
        return currentState;
    }
}