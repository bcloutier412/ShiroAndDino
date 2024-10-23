using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public GameObject gameOverScreen; // Reference to the game over UI panel
    private bool isGameOver = false;
    public Text coinText;
    void Start()
    {
        gameOverScreen.SetActive(false); 
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


    // In GameManager


public void UpdateCoinDisplay(int coinCount)
{
    if (coinText != null)
    {
        coinText.text = coinCount.ToString();
    }
}



    public void RestartGame()
    {
        Time.timeScale = 1; // Unfreeze the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the game
    }
}
