using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public enum SpawnState
    {
        Spawning, Waiting, Counting
    };

    public int MaxEnemiesToSpawn = 15;

    [SerializeField] private List<CharacterStats> enemyList;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float waveCountdown = 0;
    [SerializeField] private float firstWaveTimer = 10f;
    [SerializeField] private List<Wave> waves;
    [SerializeField] private Transform[] spawners;

    private SpawnState state = SpawnState.Counting;

    private int currentWave;

    private void Start()
    {
        waveCountdown = firstWaveTimer;
        currentWave = 0;
    }

    private void Update()
    {
        if (state == SpawnState.Waiting)
        {
            if (!EnemiesAreDead())
            {
                return;
            }
            else
            {
                // Wave is completed
                CompletedWave();
            }
            //print(EnemiesAreDead());
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.Spawning)
            {
                // Spawn enemies
                StartCoroutine(SpawnWave(waves[currentWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        state = SpawnState.Spawning;
        int enemiesToSpawn = wave.enemiesAmmount;

        while (enemiesToSpawn > 0)
        {
            int maxEnemiesToSpawn = Mathf.Min(enemiesToSpawn, MaxEnemiesToSpawn);
            int numEnemiesToSpawn = Mathf.Min(maxEnemiesToSpawn, wave.enemy.Count);

            for (int i = 0; i < numEnemiesToSpawn; i++)
            {
                GameObject enemyPrefab = wave.enemy[i];
                SpawnZombie(enemyPrefab);
                yield return new WaitForSeconds(wave.delay);
                enemiesToSpawn--;
            }

            // Wait until some enemies are dead before spawning more
            while (CountLivingEnemies() >= maxEnemiesToSpawn && !AllEnemiesDead())
            {
                yield return null;
            }
        }

        state = SpawnState.Waiting;
        yield break;
    }

    private int CountLivingEnemies()
    {
        int count = 0;
        foreach (CharacterStats enemy in enemyList)
        {
            if (!enemy.IsDead())
            {
                count++;
            }
        }
        return count;
    }

    private bool AllEnemiesDead()
    {
        foreach (CharacterStats enemy in enemyList)
        {
            if (!enemy.IsDead())
            {
                return false;
            }
        }
        return true;
    }

    public void SpawnZombie(GameObject enemyPrefab)
    {
        int randomInt = Random.Range(1, spawners.Length);
        Transform randomSpawner = spawners[randomInt];

        GameObject newEnemy = Instantiate(enemyPrefab, randomSpawner.position, randomSpawner.rotation);
        CharacterStats newEnemyStats = newEnemy.GetComponent<CharacterStats>();

        enemyList.Add(newEnemyStats);
    }

    private bool EnemiesAreDead()
    {
        int i = 0;
        foreach (CharacterStats enemy in enemyList)
        {
            if (enemy.IsDead())
            {
                i++;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    private void CompletedWave()
    {
        //print("Wave Completed");
        state = SpawnState.Counting;
        waveCountdown = timeBetweenWaves;

        if (currentWave + 1 > waves.Count - 1)
        {
            currentWave = 0;
            //print("Completed the game, Well Done");
        }
        else
        {
            currentWave++;
        }
    }
}