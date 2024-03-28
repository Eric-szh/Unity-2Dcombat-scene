using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    int xDirection = 0;
    int facingDirection = 0;

    public float speed = 4f;
    public float jumpForce = 12f;

    public HealthBar healthBar;
    public int maxHealth = 100;
    public int health;

    int availableJumps = 2;
    int maxJumps = 2;

    int maxDashes = 1;
    int availableDashes = 1;

    public float dashDistance = 10f;
    public float dashTime = 0.4f;
    bool isDashing;
    bool in_air = false;

    bool paralyzed = false;
    bool invincible = false;

    float slowAmount = 0.5f;
    float slowTime = 2f;
    bool slowed = false;
    bool entangled = false;

    bool isAttacking = false;
    public Vector3 attackDeviation;
    public float attackRange;
    public int attackDamage = 10;

    public void AttackInterrpution()
    {
        this.isAttacking = false;
    }

    public void TakeDamage(int damage, float paralyze_time = 0.2f)
    {
        this.health -= damage;
        healthBar.SetHealth(health);
        Debug.Log("took damage");
        if (health <=  0 )
        {
            //die, this is cute
            Debug.Log("died!!!!orz");
            this.GetComponent<Animator>().SetBool("player_died", true);
        }

        if (!this.invincible)
        {
            this.StartParalyze(paralyze_time);
            this.StartInvincible(1f);
        }

    }

    void StartParalyze(float paralyze_time)
    {
        this.paralyzed = true;
        Invoke("Unparalyze", paralyze_time);
    }

    void StartInvincible(float invincible_time)
    {
        this.invincible = true;
        // put player in another layer
        this.gameObject.layer = 11;
        StartCoroutine(this.flashing());
        Invoke("Uninvincible", invincible_time);
    }

    // flashing effect when player is paralyzed
    IEnumerator flashing()
    {
        while (this.invincible)
        {
            // turn the sprite transparent
            var colorTemp = GetComponent<SpriteRenderer>().color;
            colorTemp.a = 0.5f;
            GetComponent<SpriteRenderer>().color = colorTemp;
            yield return new WaitForSeconds(0.1f);
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void Uninvincible()
    {
        CancelInvoke("Uninvincible");
        this.invincible = false;
        // put player back to the original layer
        this.gameObject.layer = 10;
    }

    private void Unparalyze()
    {
        CancelInvoke("Unparalyze");
        this.paralyzed = false;
    }

    // when entangled, it will interrut the current animation!! and play the entangled animation
    public void Entangle()
    {
        this.entangled = true;
        GetComponent<PlayerAniController>().ChangeAnimationState("Player_restrained");
    }

    public void Unentangle()
    {
        this.entangled = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hello World!");
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        this.xDirection = 0;

        {
            if (Input.GetKey("a"))
            {
                this.xDirection = -1;
                this.facingDirection = -1;
                this.attackDeviation = new Vector3(Math.Abs(this.attackDeviation.x) * -1, this.attackDeviation.y, this.attackDeviation.z);
            }
            if (Input.GetKey("d"))
            {
                this.xDirection = 1;
                this.facingDirection = 1;
                this.attackDeviation = new Vector3(Math.Abs(this.attackDeviation.x), this.attackDeviation.y, this.attackDeviation.z);
            }
        }


        // movement
        if (!this.paralyzed)
        {
            this.transform.position = new Vector3(this.transform.position.x + this.speed * Time.deltaTime * xDirection, this.transform.position.y, this.transform.position.z);
        }

        //jumping
        if (Input.GetKeyDown("space") && !this.paralyzed)
        {
            if (this.availableJumps > 0)
            {   
                in_air = true;
                this.PlayDirBased("Player_jumpL", "Player_jumpR", true);

                //resets the gravity if the player is currently dashing
                this.GetComponent<Rigidbody2D>().gravityScale = 3;
                // make the players falling speed 0
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                
                this.availableJumps--;
                float jump_counter = this.availableJumps;
                this.GetComponent<Animator>().SetFloat("jump_counter", jump_counter);


                this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, this.jumpForce), ForceMode2D.Impulse);
            }
        }

        //dashing
        if (Input.GetKeyDown(KeyCode.LeftShift) && !this.paralyzed)
        {
            if (this.availableDashes > 0)
            {
                this.PlayDirBased("Player_dashL", "Player_dashR");
                StartCoroutine(Dash(xDirection));
                if (in_air)
                    this.availableDashes--;
            }
            
        }

        //attacking
        if (Input.GetKeyDown(KeyCode.Mouse0) && !this.paralyzed)
        {
            if (!isAttacking)
            {
                this.GetComponent<PlayerAniController>().ChangeAnimationState("Player_attack");
                isAttacking = true;
                this.PlayDirBased("Player_slashL", "Player_slashR");
            }
        }

        // animation stuff
        if (this.in_air == false && this.isDashing == false && this.entangled == false && this.isAttacking == false && this.paralyzed == false)
        {
            if(xDirection == 1)
            {
                this.GetComponent<PlayerAniController>().ChangeAnimationState("Player_right");
            } else if (xDirection == -1)
            {
                this.GetComponent<PlayerAniController>().ChangeAnimationState("Player_left");
            }
            else
            {
                this.GetComponent<PlayerAniController>().ChangeAnimationState("Player_idle");
            }
            
        }
        // Debug.Log("in_air: " + in_air);
        // Debug.Log("isDashing: " + isDashing);
        // Debug.Log("entangled: " + entangled);
        // Debug.Log("isAttacking: " + isAttacking);

    }

    private void PlayDirBased(string leftAnimation, string rightAnimation, bool forced = false)
    {
        if(this.facingDirection == -1)
        {
            this.GetComponent<PlayerAniController>().ChangeAnimationState(leftAnimation, forced);
        }
        else
        {
            this.GetComponent<PlayerAniController>().ChangeAnimationState(rightAnimation, forced);
        }
        
    }

    public void Attack()
    {
        Vector3 attackPoint = this.transform.position + this.attackDeviation;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint, this.attackRange);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.tag == "Boss")
            {
                enemy.GetComponent<BossBehavior>().TakeDamage(this.attackDamage);
            }
        }
    }

    public void AttackEnd()
    {
        isAttacking = false;
    }

    public void Land() { 
        in_air = false;
        this.availableJumps = this.maxJumps;
        this.availableDashes = this.maxDashes;

        // Debug.Log("Player landed");
        float jump_counter = this.availableJumps;
    }
    IEnumerator Dash(float Direction) {
        
        isDashing = true;
        // make the players falling speed 0
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, 0);
        //dash
        this.GetComponent<Rigidbody2D>().AddForce(new Vector2(dashDistance*Direction, 0f), ForceMode2D.Impulse);
        
        //set the gravity to 0 so player doesn't fall while dashing
        float gravity = this.GetComponent<Rigidbody2D>().gravityScale;
        this.GetComponent<Rigidbody2D>().gravityScale = 0;

        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        this.GetComponent<Rigidbody2D>().gravityScale = gravity;
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, this.GetComponent<Rigidbody2D>().velocity.y);
    }

    public void SlowDown()
    {
        if (slowed)
        {
            CancelInvoke("Unslow");
            Invoke("Unslow", slowTime);
        } else
        {   
            this.slowed = true;
            this.speed *= slowAmount;
            Invoke("Unslow", slowTime);
        }

        // turn the sprite purple
        GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 1f);
    }

    private void Unslow()
    {
        this.speed /= slowAmount;
        CancelInvoke("Unslow");
        this.slowed = false;
        // turn the sprite back to normal
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
    }

    private void OnDrawGizmosSelected()
    {
        if (this.attackDeviation == null)
        {
            return;
        }

        Vector3 attackPoint = this.transform.position + this.attackDeviation;

        Gizmos.DrawWireSphere(attackPoint, this.attackRange);
    }
}
