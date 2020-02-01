using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public GameObject m_planet;
    Transform m_center;
    public float m_radius = 4.5f;
    float m_rotateSpeed = 80.0f;
    Vector3 m_rotateAxis = Vector3.up;

    GameObject m_targetSoldier;
    int m_attck;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.RotateAround(m_center.position, m_rotateAxis, m_rotateSpeed * Time.deltaTime);
    }

}
