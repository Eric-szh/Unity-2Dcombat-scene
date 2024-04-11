using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AmuletController : MonoBehaviour
{
    public int currentState = 0;    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Progress()
    {
        GetComponent<AniController>().ChangeAnimationState("amu_" + Convert.ToString(currentState) + "-" + Convert.ToString(currentState + 1));
        currentState++;
    }

    public void Static()
    {
        GetComponent<AniController>().ChangeAnimationState("amu_" + Convert.ToString(currentState));
    }

    public void Use()
    {
        if (currentState >= 4)
        {
            GetComponent<AniController>().ChangeAnimationState("amu_use");
            currentState = 0;
        }
    }
}
