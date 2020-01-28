using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public GameObject m_planet;
    Transform m_center;
    public float m_radius = 4.5f;
    float m_rotateSpeed = 80.0f;
    Vector3 m_rotateAxis = Vector3.up;

    public int m_health = 100;
    int m_attack = 10;
    public int m_level = 1;
    public Type m_type;
    public Belong m_belong; // 0 for player, 1 for opponents

    bool m_isMoving = false;
    GameObject m_startPlanet;
    GameObject m_targetPlanet;
    Vector3 m_targetPosition;
    Vector3 m_startPosition;
    float m_moveSpeed = 0.6f;
    float m_moveTimer = 0.0f;


    Vector3 randomNormalizedVector()
    {
        return Vector3.Normalize(new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)));
    }

    // Start is called before the first frame update
    void Start()
    {
        InitRotate();

        // Level
        m_health += m_level * 10;
        m_attack += m_level * 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_isMoving)    // Orbit the planet
        {
            this.transform.RotateAround(m_center.position, m_rotateAxis, m_rotateSpeed * Time.deltaTime);
        }
        else    // Move to target planet
        {
            // Rotate to the side of planet that will not move through the planet
            Vector3 start = m_startPosition - m_startPlanet.transform.position;
            Vector3 sphere = m_targetPlanet.transform.position - m_startPlanet.transform.position;
            if (Vector3.Dot(start, sphere) < 0)
            {
                transform.RotateAround(m_center.position, m_rotateAxis, m_rotateSpeed * Time.deltaTime);
                m_startPosition = this.transform.position;
                m_targetPosition = (this.transform.position - m_targetPlanet.transform.position).normalized * m_radius + 
                                   m_targetPlanet.transform.position;
            }
            else
            {
                m_moveTimer += m_moveSpeed * Time.deltaTime;
                //this.gameObject.transform.position += (m_targetPosition - m_startPosition).normalized * m_moveSpeed * Time.deltaTime;
                transform.position = Vector3.Lerp(m_startPosition, m_targetPosition, m_moveTimer);
                if (transform.position == m_targetPosition)
                {
                    m_isMoving = false;
                    m_planet = m_targetPlanet;
                    InitRotate();
                    m_targetPlanet.GetComponent<Planet>().Arrive(this);
                }
            }
        }
    }

    public void InitAttribute(int level, Type type, Belong belong, GameObject planet)
    {
        this.m_level = level;
        this.m_type = type;
        this.m_belong = belong;
        this.m_planet = planet;

        m_health += level * 10;
        m_attack += level * 2;
        this.gameObject.transform.localScale *= level;
        SetColorByType();
        InitRotate();
    }

    void InitRotate()
    {
        m_center = m_planet.transform;
        // m_radius = m_planet.transform.localScale.x * 1.1f;
        Vector3 randPosition = randomNormalizedVector();
        Vector3 dis = transform.position - m_center.position;
        transform.position = (transform.position - m_center.position).normalized * m_radius + m_center.position;
        m_rotateAxis = Vector3.Cross(dis, randPosition);
        m_rotateAxis = Vector3.Normalize(m_rotateAxis);
    }

    void SetColorByType()
    {
        Renderer renderer = this.gameObject.GetComponent<Renderer>();
        switch (m_type)
        {
            case Type.WATER:
                Color c = new Color(0, 0, 1, 1);
                renderer.material.SetColor("_Color", c);
                break;
            case Type.GRASS:
                renderer.material.SetColor("_Color", Color.green);
                break;
            case Type.FIRE:
                renderer.material.SetColor("_Color", Color.red);
                break;
        }

        /* Glow Control */
        //Behaviour halo = (Behaviour)this.gameObject.GetComponent("Halo");
        //halo.enabled = true;
        //SerializedObject halo = new SerializedObject(this.gameObject.GetComponent("Halo"));
        //halo.FindProperty("m_Size").floatValue += 3f;
        //halo.FindProperty("m_Enabled").boolValue = true;
        //if (m_belong == Belong.PLAYER)
        //{
        //    halo.FindProperty("m_Color").colorValue = Color.white;
        //}
        //else
        //{
        //    halo.FindProperty("m_Color").colorValue = Color.black;
        //}
        //halo.ApplyModifiedProperties();
        GameObject shell = this.gameObject.transform.GetChild(0).gameObject;
        Renderer childRenderer = shell.GetComponent<Renderer>();
        switch (m_belong)
        {
            case Belong.PLAYER:
                childRenderer.material.SetColor("_Color", new Color(1, 1, 1, 0.3f));
                break;
            case Belong.ENEMY:
                childRenderer.material.SetColor("_Color", new Color(0, 0, 0, 0.5f));
                break;
        }
    }

    public void Attack(Soldier other, Planet planet)
    {
        other.m_health -= this.m_attack;
    }

    public void MoveToPlanet(Planet planet)
    {
        m_isMoving = true;
        m_startPlanet = m_planet;
        m_planet = planet.gameObject;
        m_targetPlanet = planet.gameObject;
        m_startPosition = this.transform.position;
        m_targetPosition = (this.transform.position - m_targetPlanet.transform.position).normalized * m_radius + planet.transform.position;
        m_moveTimer = 0.0f;
    }
}
