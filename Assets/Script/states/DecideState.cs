using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecideState : State
{
    public float chaseThreshold;
    public float waitTime;
    public float rangedThreshold;
    public int fogRatio;

    private bool lastRanged = false;


    public override void Enter()
    {
        GetComponent<BossAniController>().ChangeAnimationState("Boss_idle");
        Invoke("Decide", waitTime);
    }

    public override void Exit()
    {
        return;
    }

    public override void Tick()
    {

    }

    private void Decide()
    {
        GameObject player = this._stateMachine.Player;
        float distance = Vector3.Distance(player.transform.position, this.transform.position);
        if (Random.Range(0, 100) < fogRatio)
        {
            this._stateMachine.ChangeState<PrepareFogState>();
            this.lastRanged = false;
        }
        else if (distance < chaseThreshold || lastRanged || distance > rangedThreshold)
        {
            this._stateMachine.ChangeState<ChaseState>();
            this.lastRanged = false;
        }
        else
        {
            this._stateMachine.ChangeState<RangedState>();
            this.lastRanged = true;
        }
    }
}
