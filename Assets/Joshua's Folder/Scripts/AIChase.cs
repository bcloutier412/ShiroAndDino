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

    void Start()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>(); // Get reference to Enemy component
    }

    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = (player.transform.position - transform.position).normalized;

        if (distance < chaseRange && distance > stoppingDistance)
        {
            animator.SetBool("IsWalking", true);
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("IsWalking", false);

            if (distance <= attackRange)
            {
                enemy.AttackPlayer(player); // Call the attack method from Enemy script
            }
        }

        if (animator.GetBool("Death") == true)
        {
            speed = 0;
        }
    }
}
