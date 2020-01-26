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

    Vector3 randomNormalizedVector()
    {
        return Vector3.Normalize(new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)));
    }

    // Start is called before the first frame update
    void Start()
    {
        m_center = m_planet.transform;
        // m_radius = m_planet.transform.localScale.x * 1.1f;
        Vector3 randPosition = randomNormalizedVector();
        Vector3 dis = transform.position - m_center.position;
        transform.position = (transform.position - m_center.position).normalized * m_radius + m_center.position;
        m_rotateAxis = Vector3.Cross(dis, randPosition);
        m_rotateAxis = Vector3.Normalize(m_rotateAxis);

        // Level
        m_health += m_level * 10;
        m_attack += m_level * 2;
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(m_center.position, m_rotateAxis, m_rotateSpeed * Time.deltaTime);
    }

    public void InitAttribute(int level, Type type, Belong belong, GameObject planet)
    {
        this.m_level = level;
        this.m_type = type;
        this.m_belong = belong;
        this.m_planet = planet;

        m_health += level * 10;
        m_attack += level * 2;
        SetColorByType();
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
        //halo.enable = true;
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
}
