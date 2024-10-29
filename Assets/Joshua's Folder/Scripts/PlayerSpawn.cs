using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawn : MonoBehaviour
{
    public Transform door1Spawn;
    public Transform door2Spawn;
    public Transform defaultSpawn;

    private void OnEnable()
    {
        // Subscribe to the scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from the scene loaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Optionally call this here if you need to set the position when the scene loads
        SetPlayerSpawnPosition();
        OnDisable();
    }

    void Start()
    {
        // Set player spawn position at the start of the game
        SetPlayerSpawnPosition();
    }

    void SetPlayerSpawnPosition()
    {
        // Set the player position once based on the entrance.
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = GetSpawnPosition();
        }
    }

    Vector3 GetSpawnPosition()
    {
        // Determine the spawn position based on last entrance
        switch (GameManager.lastEntranceName)
        {
            case "door1":
                return door1Spawn.position;
            case "door2":
                return door2Spawn.position;
            default:
                return defaultSpawn.position;
        }
    }
}
