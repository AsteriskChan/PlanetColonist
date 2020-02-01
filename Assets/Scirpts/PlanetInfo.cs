using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetInfo : MonoBehaviour
{
    private Planet m_selectedPlanet = null;
    private Text m_text;

    // Start is called before the first frame update
    void Start()
    {
        m_text = this.gameObject.GetComponent<Text>();
        // m_text.text = "Type:\nBelonging:\nLevel:\nEnemy Soldiers:\nPlayer Soldiers:\n";
        m_text.text = "Belonging:\nLevel:\nEnemy Soldiers:\nPlayer Soldiers:\n";
    }

    // Update is called once per frame
    void Update()
    {
        if (m_selectedPlanet)
        {
            string typeStr = "";
            string belongStr = "";
            switch (m_selectedPlanet.m_type)
            {
                case Type.FIRE:
                    typeStr = "Fire";
                    break;
                case Type.WATER:
                    typeStr = "Water";
                    break;
                case Type.GRASS:
                    typeStr = "Grass";
                    break;
            }
            switch (m_selectedPlanet.m_belong)
            {
                case Belong.ENEMY:
                    belongStr = "Enemy";
                    break;
                case Belong.PLAYER:
                    belongStr = "Player";
                    break;
                case Belong.NONE:
                    belongStr = "None";
                    break;
            }
            m_text.text = /*"Type:" + typeStr + "\n" + */
                          "Belonging:" + belongStr + "\n" +
                          "Level:" + m_selectedPlanet.m_level.ToString() + "\n" +
                          "Enemy Soldiers:" + m_selectedPlanet.m_enemySoldiers.Count.ToString() + "\n" +
                          "Player Soldiers:" + m_selectedPlanet.m_playerSoldiers.Count.ToString() + "\n";
        }
    }

    public void SetSelectedPlanet(Planet planet)
    {
        m_selectedPlanet = planet;
    }
}
