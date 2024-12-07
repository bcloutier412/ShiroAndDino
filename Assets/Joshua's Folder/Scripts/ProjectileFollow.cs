using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFollow : MonoBehaviour
{
    public float speed;
    public int damageAmount = 1; // Amount of damage this projectile deals
    public float homingDuration = 2.0f; // Duration for which the projectile will home in on the player

    private Transform player;
    private Vector2 target;
    private bool isHoming = true;

    void Start()
    {
        // Find the player
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player != null)
        {
            // Start the homing coroutine
            StartCoroutine(HomingCoroutine());
        }
    }

    void Update()
    {
        if (isHoming && player != null)
        {
            // Update the target position to the player's current position
            target = new Vector2(player.position.x, player.position.y);

            // Calculate direction and rotate arrow to face it
            Vector2 direction = target - (Vector2)transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        // Move the projectile towards the target
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Destroy the projectile if it reaches the target
        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator HomingCoroutine()
    {
        // Homing for the specified duration
        yield return new WaitForSeconds(homingDuration);
        // Stop homing after the duration
        isHoming = false;
    }
}