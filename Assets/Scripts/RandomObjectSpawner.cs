using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectSpawner : MonoBehaviour
{
    public GameObject[] enemies;

    public float spawnRate = 5f;
    // Update is called once per frame
    void Update()
    {
        if (Time.timeSinceLevelLoad > spawnRate)
        {
            spawnRate = Time.timeSinceLevelLoad + Random.Range(1f, 2f);
            int randomEnemy = Random.Range(0, enemies.Length);
            Vector3 randomPosition = new Vector3(Random.Range(-10, 35), 5, Random.Range(-3 , 44));
            Instantiate(enemies[randomEnemy], randomPosition, Quaternion.identity);
        }

    }
}
