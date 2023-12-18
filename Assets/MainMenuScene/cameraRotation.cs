using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRotation : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject target; // The target object
    public float speed = 3.5f; // Speed of rotation
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // Rotate around the target at the specified speed
            transform.RotateAround(target.transform.position, Vector3.up, speed * Time.deltaTime);

            // Make sure the camera is always looking at the target
            transform.LookAt(target.transform);
        }
    }
}
