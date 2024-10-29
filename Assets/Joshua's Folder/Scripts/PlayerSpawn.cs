using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawn : MonoBehaviour
{
    public string defaultSpawnDoorName = "defaultSpawn"; // Default spawn door name if no match is found

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetPlayerSpawnPosition();
    }

    void Start()
    {
        SetPlayerSpawnPosition();
    }

    void SetPlayerSpawnPosition()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3 spawnPosition = GetSpawnPosition();
            Debug.Log($"Setting Player Position: {spawnPosition}");
            player.transform.position = spawnPosition;
        }
        else
        {
            Debug.LogWarning("Player GameObject not found!");
        }
    }


    Vector3 GetSpawnPosition()
    {
        string lastEntranceName = GameManager.lastEntranceName; // Keep as string
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnTag");

        foreach (GameObject spawnPoint in spawnPoints)
        {
            if (spawnPoint.name == lastEntranceName) // Compare names as strings
            {
                return spawnPoint.transform.position;
            }
        }

        // If no matching spawn point is found, use the default spawn
        GameObject defaultSpawnPoint = GameObject.Find(defaultSpawnDoorName);
        if (defaultSpawnPoint != null)
        {
            return defaultSpawnPoint.transform.position; // Return the position of the default spawn point
        }
        else
        {
            Debug.LogWarning("No matching spawn point found, and default spawn point is also missing.");
            return Vector3.zero; // Return Vector3.zero if no spawn points are found
        }
    }


}
