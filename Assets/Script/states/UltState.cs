using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltState : State
{
    public GameObject gameController;

    public override void Enter()
    {
        GetComponent<BossAniController>().ChangeAnimationState("Boss_ult");
    }

    public override void Exit()
    {
    }

    public override void Tick()
    {
    }

    
    public void Darken()
    {
        gameController.GetComponent<GameController>().Darken();
    }

    public void FinishUlt() { 
        this._stateMachine.ChangeState<DecideState>();
    }
}
