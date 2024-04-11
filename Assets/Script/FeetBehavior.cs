using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetBehavior : MonoBehaviour
{
    public Boolean isGrounded = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if ((collision.gameObject.tag == "Land"))
        {
            Debug.Log("Feet hit something!");
            this.transform.parent.GetComponent<PlayerBehavior>().Land();
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Land"))
        {
            Debug.Log("Feet left something!");
            isGrounded = false;
        }
    }
}
