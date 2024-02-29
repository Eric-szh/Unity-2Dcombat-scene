using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    
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


    public void TakeDamage(int damage)
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "Boss"))
        {
            //Debug.Log("hitboss");
            TakeDamage(30);
        }


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
        int xDirection = 0;

        //if (!isDashing)
        {
            if (Input.GetKey("a"))
            {
                xDirection = -1;
            }
            if (Input.GetKey("d"))
            {
                xDirection = 1;
            }
        }



        this.transform.position = new Vector3(this.transform.position.x + this.speed * Time.deltaTime * xDirection, this.transform.position.y, this.transform.position.z);
        // update velocity in animator
        this.GetComponent<Animator>().SetFloat("player_velocity", xDirection);

        //jumping
        if (Input.GetKeyDown("space"))
        {
            if (this.availableJumps > 0)
            {   
                in_air = true;
                this.GetComponent<Animator>().SetBool("player_dash", false);
                //resets the gravity if the player is currently dashing
                this.GetComponent<Rigidbody2D>().gravityScale = 3;
                // make the players falling speed 0
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                
                this.availableJumps--;
                float jump_counter = this.availableJumps;
                this.GetComponent<Animator>().SetFloat("jump_counter", jump_counter);


                this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, this.jumpForce), ForceMode2D.Impulse);
                this.GetComponent<Animator>().SetBool("player_fly", true);
            }
        }

        //dashing
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (this.availableDashes > 0)
            {
                StartCoroutine(Dash(xDirection));
                if (in_air)
                    this.availableDashes--;
            }
            
        }
    }

    public void Land() { 
        in_air = false;
        this.availableJumps = this.maxJumps;
        this.availableDashes = this.maxDashes;

        this.GetComponent<Animator>().SetBool("player_fly", false);
        // Debug.Log("Player landed");
        float jump_counter = this.availableJumps;
        this.GetComponent<Animator>().SetFloat("jump_counter", jump_counter);

    }
    IEnumerator Dash(float Direction) {
        
        isDashing = true;
        this.GetComponent<Animator>().SetBool("player_dash", true);
        // make the players falling speed 0
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, 0);
        //dash
        this.GetComponent<Rigidbody2D>().AddForce(new Vector2(dashDistance*Direction, 0f), ForceMode2D.Impulse);
        
        //set the gravity to 0 so player doesn't fall while dashing
        float gravity = this.GetComponent<Rigidbody2D>().gravityScale;
        this.GetComponent<Rigidbody2D>().gravityScale = 0;

        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        this.GetComponent<Animator>().SetBool("player_dash", false);
        this.GetComponent<Rigidbody2D>().gravityScale = gravity;
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, this.GetComponent<Rigidbody2D>().velocity.y);
    }
}
