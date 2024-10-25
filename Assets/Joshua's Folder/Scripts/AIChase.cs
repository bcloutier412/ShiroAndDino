using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChase : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float stoppingDistance = 1.0f;
    public float attackRange = 0.8f;
    public float chaseRange = 8;

    private Animator animator;
    private float distance;
    private Enemy enemy;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>(); // Get reference to Enemy component
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (player == null)
        {
            player = GameObject.FindWithTag("Player"); // Ensure player is assigned
        }
    }

void Update()
{
    // Calculate the distance and direction to the player
    distance = Vector2.Distance(transform.position, player.transform.position);
    Vector2 direction = (player.transform.position - transform.position).normalized;

    // If within chase range, move toward the player
    if (distance < chaseRange && distance > stoppingDistance)
    {
    

        // Move towards the player
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        // Update X and Y parameters in the animator
        animator.SetFloat("X", direction.x);
        animator.SetFloat("Y", direction.y);
        animator.SetBool("IsWalking", true);
        // Debug to confirm values are being passed
        Debug.Log($"X: {direction.x}, Y: {direction.y}");
    }
    else
    {
        // Stop walking animation
        animator.SetBool("IsWalking", false);

        // Attack if in range
        if (distance <= attackRange)
        {
            enemy.AttackPlayer(player);
        }
    }

    // Check if death animation is triggered
    if (animator.GetBool("Death"))
    {
        speed = 0;
    }
}


}
