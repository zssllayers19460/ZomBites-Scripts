using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int MaxEnemiesToSpawn = 15;

    [SerializeField] private float timeBetweenSpawns = 5f;
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private Transform[] spawners;

    private List<CharacterStats> enemyList = new List<CharacterStats>(); // Initialize the list
    private float spawnCountdown = 0f;
    private bool isSpawning = false;

    private void Start()
    {
        spawnCountdown = timeBetweenSpawns;
    }

    private void Update()
    {
        if (!isSpawning && spawnCountdown <= 0)
        {
            StartCoroutine(SpawnEnemies());
        }

        spawnCountdown -= Time.deltaTime;
    }

    private IEnumerator SpawnEnemies()
    {
        isSpawning = true;

        int enemiesToSpawn = Random.Range(1, MaxEnemiesToSpawn + 1);

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
            Transform randomSpawner = spawners[Random.Range(0, spawners.Length)];

            GameObject newEnemy = Instantiate(enemyPrefab, randomSpawner.position, randomSpawner.rotation);
            CharacterStats newEnemyStats = newEnemy.GetComponent<CharacterStats>();

            enemyList.Add(newEnemyStats);

            yield return new WaitForSeconds(0.7f); // Delay between spawning each enemy
        }

        isSpawning = false;
        spawnCountdown = timeBetweenSpawns;
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemyList.Remove(enemy.GetComponent<CharacterStats>());
    }
}