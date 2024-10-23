using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;     // Array of different enemy prefabs to spawn
    public Transform[] spawnPoints;       // Array of possible spawn locations
    public float initialSpawnInterval = 5f;  // Initial time interval between enemy spawns
    public int maxEnemies = 10;           // Maximum number of enemies that can be in the scene at once

    private float spawnInterval;          // Current spawn interval (will decrease over time)
    private int currentEnemyCount = 0;    // Keeps track of the current number of enemies

    private void Start()
    {
        spawnInterval = initialSpawnInterval; // Set the spawn interval to its initial value
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (currentEnemyCount < maxEnemies)
            {
                // Choose a random spawn point
                int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
                Transform spawnPoint = spawnPoints[randomSpawnIndex];

                // Choose a random enemy type from the array of enemy prefabs
                int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
                GameObject enemyPrefab = enemyPrefabs[randomEnemyIndex];

                // Instantiate the chosen enemy at the chosen spawn point
                Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

                // Increment the enemy count
                currentEnemyCount++;

                // Optionally decrease the spawn interval over time to increase difficulty
                spawnInterval = Mathf.Max(spawnInterval - 0.1f, 1f); // Ensure it doesn't go below 1 second
            }
        }
    }

    // Call this method when an enemy is destroyed
    public void OnEnemyDestroyed()
    {
        currentEnemyCount--;
        currentEnemyCount = Mathf.Clamp(currentEnemyCount, 0, maxEnemies); // Prevent negative counts
    }
}
