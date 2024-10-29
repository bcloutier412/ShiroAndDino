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

    public Dictionary<string, Vector3> savedPositions = new Dictionary<string, Vector3>();

    public float xPosition;
    public float yPosition;
    public float zPosition;


    // Reset stats and position
    public void ResetPlayerStats()
    {
        currentHealth = maxHealth;
        SavePlayerPosition(originalPosition);  // Reset position to the original
    }

    public void SavePlayerPosition(Vector3 position)
    {
        PlayerPrefs.SetFloat("PlayerX", position.x);
        PlayerPrefs.SetFloat("PlayerY", position.y);
        PlayerPrefs.SetFloat("PlayerZ", position.z);
        PlayerPrefs.Save();
    }

    public Vector3 LoadPlayerPosition()
    {
        return new Vector3(
            PlayerPrefs.GetFloat("PlayerX", originalPosition.x),
            PlayerPrefs.GetFloat("PlayerY", originalPosition.y),
            PlayerPrefs.GetFloat("PlayerZ", originalPosition.z)
        );
    }
}
