using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    bool hitGround = false;
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !hitGround)
        {
            Debug.Log("Bia");
            hitGround = true;
            GameObject boss = GameObject.Find("Boss");
            boss.GetComponent<WaitState>().StartMoving();
        }
    }


}
