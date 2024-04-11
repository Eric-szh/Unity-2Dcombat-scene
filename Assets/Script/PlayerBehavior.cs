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
    bool bloomed = false;
    public bool death = false;

    bool isAttacking = false;
    public Vector3 attackDeviation;
    public float attackRange;
    public int attackDamage = 10;

    public GameObject gameController;

    public void AttackInterrpution()
    {
        this.isAttacking = false;
    }

    public void Heal(int healAmount)
    {
        this.health += healAmount;
        if (this.health > this.maxHealth)
        {
            this.health = this.maxHealth;
        }
        healthBar.SetHealth(health);
    }

    public void TakeDamage(int damage, float paralyze_time = 0.2f, bool ignoreInv = false, float invincible_time = 3f)
    {

        if (this.invincible && !ignoreInv && this.death)
        {
            return;
        }

        this.health -= damage;
        healthBar.SetHealth(health);
        // Debug.Log("took damage");
        if (health <=  0 && !death)
        {
            //die, this is cute
            Debug.Log("died!!!!orz");
            this.PlayDirBased("Player_deathL", "Player_deathR", true);
            this.death = true;
            this.invincible = true;
        }

        if (!this.invincible)
        {
            this.StartParalyze(paralyze_time);
            if (invincible_time > 0)
            {
                this.StartInvincible(invincible_time);
            }
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
            var colorTempAgain = GetComponent<SpriteRenderer>().color;
            colorTempAgain.a = 1f;
            GetComponent<SpriteRenderer>().color = colorTempAgain;
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
        if (!invincible)
        {
            this.entangled = true;
            GetComponent<PlayerAniController>().ChangeAnimationState("Player_restrained");
        }
        
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


        if (death)
        {
            return;
        }

        // color fix
        if (!slowed && !invincible && GetComponent<SpriteRenderer>().color != new Color(1f, 1f, 1f))
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
        }

        this.xDirection = 0;

        {
            // Debug.Log(Input.GetAxis("Horizontal"));
            this.xDirection = (int) Input.GetAxis("Horizontal");
            if (this.xDirection != 0)
            {
                this.facingDirection = (int)Input.GetAxis("Horizontal");
                this.attackDeviation = new Vector3(Math.Abs(this.attackDeviation.x) * this.facingDirection, this.attackDeviation.y, this.attackDeviation.z);
            }

        }


        // movement
        if (!this.paralyzed)
        {
            this.transform.position = new Vector3(this.transform.position.x + this.speed * Time.deltaTime * xDirection, this.transform.position.y, this.transform.position.z);
        }

        //jumping
        if (Input.GetButtonDown("Jump") && !this.paralyzed)
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
        
        // Debug.Log(Input.GetAxis("Dash"));
        // Debug.Log(Input.GetAxis("Dash"));

        //dashing
        if (Input.GetButtonDown("Dash") && !this.paralyzed)
        {
           
            if (this.availableDashes > 0 && !isDashing)
            {   
                this.PlayDirBased("Player_dashL", "Player_dashR");
                StartCoroutine(Dash(xDirection));
                if (in_air)

                    this.availableDashes = 0;
            }
            
        }

        //attacking
        if (Input.GetButtonDown("Attack") && !this.paralyzed)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                this.PlayDirBased("Player_slashL", "Player_slashR");
            }
        }

        if (Input.GetButtonDown("Ult") && !this.paralyzed)
        {
            GameObject amulet = GameObject.Find("Amulet");
            int charges = amulet.GetComponent<AmuletController>().currentState;
            if (charges >= 4)
            {
                amulet.GetComponent<AmuletController>().Use();
                this.GetComponent<PlayerAniController>().ChangeAnimationState("Player_bloom");
                this.bloomed = true;
                this.invincible = true;
            }
            

        }

        // animation stuff
        if (!in_air && !isDashing && !entangled && !isAttacking && !bloomed && !paralyzed)
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

    public void Lighten()
    {
        this.gameController.GetComponent<GameController>().Lighten();
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
                return;
            }
        }
    }

    public void AttackEnd()
    {
        isAttacking = false;
    }

    public void BloomEnd()
    {
        this.bloomed = false;
        this.invincible = false;
    }
    public void DeathEnd()
    {
        this.gameController.GetComponent<GameController>().LoseGame();
        GetComponent<PlayerAniController>().ChangeAnimationState("Player_dead");
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
