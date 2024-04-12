using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickEnter : MonoBehaviour
{
    public GameObject bgmObject;
    // Start is called before the first frame update
    void Start()
    {
        bgmObject = GameObject.Find("BGM");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Debug.Log("Mouse Down");
        SceneManager.LoadScene("BossLevel");
        GameObject.Destroy(bgmObject);
    }
}
