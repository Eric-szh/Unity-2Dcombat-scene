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
    public float DotInterval = 0.5f;
    public float TimeAfterDark = 2f;
    public float PreventionTime = 2f;
    float opacity = 0;
    bool isDark = false;
    bool preventDark = false;

    public void Darken()
    {
        if (!preventDark)
        {
            isDark = true;
            StartCoroutine(DarkenScreen());
            Invoke("StartDot", TimeAfterDark);
        } else
        {
            preventDark = false;
            Debug.Log("Dark prevented");
        }

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
        StartCoroutine(Dot());
    }

    public void Lighten()
    {
        preventDark = true;
        Invoke("StopPreventDark", PreventionTime);
        Debug.Log("Prevent dark");

    }

    void StopPreventDark()
    {
        Debug.Log("Prevent dark stopped");
        preventDark = false;
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
            if (preventDark)
            {
                StopCoroutine(DarkenScreen());
                isDark = false;
                StartCoroutine(LightenScreen());
                Debug.Log("Stop dark manual check");
            }
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
