using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public GameObject pauseObject;
    public GameObject bgm;

    // Start is called before the first frame update
    void Start()
    {
        bgm = GameObject.Find("BGM");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pauseObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Intro");
        GameObject.Destroy(bgm);
    }
}
