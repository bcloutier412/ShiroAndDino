using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class GameManager : MonoBehaviour
{
    public GameObject gameOverScreen; // Reference to the game over UI panel
    public Text coinText;
    public GameData GameData; // Reference to the GameData ScriptableObject
    private bool isGameOver = false;

    public PlayerData PlayerData;

    void Start()
    {
        PlayerData.currentHealth = PlayerData.maxHealth;
        gameOverScreen.SetActive(false);
        UpdateCoinDisplay(GameData.totalCoinsCollected); // Initialize display with current coin count
    }

    public void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            gameOverScreen.SetActive(true); // Show the game over screen
            Time.timeScale = 0; // Freeze the game
        }
    }

    public void ShowGameOverScreenAfterDelay(float delay)
    {
        StartCoroutine(ShowGameOverAfterDelay(delay));
    }

    private IEnumerator ShowGameOverAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the animation duration
        GameOver(); // Now show the game over screen
    }

    public void UpdateCoinDisplay(int coinCount)
    {
        if (coinText != null)
        {
            coinText.text = coinCount.ToString();
        }
    }

public void RestartGame()
{
    Debug.Log("RestartGame called");
    Time.timeScale = 1; // Unfreeze the game

    // Reset player stats through the ScriptableObject
    PlayerData.ResetPlayerStats();

        // Optionally update the UI to show the reset health if applicable
        // UpdateHealthDisplay(GameData.currentHealth); // If you have a method for health display

        // Reload the current scene
        SceneManager.LoadScene(1);
        Debug.Log("Scene reloaded");
}




    public void QuitGame() => Application.Quit(); // Quit the game
}
