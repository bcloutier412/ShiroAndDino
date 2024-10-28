using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChase : MonoBehaviour
{
    public float speed;
    public float stoppingDistance = 1.0f;
    public float attackRange = 0.8f;
    public float chaseRange = 8.0f;

    private GameObject player;
    private Animator animator;
    private float distance;
    private Enemy enemy;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>(); 
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Find and assign the player clone in the scene
        SetPlayerReference();
    }

    void Update()
    {
        if (player == null)
        {
            SetPlayerReference(); // Continuously check if the player reference is lost
            return;
        }

        // Calculate the distance and direction to the player
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = (player.transform.position - transform.position).normalized;

        if (distance < chaseRange && distance > stoppingDistance)
        {
            // Move towards the player
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

            animator.SetFloat("X", direction.x);
            animator.SetFloat("Y", direction.y);
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);

            if (distance <= attackRange)
            {
                enemy.AttackPlayer(player);
            }
        }

        if (animator.GetBool("Death"))
        {
            speed = 0;
        }
    }

    private void SetPlayerReference()
    {
        // Attempt to find the player clone with the "Player" tag in the active scene
        player = GameObject.FindWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("No player instance found with the 'Player' tag.");
        }
    }
}
