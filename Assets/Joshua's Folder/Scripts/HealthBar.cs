using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PlayerData playerData; // Reference to the PlayerData ScriptableObject
    public Image[] hearts;         // Array of Image components for hearts
    public Sprite fullHeart;       // Full heart sprite
    public Sprite halfHeart;       // Half heart sprite (optional)
    public Sprite emptyHeart;      // Empty heart sprite

    void Start()
    {
        // Initialize current health from PlayerData and update the health bar
        // playerData.currentHealth = playerData.maxHealth; // Ensure health is set at the start
        playerData.maxHealth = playerData.currentHealth;
        UpdateHealthBar();
    }

    void FixedUpdate()
    {
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        Debug.Log("Updating health bar. Current Health: " + playerData.currentHealth);
        for (int i = 0; i < hearts.Length; i++)
        {
            int heartValue = Mathf.Clamp(playerData.currentHealth - (i * 2), 0, 2);

            if (heartValue == 2)
            {
                hearts[i].sprite = fullHeart;  // Display full heart
            }
            else if (heartValue == 1)
            {
                hearts[i].sprite = halfHeart;  // Display half heart (if applicable)
            }
            else
            {
                hearts[i].sprite = emptyHeart; // Display empty heart
            }
        }
    }

    public void TakeDamage(int damage)
    {
        playerData.currentHealth -= damage; // Use the current health from PlayerData
        playerData.currentHealth = Mathf.Clamp(playerData.currentHealth, 0, playerData.maxHealth);
        UpdateHealthBar();
    }

    public void Heal(int healAmount)
    {
        playerData.currentHealth += healAmount; // Use the current health from PlayerData
        playerData.currentHealth = Mathf.Clamp(playerData.currentHealth, 0, playerData.maxHealth);
        UpdateHealthBar();
    }
}