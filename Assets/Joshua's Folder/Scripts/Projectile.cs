using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public int damageAmount = 1; // Amount of damage this projectile deals

    private Transform player;
    private Vector2 target;

    void Start()
    {
        // Find the player and set the target position
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player != null)
        {
            target = new Vector2(player.position.x, player.position.y);

            // Calculate direction and rotate arrow to face it
            Vector2 direction = target - (Vector2)transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void Update()
    {
        // Move the projectile towards the target
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Destroy the projectile if it reaches the target
        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            DestroyProjectile();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the projectile hits the player
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damageAmount);
            }

            DestroyProjectile();
        }
        else if (other.CompareTag("Obstacle")) // Optional: destroy projectile on other collisions
        {
            DestroyProjectile();
        }
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
