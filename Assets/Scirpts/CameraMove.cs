using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    const int m_boundary = 20;
    const float m_speed = 10.0f;
    const float m_scrollSpeed = 20.0f;

    // Used for touchscreen
    private Vector2 m_oldPosition1 = new Vector2();
    private Vector2 m_oldPosition2 = new Vector2();

    Vector2 m_screenPos = new Vector2();

    // Start is called before the first frame update
    void Start()
    {
        Input.multiTouchEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        moveMouse();
        moveFinger();
    }

    bool isEnlarge(Vector2 oP1, Vector2 oP2, Vector2 nP1, Vector2 nP2)
    {
        float len1 = Vector2.Distance(oP1, oP2);
        float len2 = Vector2.Distance(nP1, nP2);
        return len1 < len2;
    }

    void moveFinger()
    {
        Camera camera = this.gameObject.GetComponent<Camera>();
        if (Input.touchCount == 1)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
                m_screenPos = Input.touches[0].position;
            if (Input.touches[0].phase == TouchPhase.Moved)
            {
                transform.Translate(new Vector3(Input.touches[0].deltaPosition.x * Time.deltaTime, Input.touches[0].deltaPosition.y * Time.deltaTime, 0));
            }
        }
        if (Input.touchCount > 1)
        {
            Vector2 nposition1 = new Vector2();
            Vector2 nposition2 = new Vector2();

            Touch touch1 = Input.touches[0];
            Touch touch2 = Input.touches[1];
            if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
            {
                nposition1 = touch1.position;
                nposition2 = touch2.position;
                if (isEnlarge(m_oldPosition1, m_oldPosition2, nposition1, nposition2))
                {
                    camera.orthographicSize -= m_scrollSpeed * Time.deltaTime * 2;
                }
                else
                {
                    camera.orthographicSize += m_scrollSpeed * Time.deltaTime * 2;
                }
                    m_oldPosition1 = nposition1;
                    m_oldPosition2 = nposition2;
             }         
        }
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
                camera.orthographicSize -= m_scrollSpeed * Time.deltaTime * 2;
            }
            else
            {
                position.y -= m_scrollSpeed * Time.deltaTime;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (camera.orthographic)
            {
                camera.orthographicSize += m_scrollSpeed * Time.deltaTime * 2;
            }
            else
            {
                position.y += m_scrollSpeed * Time.deltaTime;
            }
        }
        transform.position = position;
    }
}
