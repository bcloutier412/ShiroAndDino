// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class PlayerSpawn : MonoBehaviour
// {
//     public string defaultSpawnDoorName = "defaultSpawn"; // Default spawn door name if no match is found

//     public PlayerData playerData; 

//     private void OnEnable()
//     {
//         SceneManager.sceneLoaded += OnSceneLoaded;
//     }

//     private void OnDisable()
//     {
//         SceneManager.sceneLoaded -= OnSceneLoaded;
//     }

//     private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//     {
//         if (playerData.finishedLoadingData == true) {
//             Debug.Log("SETTING POSITION TO SPAWN");
//             SetPlayerSpawnPosition();
//         }
//     }

//     void Start()
//     {
//          if (playerData.finishedLoadingData == true) {
//         SetPlayerSpawnPosition();
//          }
//     }

// void SetPlayerSpawnPosition()
// {
//     GameObject player = GameObject.FindWithTag("Player");
//     if (player != null)
//     {
//         Vector3 spawnPosition;
//         if (playerData.IsContinuingGame())
//         {
//             //spawnPosition = playerData.LoadPlayerPosition();
//              spawnPosition = GetSpawnPosition();
//         }
//         else
//         {
//             spawnPosition = GetSpawnPosition();
//         }
//         player.transform.position = spawnPosition;
//     }
// }



//     Vector3 GetSpawnPosition()
//     {
//         string lastEntranceName = GameManager.lastEntranceName; // Keep as string
//         GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnTag");

//         foreach (GameObject spawnPoint in spawnPoints)
//         {
//             if (spawnPoint.name == lastEntranceName) // Compare names as strings
//             {
//                 return spawnPoint.transform.position;
//             }
//         }

//         // If no matching spawn point is found, use the default spawn
//         GameObject defaultSpawnPoint = GameObject.Find(defaultSpawnDoorName);
//         if (defaultSpawnPoint != null)
//         {
//             Debug.LogWarning("Default spawn point");
//             return defaultSpawnPoint.transform.position; // Return the position of the default spawn point
    
//         }
//         else
//         {
//             Debug.LogWarning("No matching spawn point found, and default spawn point is also missing.");
//             return Vector3.zero; // Return Vector3.zero if no spawn points are found
//         }
//     }


// }
