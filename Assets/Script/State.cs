using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    protected BossStateMachine _stateMachine;
    public bool canExit;

    protected virtual void Awake()
    {
        canExit = true;
        _stateMachine = GetComponent<BossStateMachine>();
    }

    public abstract void Enter();


    public abstract void Tick();


    public abstract void Exit();

}
