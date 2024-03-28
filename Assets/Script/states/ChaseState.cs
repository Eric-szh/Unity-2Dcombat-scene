using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    // the distance at which the enemy will start attacking
    public float attackDistance = 4.0f;

    // the maximum time the enemy will chase the player
    public float maximumChaseTime = 5.0f; 

    public override void Enter()
    {
        Invoke("GiveUp", maximumChaseTime);
    }

    public override void Exit()
    {
        CancelInvoke("GiveUp");
    }

    public override void Tick()
    {
        // move towards the player
        GameObject player = this._stateMachine.Player;

        // determine if it is going left or right
        if (player.transform.position.x < this.transform.position.x)
        {
            this.GetComponent<BossBehavior>().SetFacingLeft();
            this.GetComponent<BossAniController>().ChangeAnimationState("Boss_left");
        }
        else
        {
            this.GetComponent<BossBehavior>().SetFacingRight();
            this.GetComponent<BossAniController>().ChangeAnimationState("Boss_right");
        }
        this.GetComponent<BossBehavior>().MoveTo(player.transform.position);

        // if the player is close enough, attack
        float distance = Vector3.Distance(player.transform.position, this.transform.position);
        if (distance < attackDistance)
        {
            this._stateMachine.ChangeState<MReadyState>();
        }
    }

    private void GiveUp()
    {
        this._stateMachine.ChangeState<DecideState>();
    }

}
