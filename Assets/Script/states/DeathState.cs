using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : State
{
    protected override void Awake()
    {
        base.Awake();
        canExit = false;
    }

    public override void Enter()
    {
        Debug.Log("Enter Death");
        GetComponent<BossAniController>().ChangeAnimationState("Boss_death");
    }

    public override void Exit()
    {
        Debug.Log("Exit Death");
    }

    public override void Tick()
    {

    }

    public void DeathEnd()
    {
        GetComponent<BossBehavior>().gameController.GetComponent<GameController>().WinGame();
        GetComponent<BossAniController>().ChangeAnimationState("Boss_dead");

    }
}
