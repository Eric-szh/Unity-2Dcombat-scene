using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineController : MonoBehaviour
{
    public Transform hidePoint;
    public Transform showPoint;
    public float groundLevel = -2.05f;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = hidePoint.position;
        GetComponent<Animator>().Play("Stop");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            EnableVine(showPoint);
        }
    }

    public void EnableVine(Transform position)
    {
        this.transform.position = new Vector2(position.position.x, groundLevel);
        GetComponent<Animator>().Play("Vine");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerBehavior>().TakeDamage(1);
            Debug.Log("Ranged HitPlayer");
        }
    }

    public void DisableVine()
    {
        this.transform.position = hidePoint.position;
        GetComponent<Animator>().Play("Stop");
    }

 

}
