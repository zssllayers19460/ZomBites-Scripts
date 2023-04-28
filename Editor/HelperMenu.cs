using UnityEditor;
using UnityEngine;

public static class HelperMenu
{
    [MenuItem("MyMenu/Add Zombie")]
public static void AddZombie()
{
    GameObject enemy = GameObject.FindGameObjectWithTag("Enemy"); // find a GameObject with the "enemy" tag
    if (enemy != null)
    {
        EnemySpawner spawner = GameObject.FindObjectOfType<EnemySpawner>(); // find the EnemySpawner component in the scene
        if (spawner != null)
        {
            spawner.SpawnZombie(enemy); // call the SpawnZombie method with the enemy parameter
        }
    }
}
}