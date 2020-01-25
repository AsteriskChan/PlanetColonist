using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public GameObject planet;
    Transform center;
    float radius = 0.2f;
    float rotationSpeed = 80.0f;
    Vector3 axis = Vector3.up;
    Vector3 desiredPosition;
    float radiusSpeed = 0.5f;

    public int health = 100;
    int attack = 10;
    public int level = 1;
    public Type type;
    public Belong belong; // 0 for player, 1 for opponents

    Vector3 randomNormalizedVector()
    {
        return Vector3.Normalize(new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)));
    }

    // Start is called before the first frame update
    void Start()
    {
        center = planet.transform;
        radius = planet.transform.localScale.x + 1.0f;
        Vector3 randPosition = randomNormalizedVector();
        Vector3 dis = transform.position - center.position;
        transform.position = (transform.position - center.position).normalized * radius + center.position;
        axis = Vector3.Cross(dis, randPosition);
        axis = Vector3.Normalize(axis);

        // Level
        health += level * 10;
        attack += level * 2;
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(center.position, axis, rotationSpeed * Time.deltaTime);
    }

    public void initAttribute(int level, Type type, Belong belong, GameObject planet)
    {
        this.level = level;
        this.type = type;
        this.belong = belong;
        this.planet = planet;

        health += level * 10;
        attack += level * 2;
    }
}
