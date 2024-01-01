using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public float speed = 1f;
    public float jumpForce = 5f;
    int health = 100;
    int availableJumps = 2;
    int maxJumps = 2;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hello World!");
    }

    // Update is called once per frame
    void Update()
    {
        int xDirection = 0;

        if (Input.GetKey("a"))
        {
            xDirection = -1;
        }
        if (Input.GetKey("d"))
        {
            xDirection = 1;
        }

        this.transform.position = new Vector3(this.transform.position.x + this.speed * Time.deltaTime * xDirection, this.transform.position.y, this.transform.position.z);
        // update velocity in animator
        this.GetComponent<Animator>().SetFloat("player_velocity", xDirection);


        if (Input.GetKeyDown("space"))
        {
            if (this.availableJumps > 0)
            {
                this.availableJumps--;

                // make the players falling speed 0
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, 0);

                this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, this.jumpForce), ForceMode2D.Impulse);
                this.GetComponent<Animator>().SetBool("player_fly", true);
            }
        }


    }

    public void Land() { 
        this.availableJumps = this.maxJumps;
        this.GetComponent<Animator>().SetBool("player_fly", false);
        // Debug.Log("Player landed");
        
    }
}
