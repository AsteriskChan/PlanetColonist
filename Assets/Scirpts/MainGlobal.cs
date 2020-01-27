using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainGlobal : MonoBehaviour
{
    public PlanetInfo m_planetInfo;
    public GameObject m_winCanvas;
    public GameObject m_lossCanvas;
    List<GameObject> m_planets;

    Planet m_selectedPlanet = null;
    Planet m_targetPlanet = null;

    public GameObject m_upgradeButton;
    // Start is called before the first frame update
    void Start()
    {
        m_planets = new List<GameObject>(GameObject.FindGameObjectsWithTag("Planet"));
        Time.timeScale = 1;
        HideCanvas();
    }

    // Update is called once per frame
    void Update()
    {
        WinOrLoss();
        SelectPlanet();
        ActiveUpgradeButton();
        ChooseTargetPlanet();
        Move();
    }

    void HighLightPlanet(GameObject g, bool highlight)
    {
        Behaviour halo = (Behaviour)g.gameObject.GetComponent("Halo");
        halo.enabled = highlight;
    }

    void SelectPlanet()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.gameObject.name);
                m_selectedPlanet = hit.transform.gameObject.GetComponent<Planet>();
                m_planetInfo.SetSelectedPlanet(m_selectedPlanet);
                HighLightPlanet(m_selectedPlanet.gameObject, true);
                foreach (GameObject g in m_planets)
                {
                    if (g != m_selectedPlanet.gameObject)
                    {
                        HighLightPlanet(g, false);
                    }
                }
            }
        }
    }

    void ChooseTargetPlanet()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {             
                m_targetPlanet = hit.transform.gameObject.GetComponent<Planet>();
                //Debug.Log("Choose Target" + m_targetPlanet.gameObject.name);
            }
        }
    }

    void Move()
    {
        if (m_selectedPlanet && m_targetPlanet && 
            m_selectedPlanet != m_targetPlanet &&
            !Input.GetMouseButton(0))
        {
            Debug.Log("Move" + m_targetPlanet.gameObject.name);
            m_selectedPlanet.MoveSoldier(m_targetPlanet, -1);
            m_selectedPlanet = m_targetPlanet;
        }
    }

    void WinOrLoss()
    {
        bool allBelongToPlayer = true;
        bool allBelongToEnemy = true;
        foreach (GameObject o in m_planets)
        {
            Planet p = o.GetComponent<Planet>();
            if (p.m_belong != Belong.PLAYER)
            {
                allBelongToPlayer = false;
            }
            if (p.m_belong != Belong.ENEMY)
            {
                allBelongToEnemy = false;
            }
        }
        if (allBelongToPlayer)
        {
            ShowWinCanvas();
        }
        if (allBelongToEnemy)
        {
            ShowLossCanvas();
        }
    }

    public void HideCanvas()
    {
        m_winCanvas.SetActive(false);
        m_lossCanvas.SetActive(false);
    }

    public void ShowWinCanvas()
    {
        m_winCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    public void ShowLossCanvas()
    {
        m_lossCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    public void ActiveUpgradeButton()
    {
        if (!m_selectedPlanet || m_selectedPlanet.m_belong != Belong.PLAYER)
        {
            m_upgradeButton.GetComponent<Button>().interactable = false;
            return;
        }
        int level = m_selectedPlanet.m_level;
        int playerSoldierNum = m_selectedPlanet.m_playerSoldiers.Count;
        if (playerSoldierNum >= level * 10)
        {
            m_upgradeButton.GetComponent<Button>().interactable = true;
            return;
        }
    }

    public void UpgradePlanet()
    {
        if (!m_selectedPlanet || m_selectedPlanet.m_belong != Belong.PLAYER)
        {
            return;
        }
        m_selectedPlanet.Upgrade();
    }
}
