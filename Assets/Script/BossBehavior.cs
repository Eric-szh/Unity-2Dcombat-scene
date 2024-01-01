using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{

    public float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int xDirection = 0;

        // use the left and right arrow keys to move the boss
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            xDirection = -1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            xDirection = 1;
        }

        this.transform.position = new Vector3(this.transform.position.x + this.speed * Time.deltaTime * xDirection, this.transform.position.y, this.transform.position.z);
    }
}
