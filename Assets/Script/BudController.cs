using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BudController : MonoBehaviour
{
    public GameObject bud;
    public GameObject boss;
    public float distanceToBoss = 5.0f;
    public int budLeft = 5;
    public float groundLevel = -17.59f;
    public float leftBound = -14.0f;
    public float rightBound = 14.0f;
    public bool ultReady = false;


    private void Start()
    {
    }

    public void SpawnBud() {         
        if (budLeft > 0)
        {
            Vector2 position = new Vector2(Random.Range(leftBound, rightBound), groundLevel);
            while(Vector2.Distance(position, boss.transform.position) < distanceToBoss)
            {
                position = new Vector2(Random.Range(leftBound, rightBound), groundLevel);
            }
            Instantiate(bud, position, Quaternion.identity);
            budLeft--;
            float time = Random.Range(5.0f, 10.0f);
            Invoke("SpawnBud", time);
        }
        else
        {
            ultReady = true;
            Debug.Log("Ultimate Ready");
        }
    }
}
