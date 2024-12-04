using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    public int coinValue = 1;  // The value of the coin
    public GameData coinData;  // Reference to the CoinData ScriptableObject
    public TrackableObject trackableObject; // Reference to the TrackableObject ScriptableObject

    private Rigidbody2D rb;
    private bool isBouncing = true;

    // Parameters for the bounce
    public float initialBounceForce = 5f;  // Initial force applied to the coin when it spawns
    public float bounceDamping = 0.9f;  // Damping factor for bounce
    public float minBounceThreshold = 0.1f;  // Minimum velocity required to keep bouncing

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Apply an initial random bounce force when the coin spawns
        Vector2 bounceDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(0.8f, 1.2f)).normalized;
        rb.AddForce(bounceDirection * initialBounceForce, ForceMode2D.Impulse);
    }

    void Update()
    {
        // Stop bouncing when the coin's velocity is very low
        if (isBouncing && rb.velocity.magnitude < minBounceThreshold)
        {
            isBouncing = false;
            rb.velocity = Vector2.zero; // Stop movement when velocity is low
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Reverse the Y velocity to simulate bouncing and apply damping
        if (isBouncing)
        {
            Vector2 velocity = rb.velocity;
            velocity.y = Mathf.Abs(velocity.y) * bounceDamping;  // Reverse and dampen the Y velocity
            rb.velocity = velocity;

            // Stop bouncing if the Y velocity becomes too small
            if (Mathf.Abs(rb.velocity.y) < minBounceThreshold)
            {
                isBouncing = false;
                rb.velocity = Vector2.zero; // Ensure it stops moving
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
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
