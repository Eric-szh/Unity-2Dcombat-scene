using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{

    public const float speed = 4f;
    public int facingDirection = 0;
    public GameObject attackPoint;
    public float pushForce = 50f;
    public int contactDamage = 20;

    public int Health = 200;
    public int maxHealth = 200;

    private bool secondPhase = false;
    private bool isDead = false;

    public GameObject gameController;

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
        Vector3 newPos = new Vector3(pos.x, this.transform.position.y, this.transform.position.z);
        this.transform.position = Vector3.MoveTowards(this.transform.position, newPos, speed * Time.deltaTime);
    }

    public void PushPlayerAway(Vector3 orignalPoint, float upForce = 1.2f, float awayForce = 0.5f)
    {
        GameObject player = GetComponent<BossStateMachine>().Player;
        Vector3 pushDir = (player.transform.position - orignalPoint).normalized;  
        Vector2 pushDir2 = new Vector2(Math.Sign(pushDir.x) * awayForce, upForce);
        if (!player.GetComponent<PlayerBehavior>().death)
        {
            player.GetComponent<Rigidbody2D>().velocity = pushDir2 * pushForce;
        }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            this.PushPlayerAway(collision.contacts[0].point);
            collision.gameObject.GetComponent<PlayerBehavior>().TakeDamage(contactDamage);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }

        this.Health -= damage;
        Debug.Log("Boss took " + damage + " damage");
        if (this.Health <= 0)
        {
            this.GetComponent<BossStateMachine>().ChangeState<DeathState>();
            isDead = true;
        }
        else
        {
            Flash();
            if (this.Health <= this.maxHealth * 0.6 && !secondPhase)
            {
                GameObject.Find("BudController").GetComponent<BudController>().SpawnBud();
                secondPhase = true;
            }
        }

    }

    // quickly flash to indicate damage
    public void Flash()
    {
        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        for (int i = 0; i < 1; i++)
        {
            sprite.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }
}

