using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeState : State
{
    // the entire length of the attack
    public float attackLength = 1.0f;
    // when the attack should start after the animation starts
    public float startTime = 0.0f;

    public GameObject attackPoint;
    public float attackRange = 1.0f;
    public LayerMask playerLayer;

    public override void Enter()
    {
        
        if (GetComponent<BossBehavior>().facingDirection == 1)
        {
            GetComponent<BossAniController>().ChangeAnimationState("Boss_slashR");
        }
        else
        {
            GetComponent<BossAniController>().ChangeAnimationState("Boss_slashL");
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

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);
    }

    // the calling of the attack is done in the animation clips
    private void Attack()
    {
        Debug.Log("Attacking");
        Collider2D playerHit = Physics2D.OverlapCircle(attackPoint.transform.position, attackRange, playerLayer);
        // Debug.Log(playerHit);
        if (playerHit != null)
        {
            playerHit.GetComponent<PlayerBehavior>().TakeDamage(30);
        }
    }

    // the calling of the leave is done in the animation clips
    private void Leave()
    {
        this._stateMachine.ChangeState<DecideState>();
    }

}
