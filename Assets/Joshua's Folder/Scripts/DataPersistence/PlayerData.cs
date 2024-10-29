using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    
    public float doubleHitWindow = 2.0f; // Time allowed for double hit
    public float doubleHitCooldown = 2.0f; // Cooldown duration
    public float invincibilityDuration = 1.5f;  
    
    public int maxHealth = 10;  
    public int currentHealth; // Add a current health variable

    public Dictionary<string, Vector3> savedPositions = new Dictionary<string, Vector3>();

    public float xPosition;
    public float yPosition;
    public float zPosition;

    // Reset the player stats to their initial values
    public void ResetPlayerStats()
    {
        currentHealth = maxHealth; // Reset current health to maximum
        // Reset any other stats if necessary
    }

    public void SavePlayerPosition(Vector3 position)
{
    xPosition = position.x;
    yPosition = position.y;
    zPosition = position.z;

    PlayerPrefs.SetFloat("PlayerX", xPosition);
    PlayerPrefs.SetFloat("PlayerY", yPosition);
    PlayerPrefs.SetFloat("PlayerZ", zPosition);
    PlayerPrefs.Save();
}

public Vector3 LoadPlayerPosition()
{
    xPosition = PlayerPrefs.GetFloat("PlayerX", 0);
    yPosition = PlayerPrefs.GetFloat("PlayerY", 0);
    zPosition = PlayerPrefs.GetFloat("PlayerZ", 0);
    return new Vector3(xPosition, yPosition, zPosition);
}

}
