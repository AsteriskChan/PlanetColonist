using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    const int m_boundary = 20;
    const float m_speed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveMouse();
    }

    void moveMouse()
    {
        Vector3 position = transform.position;
        Camera camera = this.gameObject.GetComponent<Camera>();
        if (Input.mousePosition.x > Screen.width - m_boundary)
        {
            position.x += m_speed * Time.deltaTime; // move on +X axis
        }
        if (Input.mousePosition.x < 0 + m_boundary)
        {
            position.x -= m_speed * Time.deltaTime; // move on -X axis
        }
        if (Input.mousePosition.y > Screen.height - m_boundary)
        {
            position.z += m_speed * Time.deltaTime; // move on +Z axis
        }
        if (Input.mousePosition.y < 0 + m_boundary)
        {
            position.z -= m_speed * Time.deltaTime; // move on -Z axis
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (camera.orthographic)
            {
                camera.orthographicSize -= m_speed * Time.deltaTime;
            }
            else
            {
                position.y -= m_speed * Time.deltaTime;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (camera.orthographic)
            {
                camera.orthographicSize += m_speed * Time.deltaTime;
            }
            else
            {
                position.y += m_speed * Time.deltaTime;
            }
        }
        transform.position = position;
    }
}
