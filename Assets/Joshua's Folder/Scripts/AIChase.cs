using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChase : MonoBehaviour
{
    public GameObject player;              // The player the slime is chasing
    public float speed;                    // Movement speed of the slime
    public float stoppingDistance = 1.0f;  // Minimum distance to stop before the player
    public float attackRange = 0.8f;       // Distance within which the slime can attack the player
    public float attackCooldown = 1.5f;    // Time between attacks

    private Animator animator;             // Animator for the slime
    private float distance;
    private bool canAttack = true;         // To track if the slime can attack

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Calculate the distance between the slime and the player
        distance = Vector2.Distance(transform.position, player.transform.position);

        // Determine the direction toward the player
        Vector2 direction = (player.transform.position - transform.position).normalized;

        // Move toward the player if distance is greater than the stopping distance
        if (distance < 4 && distance > stoppingDistance)
        {
            // Set the walking animation
            animator.SetBool("IsWalking", true);

            // Move the slime toward the player
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else
        {
            // Stop walking if within stopping distance
            animator.SetBool("IsWalking", false);

            // Check if the player is within attack range and if the slime can attack
            if (distance <= attackRange && canAttack)
            {
                StartCoroutine(AttackPlayer());
            }
        }

        // If the slime is dead, stop movement entirely
        if (animator.GetBool("Death") == true)
        {
            speed = 0;
        }
    }

    IEnumerator AttackPlayer()
    {
        canAttack = false;  // Disable further attacks during cooldown

        // Trigger attack animation or damage logic here
        Debug.Log("Slime attacks player!");
        animator.SetTrigger("Attack");

        // Add damage logic to the player here, e.g., call a method on the player's health script
        //player.GetComponent<PlayerHealth>().TakeDamage();

        // Wait for the cooldown time before allowing another attack
        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;  // Re-enable attacking after cooldown
    }
}
