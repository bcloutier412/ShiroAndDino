using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    public int coinValue = 1;  // The value of the coin
    public GameData coinData;  // Reference to the CoinData ScriptableObject
    public TrackableObject trackableObject; // Reference to the TrackableObject ScriptableObject

    public AudioClip coinCollectSound; // Reference to the coin collect sound
    private AudioSource audioSource; // Reference to the AudioSource component

    private Rigidbody2D rb;
    private bool isBouncing = true;

    // Parameters for the bounce
    public float initialBounceForce = 5f;  // Initial force applied to the coin when it spawns
    public float bounceDamping = 0.9f;  // Damping factor for bounce
    public float minBounceThreshold = 0.1f;  // Minimum velocity required to keep bouncing

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        // Apply an initial random bounce force when the coin spawns
        Vector2 bounceDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(0.8f, 1.2f)).normalized;
        rb.AddForce(bounceDirection * initialBounceForce, ForceMode2D.Impulse);

        // Debug logs to check AudioSource and AudioClip
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing or not assigned.");
        }
        if (coinCollectSound == null)
        {
            Debug.LogError("coinCollectSound AudioClip is not assigned.");
        }
    }

    void Update()
    {
        // Stop bouncing when the coin's velocity is very low
        if (isBouncing && rb.velocity.magnitude < minBounceThreshold)
        {
            rb.velocity = Vector2.zero;
            isBouncing = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D called with: " + other.gameObject.name);
        if (other.CompareTag("Player"))  // Ensure the object is tagged as Player
        {
            CollectCoin();
        }
    }

    // Collect the coin and increase the player's coin count
    void CollectCoin()
    {
        // Use the ScriptableObject to track total coins
        coinData.totalCoinsCollected += coinValue;  
        Debug.Log("Coins collected: " + coinData.totalCoinsCollected);

        if (audioSource != null && coinCollectSound != null)
        {
            Debug.Log("Playing coin collect sound");
            audioSource.pitch = Random.Range(0.95f, 1.05f); // Set the pitch to a random value between 0.95 and 1.05
            audioSource.PlayOneShot(coinCollectSound);
        }
        else
        {
            Debug.LogWarning("AudioSource or coinCollectSound is null");
        }

        // Call the MarkAsDestroyed method to track the coin as destroyed
        if (trackableObject != null)
        {
            trackableObject.MarkAsDestroyed();
        }

        // Assuming GameManager has a method to update the coin display
        FindObjectOfType<GameManager>().UpdateCoinDisplay(coinData.totalCoinsCollected);

        Destroy(gameObject);  // Destroy the coin object
    }
}