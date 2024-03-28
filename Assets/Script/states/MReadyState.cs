using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MReadyState : State
{
    public float attackWindupTime = 0.5f;
    public override void Enter()
    {
        Invoke("Attack", attackWindupTime);
    }

    public override void Exit()
    {
    }

    public override void Tick()
    {
    }

    private void Attack()
    {
        this._stateMachine.ChangeState<MeleeState>();
    }
}
