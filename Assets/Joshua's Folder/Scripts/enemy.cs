using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, Idamageable
{
    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D physicsCollider;
    private SpriteRenderer spriteRenderer;

    public float Health
    {
        set
        {
            _health = value;
            if (_health <= 0)
            {
                animator.SetTrigger("Death");
                Targetable = false;
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

    public float damageAmount = 1; // Amount of damage this enemy does

    void Start()
    {
        physicsCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Defeated()
    {
        Destroy(gameObject);
    }

    public void OnHit(float damage, Vector2 knockback)
    {
        Health -= damage;
        print("Hit");
        spriteRenderer.color = Color.red;
        StartCoroutine(ResetColor());
        rb.AddForce(knockback);
    }

    public void OnHit(float damage)
    {
        Health -= damage;
        spriteRenderer.color = Color.red;
        StartCoroutine(ResetColor());
    }

    IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    public void DestroySelf()
    {
        Defeated();
    }

    public void AttackPlayer(GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null && !playerController.isInvincible)
        {
            playerController.StartInvincibility(); // Trigger invincibility on the player
            playerController.currentHealth -= damageAmount; // Apply damage to the player
            Debug.Log("Player hit by enemy for " + damageAmount + " damage.");
        }
    }
}
