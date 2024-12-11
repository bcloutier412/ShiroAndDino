using UnityEngine;
using UnityEngine.UI;

public class Boss_Music_Script : MonoBehaviour
{
    public Text uiText; // Reference to the UI Text element
    public string message; // The message to display
    public Image uiImage; // Reference to the UI Image element
    public AudioSource bossMusic; // Reference to the AudioSource component for boss music
    public AudioSource otherMusic; // Reference to the other AudioSource that plays on awake

    private void Start()
    {
        if (uiText != null)
        {
            uiText.gameObject.SetActive(false); // Hide the text box initially
        }

        if (uiImage != null)
        {
            uiImage.gameObject.SetActive(false); // Hide the image initially
        }

        if (bossMusic != null)
        {
            bossMusic.Stop(); // Ensure the boss music is stopped initially
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (uiText != null)
            {
                uiText.text = message; // Set the message
                uiText.gameObject.SetActive(true); // Show the text box
            }

            if (uiImage != null)
            {
                uiImage.gameObject.SetActive(true); // Show the image
            }

            if (bossMusic != null)
            {
                bossMusic.Play(); // Play the boss music
            }

            if (otherMusic != null)
            {
                otherMusic.Stop(); // Stop the other music
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (uiText != null)
            {
                uiText.gameObject.SetActive(false); // Hide the text box when the player exits the collider
            }

            if (uiImage != null)
            {
                uiImage.gameObject.SetActive(false); // Hide the image when the player exits the collider
            }

            if (bossMusic != null)
            {
                bossMusic.Stop(); // Stop the boss music
            }

            if (otherMusic != null)
            {
                otherMusic.Play(); // Restart the other music if needed
            }
        }
    }
}