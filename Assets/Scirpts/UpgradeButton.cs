using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    private Planet m_planet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_planet && m_planet.CanUpgrade(Belong.PLAYER))
        {
            this.gameObject.GetComponent<Button>().interactable = true;
        }
        else
        {
            this.gameObject.GetComponent<Button>().interactable = false;
        }
    }

    public void Upgrade()
    {
        if (m_planet && m_planet.CanUpgrade(Belong.PLAYER))
        {
            m_planet.Upgrade(Belong.PLAYER);
        }
    }

    public void SetSelectedPlanet(Planet p)
    {
        m_planet = p;
    }


}
