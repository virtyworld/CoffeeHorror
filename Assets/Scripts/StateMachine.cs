using UnityEngine;
using System;

public abstract class StateMachine<T> : MonoBehaviour where T : Enum
{
    // Текущее состояние
    protected T currentState;

    // Событие для изменения состояния
    public delegate void StateChangeHandler(T newState);
    public event StateChangeHandler OnStateChanged;

    protected virtual void Start()
    {
        // Устанавливаем начальное состояние
        SetState(GetInitialState());
    }

    // Метод для получения начального состояния (должен быть реализован в дочернем классе)
    protected abstract T GetInitialState();

    // Метод для изменения состояния
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

    // Метод для обработки изменения состояния (должен быть реализован в дочернем классе)
    protected abstract void HandleStateChange(T newState);

    // Получение текущего состояния
    public T GetCurrentState()
    {
        return currentState;
    }
}