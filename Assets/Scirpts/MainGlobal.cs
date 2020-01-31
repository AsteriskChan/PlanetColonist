using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainGlobal : MonoBehaviour
{
    public PlanetInfo m_planetInfo;
    public GameObject m_winCanvas;
    public GameObject m_lossCanvas;
    public GameObject m_numberText;
    List<GameObject> m_planets;

    Planet m_selectedPlanet = null;
    public float m_maxDistance = 30;

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
        ActiveUpgradeButton();
        m_numberText.transform.position = Input.mousePosition;
    }

    void HighLightPlanet(GameObject g, bool highlight, Color c)
    {
        //Behaviour haloBehavior = (Behaviour)g.gameObject.GetComponent("Halo");
        //haloBehavior.enabled = highlight;
        SerializedObject halo = new SerializedObject(g.GetComponent("Halo"));
        //halo.FindProperty("m_Size").floatValue += 3f;
        halo.FindProperty("m_Enabled").boolValue = highlight;
        halo.FindProperty("m_Color").colorValue = c;
        halo.ApplyModifiedProperties();
    }

    public void UpdateHighlight(GameObject selectedPlanet)
    {
        foreach (GameObject g in m_planets)
        {
            if (g == selectedPlanet.gameObject)
            {
                continue;
            }
            if (Vector3.Distance(g.transform.position, selectedPlanet.transform.position) < m_maxDistance)
            {
                HighLightPlanet(g, true, Color.yellow);
            }
            else
            {
                HighLightPlanet(g, false, Color.white);
            }
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
