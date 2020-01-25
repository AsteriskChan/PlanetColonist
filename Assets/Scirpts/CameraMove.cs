using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    const int boundary = 20;
    const float speed = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        if (Input.mousePosition.x > Screen.width - boundary)
        {
            position.x += speed * Time.deltaTime; // move on +X axis
        }
        if (Input.mousePosition.x < 0 + boundary)
        {
            position.x -= speed * Time.deltaTime; // move on -X axis
        }
        if (Input.mousePosition.y > Screen.height - boundary)
        {
            position.z += speed * Time.deltaTime; // move on +Z axis
        }
        if (Input.mousePosition.y < 0 + boundary)
        {
            position.z -= speed * Time.deltaTime; // move on -Z axis
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            position.y -= speed * Time.deltaTime;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            position.y += speed * Time.deltaTime;
        }
        transform.position = position;
    }
}
