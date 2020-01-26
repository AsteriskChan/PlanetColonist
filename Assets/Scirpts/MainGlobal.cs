using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGlobal : MonoBehaviour
{
    public PlanetInfo m_planetInfo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SelectPlanet();
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
                m_planetInfo.SetSelectedPlanet(hit.transform.gameObject.GetComponent<Planet>());
            }
        }

    }
}
