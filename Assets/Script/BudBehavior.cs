using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BudBehavior : MonoBehaviour
{
    bool isGrown = false;
    GameObject amulet;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BudAniController>().ChangeAnimationState("Bud_sprout");
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && isGrown)
        {
            // Debug.Log("Bloom");
            Bloom();
            GameObject.Find("Amulet").GetComponent<AmuletController>().Progress();
        } else if (collision.gameObject.tag == "Boss" && isGrown)
        {
            Corrupt();
        }
    }
    
    void Bloom()
    {
        GetComponent<BudAniController>().ChangeAnimationState("Bud_bloom");
        this.isGrown = false;
    }

    void BloomStay()
    {
        GetComponent<BudAniController>().ChangeAnimationState("Bud_bloomed");
    }

    void Corrupt()
    {
        GetComponent<BudAniController>().ChangeAnimationState("Bud_corrupt");
        this.isGrown = false;
    }

    void CorruptStay()
    {
        GetComponent<BudAniController>().ChangeAnimationState("Bud_corrupted");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BudIdle()
    {
        this.isGrown = true;
        GetComponent<BudAniController>().ChangeAnimationState("Bud_idle");
    }
}
