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
        m_numberText.transform.position = Input.mousePosition;
    }

    public void UpdateHighlight(GameObject selectedPlanet)
    {
        foreach (GameObject g in m_planets)
        {
            if (g == selectedPlanet.gameObject)
            {
                g.GetComponent<Planet>().HighLightPlanet(true, false);
            }
            else if (Vector3.Distance(g.transform.position, selectedPlanet.transform.position) < m_maxDistance)
            {
                g.GetComponent<Planet>().HighLightPlanet(false, true);
            }
            else
            {
                g.GetComponent<Planet>().HighLightPlanet(false, false);
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

}
