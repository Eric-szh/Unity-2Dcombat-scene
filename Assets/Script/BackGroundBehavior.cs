using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundBehavior : MonoBehaviour
{
    private Transform caremaTransform;
    // how fast the background moves relative to the camera, how fast it follows the camera
    public float parallaxFactor = 0.5f;
    private Vector2 startCameraPos;
    private Vector2 startBackGroundPos;
    // Start is called before the first frame update
    void Start()
    {
        caremaTransform = Camera.main.transform;
        startCameraPos = caremaTransform.position;
        startBackGroundPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float deltaX = caremaTransform.position.x - startCameraPos.x;
        float deltaY = caremaTransform.position.y - startCameraPos.y;
        float newX = startBackGroundPos.x + deltaX * parallaxFactor;
        float newY = startBackGroundPos.y + deltaY * parallaxFactor;
        this.transform.position = new Vector3(newX, newY, this.transform.position.z);
    }
}
