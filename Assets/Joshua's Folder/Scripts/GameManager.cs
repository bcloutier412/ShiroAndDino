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

    public GameObject pauseMenuScreen;

    public static string lastEntranceName;
    private bool isPaused = false;


    void Start()
    {
        //PlayerData.currentHealth = PlayerData.maxHealth;
        gameOverScreen.SetActive(false);
        UpdateCoinDisplay(GameData.totalCoinsCollected); // Initialize display with current coin count
    }

    void Update()
    {
        // Toggle pause state when pressing Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
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


public void Resume()
    {
        pauseMenuScreen.SetActive(false);
        Time.timeScale = 1f;  // Resume the game
        isPaused = false;
    }


    void Pause()
    {
        pauseMenuScreen.SetActive(true);
        Time.timeScale = 0f;  // Freeze the game
        isPaused = true;
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
        SceneManager.LoadScene(2);
        Debug.Log("Scene reloaded");
}

public void MainMenu()
    {
        Time.timeScale = 1f;  // Ensure time is reset
        SceneManager.LoadScene(0);  // Load the main menu scene (replace with the actual scene name)
    }

    public void QuitGame() => Application.Quit(); // Quit the game

public static void SetLastEntrance(string entranceName)
    {
        lastEntranceName = entranceName;
    }

}
