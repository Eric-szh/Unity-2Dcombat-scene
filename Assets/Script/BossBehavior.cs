using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{

    public const float speed = 5f;
    public int facingDirection = 0;
    public GameObject attackPoint;

    public void TurnToPlayer()
    {
        GameObject player = GetComponent<BossStateMachine>().Player;
        if (player.transform.position.x < this.transform.position.x)
        {
            this.GetComponent<BossBehavior>().SetFacingLeft();
        }
        else
        {
            this.GetComponent<BossBehavior>().SetFacingRight();
        }
    }

    public void MoveTo(Vector3 pos, float speed = speed)
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, pos, speed * Time.deltaTime);
    }

    private void AtkLeft()
    {
        this.attackPoint.transform.localPosition = new Vector3(Math.Abs(this.attackPoint.transform.localPosition.x) * -1, this.attackPoint.transform.localPosition.y, this.attackPoint.transform.localPosition.z);
    }

    private void AtkRight()
    {
        this.attackPoint.transform.localPosition = new Vector3(Math.Abs(this.attackPoint.transform.localPosition.x), this.attackPoint.transform.localPosition.y, this.attackPoint.transform.localPosition.z);
    }

    public void SetFacingLeft()
    {
        if (this.facingDirection == 1)
        {
            this.AtkLeft();
            this.facingDirection = -1;
        }
    }

    public void SetFacingRight()
    {
        if (this.facingDirection == -1)
        {
            this.AtkRight();
            this.facingDirection = 1;
        }
    }
}
