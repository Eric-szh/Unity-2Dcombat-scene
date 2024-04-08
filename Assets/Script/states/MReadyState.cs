using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MReadyState : State
{
    public float attackWindupTime = 0.5f;
    public override void Enter()
    {
        // Debug.Log("MReadyState");
        Invoke("MAttack", attackWindupTime);
    }

    public override void Exit()
    {
        // Debug.Log("MReadyState Exit");
    }

    public override void Tick()
    {
    }

    private void MAttack()
    {
        // Debug.Log("MReadyState Attack");
        this._stateMachine.ChangeState<MeleeState>();
    }
}
