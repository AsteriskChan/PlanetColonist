using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    List<GameObject> m_planets;
    float m_maxDistance = 30;
    // Start is called before the first frame update
    void Start()
    {
        m_planets = new List<GameObject>(GameObject.FindGameObjectsWithTag("Planet"));
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject g0 in m_planets)
        {
            Planet p0 = g0.GetComponent<Planet>();
            if (p0.m_belong != Belong.ENEMY)
            {
                continue;
            }
            if (p0.m_enemySoldiers.Count < 10)
            {
                continue;
            }
            if (p0.m_playerSoldiers.Count > 0)
            {
                continue;
            }
            if (p0.m_enemySoldiers.Count > 30)
            {
                p0.Upgrade(Belong.ENEMY);
                continue;
            }

            // Move
            Vector3 pos0 = p0.transform.position;
            foreach (GameObject g1 in m_planets)
            {
                int num0 = p0.m_enemySoldiers.Count;
                Planet p1 = g1.GetComponent<Planet>();
                Vector3 pos1 = p1.transform.position;
                if (Vector3.Distance(pos0, pos1) > m_maxDistance)
                {
                    continue;
                }
                else if (p1.m_belong == Belong.ENEMY)
                {
                    int num1 = p1.m_enemySoldiers.Count;
                    if (num0 > 2 * num1)
                    {
                        p0.MoveSoldier(p1, (num0 - num1) / 2, Belong.ENEMY);
                        break;
                    }
                }
                else if (p1.m_belong == Belong.NONE)
                {
                    p0.MoveSoldier(p1, num0 / 2, Belong.ENEMY);
                    break;
                }
                else if (p1.m_belong == Belong.PLAYER)
                {
                    int num1 = p1.m_playerSoldiers.Count;
                    if (num0 > num1)
                    {
                        p0.MoveSoldier(p1, num0 - 10, Belong.ENEMY);
                        break;
                    }
                }               
            }
        }
    }
}
