using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public enum SpawnState
    {
        Spawning, Waiting, Counting
    };

    [SerializeField] private List<CharacterStats> enemyList;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float waveCountdown = 0;
    [SerializeField] private Wave[] waves;
    [SerializeField] private Transform[] spawners;

    private SpawnState state = SpawnState.Counting;

    private int currentWave;

    private void Start()
    {
        waveCountdown = timeBetweenWaves;
        currentWave = 0;
    }

    private void Update()
    {
        if(state == SpawnState.Waiting)
        {
            if(!EnemiesAreDead())
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

        if(waveCountdown <= 0)
        {
            if(state != SpawnState.Spawning)
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

        for(int i = 0; i < wave.enemiesAmmount; i++)
        {
            SpawnZombie(wave.enemy);
            yield return new WaitForSeconds(wave.delay);
        }

        state = SpawnState.Waiting;

        yield break;
    }

    public void SpawnZombie(GameObject enemy)
    {
        //print(randomInt);

        int randomInt = Random.Range(1, spawners.Length);
        Transform randomSpawner = spawners[randomInt];
        
        GameObject newEnemy = Instantiate(enemy, randomSpawner.position, randomSpawner.rotation);
        CharacterStats newEnemyStats = newEnemy.GetComponent<CharacterStats>();

        enemyList.Add(newEnemyStats);
    }

    private bool EnemiesAreDead()
    {
        int i = 0;
        foreach(CharacterStats enemy in enemyList)
        {
            if(enemy.IsDead())
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

        if(currentWave + 1 > waves.Length - 1)
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