using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGlobal : MonoBehaviour
{
    public PlanetInfo m_planetInfo;
    List<GameObject> m_planets;

    Planet m_selectedPlanet = null;
    Planet m_targetPlanet = null;
    // Start is called before the first frame update
    void Start()
    {
        m_planets = new List<GameObject>(GameObject.FindGameObjectsWithTag("Planet"));
    }

    // Update is called once per frame
    void Update()
    {
        SelectPlanet();
        ChooseTargetPlanet();
        Move();
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
}
