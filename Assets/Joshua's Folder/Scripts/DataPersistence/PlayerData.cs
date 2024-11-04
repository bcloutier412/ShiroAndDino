using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;

    public float doubleHitWindow = 2.0f;
    public float doubleHitCooldown = 2.0f;
    public float invincibilityDuration = 1.5f;

    public int maxHealth = 10;
    public int currentHealth;

    // Store the original start position
    public Vector3 originalPosition = new Vector3(2.6f, -2.5f, 0f);

    //public Dictionary<string, Vector3> savedPositions = new Dictionary<string, Vector3>();

   public Vector3 savePosition;
    public int sceneIndex;

    public bool finishedLoadingData = false;


    // Reset stats and position
    public void ResetPlayerStats()
    {
        currentHealth = maxHealth;
        SavePlayerPosition(originalPosition);  // Reset position to the original
    }

public void SavePlayerPosition(Vector3 position)
{
    // Save the position directly to the savePosition variable
    savePosition = position;
    Debug.Log($"Position saved: {savePosition}");
}

public Vector3 LoadPlayerPosition()
{
    // Load the position from savePosition
    Debug.Log($"Position loaded: {savePosition}");
    return savePosition;
}


public void SaveGameStatus(bool isContinuing)
{
    PlayerPrefs.SetInt("isContinuing", isContinuing ? 1 : 0);
    PlayerPrefs.Save();
}

public bool IsContinuingGame()
{
    return PlayerPrefs.GetInt("isContinuing", 0) == 1;
}


public void SaveSceneIndex(int index)
{
    sceneIndex = index;
    //PlayerPrefs.SetInt("LastSceneIndex", sceneIndex);
    //PlayerPrefs.Save();
}

public int LoadSceneIndex()
{
   // return PlayerPrefs.GetInt("LastSceneIndex", sceneIndex); // Default to 0 (main menu) if no scene index is found
   return sceneIndex;
}


}
