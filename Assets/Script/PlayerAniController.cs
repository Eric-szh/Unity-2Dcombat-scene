using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAniController : MonoBehaviour
{
    public Animator animator;
    private string currentState;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeAnimationState(string newState, bool forced = false)
    {

        if (currentState == newState && !forced) return;

        if (forced)
        {
            animator.Play(newState, -1, 0f);
        }
        else
        {
            animator.Play(newState);
        }
        

        currentState = newState;
    }
}
