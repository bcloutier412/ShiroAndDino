using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform teleportTarget; // Assign a Transform in the Inspector

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
        {
            Debug.LogError("Player not found. Ensure the player GameObject is tagged as 'Player'.");
        }
    }

    private void OnTriggerEnter(Collider other) // Trigger when entering the collider
    {
        Debug.Log("OnTriggerEnter called with: " + other.gameObject.name);
        if (other.gameObject == player.gameObject)
        {
            if (teleportTarget != null)
            {
                Debug.Log("Inside Trigger: " + other.gameObject.name);
                Debug.Log("Teleporting player to target: " + teleportTarget.position);

                // Teleport the player to the target's position
                other.transform.position = teleportTarget.position;

                // Reset the player's velocity if it has a Rigidbody component
                Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
                if (playerRigidbody != null)
                {
                    playerRigidbody.velocity = Vector3.zero;
                    playerRigidbody.angularVelocity = Vector3.zero;
                }
            }
            else
            {
                Debug.LogWarning("Teleport target is not assigned.");
            }
        }
    }
}