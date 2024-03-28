using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareFogState : State
{
    public override void Enter()
    {
        GetComponent<BossAniController>().ChangeAnimationState("Boss_preFog");
    }

    public override void Exit()
    {

    }

    public override void Tick()
    {

    }

    public void GoFog()
    {
        this._stateMachine.ChangeState<FogState>();
    }

}
