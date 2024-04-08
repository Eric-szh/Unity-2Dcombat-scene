using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject dark;
    public GameObject player;
    public GameObject winScreen;
    public GameObject loseScreen;
    public float DarkenTime = 1f;
    public float LightenTime = 1f;
    public float MaximumOpacity = 0.7f;
    float opacity = 0;
    bool isDark = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Darken()
    {
        isDark = true;
        StartCoroutine(DarkenScreen());
    }

    IEnumerator DarkenScreen()
    {
        float darkAppearDelay = DarkenTime / 100;
        while (this.opacity < MaximumOpacity)
        {
            this.opacity += 0.01f;
            dark.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, opacity);
            yield return new WaitForSeconds(darkAppearDelay);
        }
        yield break;

    }

    public void Lighten()
    {
        if (isDark) { 
            StopCoroutine(DarkenScreen());
            isDark = false;
            StartCoroutine(LightenScreen());
        }
    }

    IEnumerator LightenScreen()
    {
        float darkDisappearDelay = LightenTime / 100;
        while (this.opacity > 0)
        {
            this.opacity -= 0.01f;
            dark.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, opacity);
            yield return new WaitForSeconds(darkDisappearDelay);
        }
        yield break;
    }


    // Update is called once per frame
    void Update()
    {
        if (isDark)
        {
            player.GetComponent<PlayerBehavior>().TakeDamage(1, 0, true);
        }
    }

    public void WinGame()
    {
        this.winScreen.SetActive(true);
    }

    public void LoseGame()
    {
        this.loseScreen.SetActive(true);
    }
}
