using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public int maxHealth = 6;             // Maximum health the player can have (e.g., 6 for 3 full hearts)
    public int currentHealth;             // Player's current health
    public Image[] hearts;                // Array of Image components for hearts
    public Sprite fullHeart;              // Full heart sprite
    public Sprite halfHeart;              // Half heart sprite (optional)
    public Sprite emptyHeart;             // Empty heart sprite

    void Start()
    {
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            int heartValue = Mathf.Clamp(currentHealth - (i * 2), 0, 2);

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
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }
}
