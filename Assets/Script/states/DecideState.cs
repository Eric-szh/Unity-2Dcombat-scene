using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecideState : State
{
    public float chaseThreshold;
    public float waitTime;
    public float rangedThreshold;
    public int fogRatio;
    public int hardRatio;
    public GameObject budController;
    public int maxMiss;
    public int minMiss;


    private bool lastRanged = false;
    private bool lastFog = false;
    private int triggerMiss;
    private int missCount = 0;

    protected override void Awake()
    {
        base.Awake();
        triggerMiss = Random.Range(minMiss, maxMiss);
    }

    public override void Enter()
    {
        Debug.Log("Decide");
        GetComponent<BossAniController>().ChangeAnimationState("Boss_idle");
        Invoke("Decide", waitTime);
    }

    public override void Exit()
    {
        CancelInvoke("Decide");
    }

    public override void Tick()
    {

    }

    private void Decide()
    {
        // Debug.Log("Deciding");
        GameObject player = this._stateMachine.Player;
        float distance = Vector3.Distance(player.transform.position, this.transform.position);
        if (budController.GetComponent<BudController>().ultReady)
        {
            this._stateMachine.ChangeState<UltState>();
            budController.GetComponent<BudController>().ultReady = false;
            this.lastRanged = false;
            this.lastFog = false;
        } 
        else if (missCount >= triggerMiss)
        {
            Debug.Log("Hard");
            Hard();
        } 
        else if (Random.Range(0, 100) < fogRatio && !lastFog)
        {
            this._stateMachine.ChangeState<PrepareFogState>();
            this.lastRanged = false;
            this.lastFog = true;
        }
        else if (distance < chaseThreshold || lastRanged || distance > rangedThreshold)
        {
            this._stateMachine.ChangeState<ChaseState>();
            this.lastRanged = false;
            this.lastFog = false;
        }
        else
        {
            this._stateMachine.ChangeState<RangedState>();
            this.lastRanged = true;
            this.lastFog = false;
        }
    }

    private void Hard()
    {
        this.triggerMiss = Random.Range(minMiss, maxMiss);
        this.missCount = 0;
        if (Random.Range(0, 100) < hardRatio)
        {
            this._stateMachine.ChangeState<PrepareFogState>();
            this.lastRanged = false;
            this.lastFog = true;
        }
        else
        {
            this._stateMachine.ChangeState<RangedState>();
            this.lastRanged = true;
            this.lastFog = false;
        }
    }

    public void Miss()
    {
        this.missCount++;
    }
}
