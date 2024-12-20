using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMove_Ref : MonoBehaviour
{
    public int sceneBuildIndex;
   // public string entranceName; // Add this to specify the entrance name for each level move
    public Transform teleportTarget; // Add this to specify the teleport position
    public GameData gameData;

    public bool sceneLoader;

    public int coinRequired;
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player") && sceneLoader == false) 
        {
           /*  Set the entrance name in GameManager before switching scenes
            GameManager.lastEntranceName = entranceName;

            // Move to the specified scene
            SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);*/
            
            // Teleport the player to the specified position
            other.transform.position = teleportTarget.position;

        }

        if(other.CompareTag("Player") && gameData.totalCoinsCollected >= coinRequired && sceneLoader == true)
        {

            SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
        }
    }
}
