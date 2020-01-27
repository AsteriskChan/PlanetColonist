using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type { WATER, FIRE, GRASS };
public enum Belong { PLAYER, ENEMY, NONE };
public class Planet : MonoBehaviour
{
    
    public GameObject m_soldier;
    public List<Soldier> m_playerSoldiers = new List<Soldier>();
    public List<Soldier> m_enemySoldiers = new List<Soldier>();
    public int m_playerSoldiersNum = 0;
    public int m_enemySoldiersNum = 0;

    // Timer
    public float m_generateInterval = 2.0f;
    private float m_generateTimer = 0.0f;
    public float m_fightInterval = 1.0f;
    private float m_fightTimer = 0.0f;

    // Attribute
    public Belong m_belong = Belong.PLAYER;
    public int m_level = 1;
    public Type m_type = Type.FIRE;
    private float m_radius = 0.5f;

    GameObject m_flagCube;
    AudioSource m_occupyAudioSource;
    AudioSource m_moveAudioSource;

    Vector3 randomNormalizedVector()
    {
        return Vector3.Normalize(new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)));
    }

    // Start is called before the first frame update
    void Start()
    {
        m_radius = 0.5f * transform.localScale.x;
        m_flagCube = this.transform.GetChild(0).gameObject;
        SetFlagColorByBelong(m_belong);
        for (int i = 0; i < m_playerSoldiersNum; ++i)
        {
            GenerateSoldier(Belong.PLAYER);
        }
        for (int i = 0; i < m_enemySoldiersNum; ++i)
        {
            GenerateSoldier(Belong.ENEMY);
        }
        AudioSource[] audios = this.gameObject.GetComponents<AudioSource>();
        m_occupyAudioSource = audios[0];
        m_moveAudioSource = audios[1];
    }

    // Update is called once per frame
    void Update()
    {
        m_generateTimer += Time.deltaTime;
        m_fightTimer += Time.deltaTime;
        // Check if we have reached beyond 2 seconds.
        // Subtracting two is more accurate over time than resetting to zero.
        if (m_generateTimer > m_generateInterval)
        {
            // Remove the recorded 2 seconds.
            m_generateTimer -= m_generateInterval;
            GenerateSoldier(this.m_belong);
            
        }
        if (m_fightTimer > m_fightInterval)
        {
            m_fightTimer -= m_fightInterval;
            Fight();
        }
        Occupy();

    }

    void GenerateSoldier(Belong belong)
    {
        if (belong == Belong.NONE)
        {
            return;
        }
        // Create a new soldier
        Vector3 randomPosition = randomNormalizedVector() * m_radius + transform.position;
        GameObject newSoldierObject = Instantiate(m_soldier, randomPosition, new Quaternion());
        newSoldierObject.transform.parent = GameObject.Find("_DynamicSoldiers").transform;
        Soldier newSoldier = newSoldierObject.GetComponent<Soldier>();
        newSoldier.InitAttribute(this.m_level, this.m_type, belong, this.gameObject);
        if (belong == Belong.PLAYER)
        {
            m_playerSoldiers.Add(newSoldier);  
        }
        if (belong == Belong.ENEMY)
        {
            m_enemySoldiers.Add(newSoldier);
        }
    }

    void Fight()
    {
        int playerNum = m_playerSoldiers.Count;
        int enemyNum = m_enemySoldiers.Count;
        if (playerNum == 0 || enemyNum == 0)
        {
            return;
        }

        // Player attacks Enemy
        for (int i = 0, j = 0; i < playerNum; ++i, ++j)
        {
            // Reset J
            if (j >= enemyNum)
            {
                j = 0;
            }
            m_playerSoldiers[i].Attack(m_enemySoldiers[j], this);
        }
        // Enemy attacks Player
        for (int i = 0, j = 0; i < enemyNum; ++i, ++j)
        {
            // Reset J
            if (j >= playerNum)
            {
                j = 0;
            }
            m_enemySoldiers[i].Attack(m_playerSoldiers[j], this);
        }
        
        // Clear dead soldiers
        List<Soldier> deadSoldiers = new List<Soldier>();
        for (int i = playerNum - 1; i >= 0; --i)
        {
            Soldier s = m_playerSoldiers[i];
            if (s.m_health <= 0)
            {
                m_playerSoldiers.RemoveAt(i);
                deadSoldiers.Add(s);
            }
        }
        for (int i = enemyNum - 1; i >= 0; --i)
        {
            Soldier s = m_enemySoldiers[i];
            if (s.m_health <= 0)
            {
                m_enemySoldiers.RemoveAt(i);
                deadSoldiers.Add(s);
            }
        }
        foreach(Soldier s in deadSoldiers)
        {
            Destroy(s.gameObject);
        }
    }

    void SetFlagColorByBelong(Belong belong)
    {
        Renderer flagRenderer = m_flagCube.GetComponent<Renderer>();
        switch (belong)
        {
            case Belong.ENEMY:
                flagRenderer.material.SetColor("_Color", Color.black);
                break;
            case Belong.PLAYER:
                flagRenderer.material.SetColor("_Color", Color.white);
                break;
            case Belong.NONE:
                flagRenderer.material.SetColor("_Color", Color.gray);
                break;
        }  
    }

    void Occupy()
    {
        if (m_belong != Belong.PLAYER && m_playerSoldiers.Count >= 10 && m_enemySoldiers.Count == 0)
        {
            for (int i = 0; i < 10; ++i)
            {
                Destroy(m_playerSoldiers[i].gameObject);
            }
            m_playerSoldiers.RemoveRange(0, 10);
            m_belong = Belong.PLAYER;
            SetFlagColorByBelong(m_belong);
            m_occupyAudioSource.Play();
        }
        if (m_belong != Belong.ENEMY && m_enemySoldiers.Count >= 10 && m_playerSoldiers.Count == 0)
        {
            for (int i = 0; i < 10; ++i)
            {
                Destroy(m_enemySoldiers[i].gameObject);
            }
            m_enemySoldiers.RemoveRange(0, 10);
            m_belong = Belong.ENEMY;
            SetFlagColorByBelong(m_belong);
            m_occupyAudioSource.Play();
        }
    }

    // num = -1 : move all soldiers
    public void MoveSoldier(Planet target, int num)
    {
        foreach(Soldier s in m_playerSoldiers.ToArray())
        {
            s.MoveToPlanet(target);
            m_playerSoldiers.Remove(s);
        }
        m_moveAudioSource.Play();
    }

    // Soldier arrives
    public void Arrive(Soldier s)
    {
        m_playerSoldiers.Add(s);
    }

    public void Upgrade()
    {
        if (m_belong != Belong.PLAYER)
        {
            return;
        }
        int playerSoldierNum = m_playerSoldiers.Count;
        if (playerSoldierNum >= m_level * 10)
        {
            for (int i = 0; i < m_level * 10; ++i)
            {
                Destroy(m_playerSoldiers[i].gameObject);
            }
            m_playerSoldiers.RemoveRange(0, m_level * 10);
            m_level += 1;
            return;
        }
    }
}
