using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, Idamageable
{
    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D physicsCollider;
    private SpriteRenderer spriteRenderer;  // Added to handle color changes

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

    void Start()
    {
        physicsCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();  // Initialize SpriteRenderer
    }

    public void Defeated()
    {
        Destroy(gameObject);
    }

    public void OnHit(float damage, Vector2 knockback)
    {
        Health -= damage;
        print("Hit");

        // Change color to red on hit
        spriteRenderer.color = Color.red;

        // Reset color after 0.5 seconds
        StartCoroutine(ResetColor());

        // Apply knockback
        rb.AddForce(knockback);
    }

    public void OnHit(float damage)
    {
        Health -= damage;

        // Change color to red on hit
        spriteRenderer.color = Color.red;
        StartCoroutine(ResetColor());
    }

    IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(0.1f);  // Wait for 0.5 seconds
        spriteRenderer.color = Color.white;     // Reset to original color (white)
    }

    public void DestroySelf()
    {
        Defeated();
    }
}
