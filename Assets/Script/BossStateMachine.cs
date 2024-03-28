using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateMachine : MonoBehaviour
{
    public State CurrentState => _currentState;
    private State _currentState;
    public GameObject Player;

    protected bool _isTransitioning;

    private void Start()
    {
        this.ChangeState<WaitState>();
    }

    public void ChangeState<T>() where T : State 
    { 
        T newState = GetComponent<T>();
        if (newState == null)
        {
            Debug.LogError("New state is null");
            return;
        }
        InitNewState(newState);

    }

    void InitNewState(State targetState)
    {
        if (_currentState != targetState && !_isTransitioning)
        {
            CallNewState(targetState);
        }
        else
        {
            Debug.LogWarning("State is already transitioning from " + _currentState + " to " + targetState);
        }
    }

    void CallNewState(State newState)
    {
        _isTransitioning = true;

        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();

        _isTransitioning = false;
    }

    private void Update()
    {
        if (_currentState != null && !_isTransitioning)
        {
            _currentState.Tick();
        }
    }

}
