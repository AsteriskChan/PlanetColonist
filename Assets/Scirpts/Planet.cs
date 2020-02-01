using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum Type { WATER, FIRE, GRASS };
public enum Belong { PLAYER, ENEMY, NONE };
public class Planet : MonoBehaviour
{
    
    public GameObject m_soldier;
    public List<Soldier> m_playerSoldiers = new List<Soldier>();
    public List<Soldier> m_enemySoldiers = new List<Soldier>();

    public int m_initPlayerSoldiersNum = 0;
    public int m_initEnemySoldiersNum = 0;

    List<GameObject> m_planets;     // Store all planets

    // Some UI GameObject
    GameObject m_numberText;
    GameObject m_global;
    GameObject m_planetInfoText;
    GameObject m_upgradeButton;

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

    private int m_moveNumber = 0;
    private bool m_addNumber = true;
    float m_maxDistance = 30;

    // Some children objects
    GameObject m_haloObject;    // For target highlight effect
    GameObject m_playerCube;
    GameObject m_enemyCube;

    // Sound Effects
    AudioSource m_occupyAudioSource;
    AudioSource m_moveAudioSource;

    Vector3 randomNormalizedVector()
    {
        return Vector3.Normalize(new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), 
            UnityEngine.Random.Range(-1.0f, 1.0f), 
            UnityEngine.Random.Range(-1.0f, 1.0f)));
    }

    // Start is called before the first frame update
    void Start()
    {
        m_radius = 0.5f * transform.localScale.x;
        m_haloObject = this.transform.GetChild(0).gameObject;
        m_playerCube = this.transform.GetChild(1).gameObject;
        m_enemyCube = this.transform.GetChild(2).gameObject;

        for (int i = 0; i < m_initPlayerSoldiersNum; ++i)
        {
            GenerateSoldier(Belong.PLAYER);
        }
        for (int i = 0; i < m_initEnemySoldiersNum; ++i)
        {
            GenerateSoldier(Belong.ENEMY);
        }

        AudioSource[] audios = this.gameObject.GetComponents<AudioSource>();
        m_occupyAudioSource = audios[0];
        m_moveAudioSource = audios[1];

        m_global = GameObject.Find("GlobalObject");
        m_numberText = GameObject.Find("Number Text");
        m_planetInfoText = GameObject.Find("Planet Info Text");
        m_upgradeButton = GameObject.Find("Upgrade Button");

        m_planets = new List<GameObject>(GameObject.FindGameObjectsWithTag("Planet"));
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

        UpdateBelong();
    }

    void UpdateBelong()
    {
        int playerNum = m_playerSoldiers.Count;
        int enemyNum = m_enemySoldiers.Count;
        int sumNum = Math.Max(playerNum + enemyNum, 1); // Avoid division by 0
        Vector3 scale = m_playerCube.transform.localScale;
        scale.x = (float)playerNum / (float)(sumNum);
        m_playerCube.transform.localScale = scale;
        scale = m_enemyCube.transform.localScale;
        scale.x = (float)enemyNum / (float)(sumNum);
        m_enemyCube.transform.localScale = scale;
        if (m_belong != Belong.PLAYER && m_playerSoldiers.Count > 0 && m_enemySoldiers.Count == 0)
        {
            m_belong = Belong.PLAYER;
            m_occupyAudioSource.Play();
        }
        else if (m_belong != Belong.ENEMY && m_enemySoldiers.Count > 0 && m_playerSoldiers.Count == 0)
        {
            m_belong = Belong.ENEMY;
            m_occupyAudioSource.Play();
        }
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

    // num = -1 : move all soldiers
    public void MoveSoldier(Planet target, int num, Belong belong)
    {
        if (belong == Belong.PLAYER)
        {
            if (m_playerSoldiers.Count <= 0)
            {
                return;
            }
            num = Math.Min(num, m_playerSoldiers.Count);
            Debug.Log(num);
            for (int i = 0; i < num; ++i)
            {
                m_playerSoldiers[i].MoveToPlanet(target);
            }
            m_playerSoldiers.RemoveRange(0, num);
            m_moveAudioSource.Play();
        }
        if (belong == Belong.ENEMY)
        {
            if (m_enemySoldiers.Count <= 0)
            {
                return;
            }
            num = Math.Min(num, m_enemySoldiers.Count);
            Debug.Log(num);
            for (int i = 0; i < num; ++i)
            {
                m_enemySoldiers[i].MoveToPlanet(target);
            }
            m_enemySoldiers.RemoveRange(0, num);
            m_moveAudioSource.Play();
        }
    }

    // Soldier arrives
    public void Arrive(Soldier s)
    {
        if (s.m_belong == Belong.PLAYER)
        {
            m_playerSoldiers.Add(s);
        }
        if (s.m_belong == Belong.ENEMY)
        {
            m_enemySoldiers.Add(s);
        }
    }

    public void Upgrade(Belong belong)
    {
        if (belong == Belong.PLAYER)
        {
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
        if (belong == Belong.ENEMY)
        {
            int enemySoldierNum = m_enemySoldiers.Count;
            if (enemySoldierNum >= m_level * 10)
            {
                for (int i = 0; i < m_level * 10; ++i)
                {
                    Destroy(m_enemySoldiers[i].gameObject);
                }
                m_enemySoldiers.RemoveRange(0, m_level * 10);
                m_level += 1;
                return;
            }
        }
    }

    public void HighLightPlanet(bool isSelected, bool isTarget)
    {
        Behaviour haloBehavior = (Behaviour)this.gameObject.GetComponent("Halo");
        haloBehavior.enabled = isSelected;

        haloBehavior = (Behaviour)m_haloObject.GetComponent("Halo");
        haloBehavior.enabled = isTarget;
    }

    public bool CanUpgrade(Belong belong)
    {
        if (belong != m_belong)
            return false;
        if (belong == Belong.PLAYER && m_playerSoldiers.Count >= m_level * 10)
        {
            return true;
        }
        if (belong == Belong.ENEMY && m_enemySoldiers.Count >= m_level * 10)
        {
            return true;
        }
        return false;
    }

    IEnumerator waitAddNumber()
    {
        yield return new WaitForSeconds(0.1f);
        m_addNumber = true;
    }

    private void OnMouseDown()
    {
        m_global.GetComponent<MainGlobal>().UpdateHighlight(this.gameObject);

        m_moveNumber = 0;
        m_planetInfoText.GetComponent<PlanetInfo>().SetSelectedPlanet(this);
        m_upgradeButton.GetComponent<UpgradeButton>().SetSelectedPlanet(this);
    }

    private void OnMouseDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Planet planet = hit.transform.gameObject.GetComponent<Planet>();
            //Debug.Log("Choose Target" + m_targetPlanet.gameObject.name);
            if (planet == this && m_addNumber)
            {
                m_addNumber = false;

                m_moveNumber = Math.Min(m_moveNumber + m_moveNumber / 10 + 1, m_playerSoldiers.Count);
                m_numberText.GetComponent<Text>().text = m_moveNumber.ToString();
                StartCoroutine(waitAddNumber());
            }
            else
            {
                m_moveNumber = Math.Min(m_moveNumber, m_playerSoldiers.Count);
            }

        }
        m_numberText.SetActive(true);
        m_numberText.GetComponent<Text>().text = m_moveNumber.ToString();
        if (Input.GetMouseButtonDown(1))
        {
            m_moveNumber = 0;
        }
    }

    private void OnMouseUp()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Planet planet = hit.transform.gameObject.GetComponent<Planet>();
            //Debug.Log("Choose Target" + m_targetPlanet.gameObject.name);
            if (planet != this && 
                Vector3.Distance(planet.transform.position, this.transform.position) < m_maxDistance)
            {
                MoveSoldier(planet, m_moveNumber, Belong.PLAYER);
                m_moveNumber = 0;
                m_global.GetComponent<MainGlobal>().UpdateHighlight(planet.gameObject);
                m_planetInfoText.GetComponent<PlanetInfo>().SetSelectedPlanet(planet);
            }
        }
        m_numberText.SetActive(false);
    }

}
