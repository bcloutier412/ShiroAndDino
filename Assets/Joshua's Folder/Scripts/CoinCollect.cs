using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    public int coinValue = 1;  // The value of the coin
    private static int totalCoinsCollected = 0;  // Static variable to track the total number of coins collected

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))  // Ensure the object is tagged as Player
        {
            CollectCoin();
        }
    }

    // Collect the coin and increase the player's coin count
    void CollectCoin()
{
    totalCoinsCollected += coinValue;  // Increase the coin count
    Debug.Log("Coins collected: " + totalCoinsCollected);

    // Assuming GameManager has a method to update the coin display
    FindObjectOfType<GameManager>().UpdateCoinDisplay(totalCoinsCollected);

    Destroy(gameObject);  // Destroy the coin object
}

    // Optionally, provide a way to get the total number of coins collected
    public static int GetTotalCoinsCollected()
    {
        return totalCoinsCollected;
    }
}
