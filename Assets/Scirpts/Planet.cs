using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type { WATER, FIRE, GRASS };
public enum Belong { PLAYER, ENEMY, NONE };
public class Planet : MonoBehaviour
{
    
    public GameObject soldier;
    HashSet<Soldier> playerSoldiers = new HashSet<Soldier>();
    HashSet<Soldier> enemySoldiers = new HashSet<Soldier>();
    Belong belong = Belong.PLAYER;
    int level = 1;
    float radius = 0.5f;
    // Used to generate soldiers
    private float waitTime = 2.0f;
    private float timer = 0.0f;
    private float visualTime = 0.0f;
    public Type type = Type.FIRE;

    Vector3 randomNormalizedVector()
    {
        return Vector3.Normalize(new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        // Check if we have reached beyond 2 seconds.
        // Subtracting two is more accurate over time than resetting to zero.
        if (timer > waitTime)
        {
            visualTime = timer;
            // Remove the recorded 2 seconds.
            timer = timer - waitTime;

            generateSoldier();

        }
    }

    void generateSoldier()
    {
        // Create a new soldier
        Vector3 randomPosition = randomNormalizedVector() * radius + transform.position;
        GameObject newSoldierObject = Instantiate(soldier, randomPosition, new Quaternion());
        Soldier newSoldier = newSoldierObject.GetComponent<Soldier>();
        newSoldier.initAttribute(this.level, this.type, this.belong, this.gameObject);
        if (belong == Belong.PLAYER)
        {
            playerSoldiers.Add(newSoldier);
        }
        else
        {
            enemySoldiers.Add(newSoldier);
        }
    }
}
