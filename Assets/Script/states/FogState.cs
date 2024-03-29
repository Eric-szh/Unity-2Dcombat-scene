using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogState : State
{
    public GameObject fog;
    public float fogHeight = 0.5f;
    public float chargeSpeed = 7f;
    public float foggingTime= 3f;

    private Vector2 chargePoint;

    public override void Enter()
    {
        GetComponent<BossBehavior>().TurnToPlayer();
        if (GetComponent<BossBehavior>().facingDirection == 1)
        {
            GetComponent<BossAniController>().ChangeAnimationState("Boss_toxicR");
            this.chargePoint = new Vector2(100, this.transform.position.y);
        }
        else
        {
            GetComponent<BossAniController>().ChangeAnimationState("Boss_toxicL");
            this.chargePoint = new Vector2(-100, this.transform.position.y);
        }

        Invoke("FogLeave", foggingTime);
        
    }

    public override void Exit()
    {
        return;
    }

    public override void Tick()
    {
       GetComponent<BossBehavior>().MoveTo(chargePoint, chargeSpeed);
    }

    public void FogLeave()
    {
        this._stateMachine.ChangeState<DecideState>();
    }

    public void FogAppear()
    {
        Transform localPosition = this.GetComponent<Transform>();
        Vector2 fogPosition = new Vector2(localPosition.position.x, localPosition.position.y + fogHeight);
        Instantiate(fog, fogPosition, localPosition.rotation);
    }

    public void OnDrawGizmosSelected()
    {
        Transform localPosition = this.GetComponent<Transform>();
        Vector2 fogPosition = new Vector2(localPosition.position.x, localPosition.position.y + fogHeight);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(fogPosition, 0.5f);
    }
 

}
