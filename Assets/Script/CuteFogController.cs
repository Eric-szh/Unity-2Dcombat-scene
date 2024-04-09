using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuteFogController : MonoBehaviour
{
    float opacity = 0;
    float maximumOpacity = 0.7f;
    float fogAppearTime = 1f;
    float fogPresistTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, opacity);
        // Debug.Log("Fog Start");
        StartCoroutine(FogAppear());
    }

    IEnumerator FogAppear()
    {
        float fogAppearDelay = fogAppearTime / 100;
        while (this.opacity < maximumOpacity) {
            this.opacity += 0.01f;
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, opacity);
            // Debug.Log("Fog Appearing " + opacity);
            yield return new WaitForSeconds(fogAppearDelay);
        }

        StartCoroutine(FogPresist());
    }

    IEnumerator FogPresist()
    {
        yield return new WaitForSeconds(fogPresistTime);
        StartCoroutine(FogDisappear());
    }

    IEnumerator FogDisappear()
    {
        float fogDisappearDelay = fogAppearTime / 100;
        while (this.opacity > 0)
        {
            this.opacity -= 0.01f;
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, opacity);
            // Debug.Log("Fog Disappearing " + opacity);
            yield return new WaitForSeconds(fogDisappearDelay);
        }
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerBehavior>().SlowDown();
            other.gameObject.GetComponent<PlayerBehavior>().TakeDamage(1, 0, true, 1);
            Debug.Log("Fog HitPlayer");
            // slow the player
            
        }
    }
}
