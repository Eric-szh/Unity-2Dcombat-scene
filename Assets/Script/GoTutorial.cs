using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoTutorial : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToTutorial()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial");
    }
}
