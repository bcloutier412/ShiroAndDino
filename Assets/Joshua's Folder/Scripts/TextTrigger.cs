using UnityEngine;
using UnityEngine.UI;

public class TextTrigger : MonoBehaviour
{
    public Text uiText; // Reference to the UI Text element
    public string message; // The message to display
    public Image uiImage; // Reference to the UI Image element

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
        }
    }
}