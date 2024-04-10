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
    public float DotInterval = 0.1f;
    public float TimeAfterDark = 2f;
    float opacity = 0;
    bool isDark = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Darken()
    {
        StartCoroutine(DarkenScreen());
        Invoke("StartDot", TimeAfterDark);
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

    IEnumerator Dot()
    {
        while (isDark)
        {
            yield return new WaitForSeconds(DotInterval);
            player.GetComponent<PlayerBehavior>().TakeDamage(2, 0, true, 0);
        }
        yield break;
    }

    void StartDot()
    {
        isDark = true;
        StartCoroutine(Dot());
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
