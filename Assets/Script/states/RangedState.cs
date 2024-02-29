using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedState : State
{
    public Transform lastPlayerLocation;
    public VineController vine;
    public override void Enter()
    {
        GameObject player = this._stateMachine.Player;
        lastPlayerLocation.position = player.transform.position;
        GetComponent<BossBehavior>().TurnToPlayer();
        if (GetComponent<BossBehavior>().facingDirection == 1)
        {
            GetComponent<BossAniController>().ChangeAnimationState("Boss_spittingR");
        }
        else
        {
            GetComponent<BossAniController>().ChangeAnimationState("Boss_spittingL");
        }
    }

    public override void Exit()
    {
        return;
    }

    public override void Tick()
    {
        return;
    }

    public void RangedAttack()
    {
        vine.EnableVine(lastPlayerLocation);
    }

    public void RangedLeave()
    {
        this._stateMachine.ChangeState<DecideState>();
    }
}
