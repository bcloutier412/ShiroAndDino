using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Range : MonoBehaviour, Idamageable
{
    private Animator animator;
    public float speed;
    public float stoppingDistance;
    public float retreatDistance;

    private float timeBtwShots;
    public float startTimeBtwShots;

    public GameObject projectile;

    public Transform player;

    public GameObject coinPrefab; // Reference to the coin prefab
    public int numberOfCoins = 1; // Number of coins to drop
    public float bounceForceMin = 2f; // Minimum bounce force for coins
    public float bounceForceMax = 5f; // Maximum bounce force for coins
    private bool isDead = false;

    private Rigidbody2D rb;
    private Collider2D physicsCollider;
    private SpriteRenderer spriteRenderer;

    public PlayerData playerData;

    public float attackDistance = 10f; // Maximum distance for detecting the player

    public float Health
    {
        set
        {
            _health = value;
            if (_health <= 0 && !isDead)
            {
                isDead = true;
                animator.SetTrigger("Death");
                Targetable = false;
                HandleDeath();
                DropCoins();
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
            if (physicsCollider != null)
                physicsCollider.enabled = value;
        }
    }

    public float _health = 6;
    public bool _targetable = true;

    public int damageAmount = 1; // Amount of damage this enemy does

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        timeBtwShots = startTimeBtwShots;
    }

void FixedUpdate()
{
    if (isDead) return;

    float distanceToPlayer = Vector2.Distance(transform.position, player.position);

    // Check if the player is within the agro range
    if (distanceToPlayer <= attackDistance)
    {
        Vector3 direction = (player.position - transform.position).normalized;

        // Movement logic
        if (distanceToPlayer > stoppingDistance)
        {
            // Move toward the player
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            animator.SetBool("IsWalking", true);
        }
        else if (distanceToPlayer < stoppingDistance && distanceToPlayer > retreatDistance)
        {
            // Stop moving
            transform.position = this.transform.position;
            animator.SetBool("IsWalking", false);
        }
        else if (distanceToPlayer < retreatDistance)
        {
            // Move away from the player
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
            animator.SetBool("IsWalking", true);
        }

        // Update Blend Tree parameters
        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);

        // Flip the sprite for left movement
        spriteRenderer.flipX = direction.x < 0;

        // Shooting logic
        if (timeBtwShots <= 0)
        {
            Instantiate(projectile, transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }
    else
    {
        // Stop moving and reset walking animation if player is out of range
        animator.SetBool("IsWalking", false);
    }
}



    private void HandleDeath()
    {
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = false;
        physicsCollider.enabled = false;

        Collider2D triggerCollider = gameObject.AddComponent<BoxCollider2D>();
        triggerCollider.isTrigger = true;
    }

    private void DropCoins()
    {
        for (int i = 0; i < numberOfCoins; i++)
        {
            GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            Rigidbody2D coinRb = coin.GetComponent<Rigidbody2D>();
            if (coinRb != null)
            {
                // Apply random bounce force
                float bounceX = Random.Range(-1f, 1f);
                float bounceY = Random.Range(bounceForceMin, bounceForceMax);
                coinRb.AddForce(new Vector2(bounceX, bounceY), ForceMode2D.Impulse);
            }
        }
    }

    public void OnHit(float damage, Vector2 knockback)
    {
        if (isDead) return;

        Health -= damage;
        spriteRenderer.color = Color.red;
        StartCoroutine(ResetColor());
        rb.AddForce(knockback);
    }

    IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    public void OnHit(float damage)
    {
        throw new System.NotImplementedException();
    }

    public void DestroySelf()
    {
        throw new System.NotImplementedException();
    }

    private void OnDrawGizmosSelected()
{
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, attackDistance);
}


    /*public void AttackPlayer(GameObject player)
    {
        if (isDead) return;

        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null && !playerController.isInvincible)
        {
            playerController.StartInvincibility();
            playerController.TakeDamage(damageAmount);
        }
    }*/
}
