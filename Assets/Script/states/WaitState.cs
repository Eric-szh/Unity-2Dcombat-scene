using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitState : State
{
    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Tick()
    {

    }

    public void StartMoving()
    {
        GetComponent<BossStateMachine>().ChangeState<DecideState>();
    }
}
