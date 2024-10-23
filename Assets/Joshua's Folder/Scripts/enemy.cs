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
            }
        }
        get
        {
            return _health;
        }
    }

    public bool Targetable
    {
        get
        {
            return _targetable;
        }
        set
        {
            _targetable = value;
            physicsCollider.enabled = value;
        }
    }

    public float _health = 6;
    public bool _targetable = true;

    public int damageAmount = 1; // Amount of damage this enemy does

    void Start()
    {
        physicsCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void HandleDeath()
    {
        // Stop physics immediately when death occurs
        rb.velocity = Vector2.zero; // Stop all movement
        rb.bodyType = RigidbodyType2D.Kinematic; // Make rigidbody non-responsive to physics
        rb.simulated = false; // Disable all physics interactions

        // Disable the physics collider to prevent collisions
        physicsCollider.enabled = false; 

        // Optionally, switch to a trigger collider for post-death interactions (e.g., animation)
        Collider2D triggerCollider = gameObject.AddComponent<BoxCollider2D>();
        triggerCollider.isTrigger = true;
    }

    // Drop coins when enemy is defeated
    private void DropCoins()
    {
        for (int i = 0; i < numberOfCoins; i++)
        {
            // Instantiate the coin at the enemy's position
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }
    }

    public void Defeated()
    {
        Destroy(gameObject);
    }

    public void OnHit(float damage, Vector2 knockback)
    {
        if (isDead) return;  // Prevent further damage if already dead
        Health -= damage;
        print("Hit");
        spriteRenderer.color = Color.red;
        StartCoroutine(ResetColor());
        rb.AddForce(knockback);
    }

    public void OnHit(float damage)
    {
        if (isDead) return;  // Prevent further damage if already dead
        Health -= damage;
        spriteRenderer.color = Color.red;
        StartCoroutine(ResetColor());
    }

    IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    public void AttackPlayer(GameObject player)
    {
        if (isDead) return; // Exit if enemy is dead

        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null && !playerController.isInvincible)
        {
            playerController.StartInvincibility(); // Trigger invincibility on the player
            playerController.TakeDamage(damageAmount); // Apply damage to the player
        }
    }

    public void DestroySelf()
    {
        throw new System.NotImplementedException();
    }
}
