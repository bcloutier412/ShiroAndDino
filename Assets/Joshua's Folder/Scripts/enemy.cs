using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, Idamageable
{
    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D physicsCollider;
    private SpriteRenderer spriteRenderer;

    public GameObject coinPrefab; // Reference to the coin prefab
    public int numberOfCoins = 1; // Number of coins to drop
    public float bounceForceMin = 2f; // Minimum bounce force for coins
    public float bounceForceMax = 5f; // Maximum bounce force for coins
    private bool isDead = false;

    public TrackableObject trackableObject; // Reference to TrackableObject script

    private Vector2 previousPosition; // To store previous position for direction check

    public PlayerData playerData; // Reference to the PlayerData ScriptableObject

    public float _health = 6;
    public bool _targetable = true;

    public float damageAmount = 1; // Damage dealt to the player upon contact

    public float Health
    {
        set
        {
            _health = value;
            if (_health <= 0 && !isDead) // Only trigger death if not already dead
            {
                isDead = true; // Set the enemy as dead
                animator.SetTrigger("Death");
                Targetable = false;
                HandleDeath(); // Handle physics immediately upon death
                DropCoins(); // Drop coins when enemy dies
                if (trackableObject != null)
                {
                    trackableObject.MarkAsDestroyed(); // Mark the enemy as destroyed
                }
            }
        }
        get { return _health; }
    }

    public bool Targetable
    {
        get { return _targetable; }
        set
        {
            _targetable = value;
            physicsCollider.enabled = value;
        }
    }

    void Start()
    {
        physicsCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Initialize previousPosition with the starting position
        previousPosition = rb.position;

        // Automatically assign the TrackableObject script if not manually set
        if (trackableObject == null)
        {
            trackableObject = GetComponent<TrackableObject>();
        }
    }

    void Update()
    {
        // Calculate movement direction by comparing current and previous positions
        Vector2 movementDirection = rb.position - previousPosition;

        // Flip the sprite based on movement direction
        if (movementDirection.x > 0)
        {
            spriteRenderer.flipX = false; // Facing right
        }
        else if (movementDirection.x < 0)
        {
            spriteRenderer.flipX = true; // Facing left
        }

        // Update the previousPosition for the next frame
        previousPosition = rb.position;
    }

    private void HandleDeath()
    {
        rb.velocity = Vector2.zero; // Stop all movement
        rb.bodyType = RigidbodyType2D.Kinematic; // Make rigidbody non-responsive to physics
        rb.simulated = false; // Disable all physics interactions
        physicsCollider.enabled = false; // Disable physics collider

        Collider2D triggerCollider = gameObject.AddComponent<BoxCollider2D>();
        triggerCollider.isTrigger = true;
    }

    private void DropCoins()
    {
        for (int i = 0; i < numberOfCoins; i++)
        {
            GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);

            // Apply a random bounce force to the coin
            Rigidbody2D coinRb = coin.GetComponent<Rigidbody2D>();
            if (coinRb != null)
            {
                Vector2 bounceDirection = new Vector2(
                    Random.Range(-1f, 1f),
                    Random.Range(0.8f, 1.2f)
                ).normalized;

                float bounceForce = Random.Range(bounceForceMin, bounceForceMax);
                coinRb.AddForce(bounceDirection * bounceForce, ForceMode2D.Impulse);
            }
        }
    }

    public void Defeated()
    {
        Destroy(gameObject);
    }

    public void OnHit(float damage, Vector2 knockback)
    {
        if (isDead) return;
        Health -= damage;
        print("Hit");
        spriteRenderer.color = Color.red;
        StartCoroutine(ResetColor());
        rb.AddForce(knockback);
    }

    IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    public void AttackPlayer(GameObject player)
    {
        if (isDead) return;

        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null && !playerController.isInvincible)
        {
            playerController.StartInvincibility();
            playerController.TakeDamage(damageAmount);
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
    public void OnHit(float damage)
    {
        throw new System.NotImplementedException();
    }
}
