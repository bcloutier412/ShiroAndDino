using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;

    public SwordAttack swordAttack;

    Vector2 movementInput;
    Rigidbody2D rb;
    private Animator animator;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    private bool canMove = true;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;  

    // New variables for double hit functionality
    public bool doubleHit = false;
    private bool canDoubleHit = false;
    public float doubleHitWindow = 2.0f; // Time allowed for double hit
    private float doubleHitTimer = 0f;

    // Cooldown variables for the double hit
    public float doubleHitCooldown = 2.0f; // Cooldown duration
    private float doubleHitCooldownTimer = 0f; // Tracks cooldown time
    private bool isOnCooldown = false; // To track if cooldown is active

    public AudioSource swordSlashAudio;
    public AudioClip sfx1, sfx2, sfx3;

    public float invincibilityDuration = 1.5f;  
    private bool isInvincible = false;          


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void FixedUpdate()
    {
        if (canMove == true)
        {
            HandleMovement();
        }

       if (isInvincible == true)
    {
        invincibilityDuration -= Time.deltaTime;
        if (invincibilityDuration <= 0)
        {
            EndInvincibility();
        }
    }

        // Handle double hit window timing
        HandleDoubleHitWindow();

        // Handle cooldown timing
        HandleCooldown();
    }

    void HandleMovement()
    {
        if (movementInput != Vector2.zero)
        {
            bool success = TryMove(movementInput);
            if (!success) success = TryMove(new Vector2(movementInput.x, 0));
            if (!success) success = TryMove(new Vector2(0, movementInput.y));

            animator.SetFloat("X", movementInput.x);
            animator.SetFloat("Y", movementInput.y);
            animator.SetBool("isWalking", true);

            if (movementInput.x < 0) transform.localScale = new Vector3(-1, 1, 1);
            else if (movementInput.x > 0) transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    void HandleDoubleHitWindow()
    {
        if (canDoubleHit)
        {
            doubleHitTimer -= Time.fixedDeltaTime;
            if (doubleHitTimer <= 0)
            {
                canDoubleHit = false;
                animator.SetBool("doubleHit", false);
                Debug.Log("Double hit window expired");
            }
            else
            {
                Debug.Log("Double hit window active, time left: " + doubleHitTimer);
            }
        }
    }

    void HandleCooldown()
    {
        if (isOnCooldown)
        {
            doubleHitCooldownTimer -= Time.fixedDeltaTime;
            if (doubleHitCooldownTimer <= 0)
            {
                isOnCooldown = false;
                Debug.Log("Double hit cooldown ended");
            }
            else
            {
                Debug.Log("Double hit cooldown active, time left: " + doubleHitCooldownTimer);
            }
        }
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            int count = rb.Cast(direction, movementFilter, castCollisions, moveSpeed * Time.fixedDeltaTime + collisionOffset);
            if (count == 0)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
        }
        return false;
    }

    // Input System callback for movement
    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnFire()
    {
        // Allow a single hit regardless of cooldown
        if (!animator.GetBool("swordAttack"))
        {
            StartSwordAttack(); // Single hit always allowed
        }

        // Double hit only allowed if not on cooldown
        if (!isOnCooldown && canDoubleHit)
        {
            StartDoubleHit();
        }
        else if (isOnCooldown)
        {
            Debug.Log("Double hit is on cooldown, but single hit allowed.");
        }
    }


    public void StartSwordAttack()
    {
        Debug.Log("Sword attack started");
        swordSlashAudio.Play();

        if (spriteRenderer.flipX == true)
        {
            animator.SetBool("swordAttack", true);
            swordAttack.AttackLeft();
        }
        else
        {
            animator.SetBool("swordAttack", true);
            swordAttack.AttackRight();
        }

        LockMovement();

        // Start the double hit window
        canDoubleHit = true;
        doubleHitTimer = doubleHitWindow;
    }

    public void StartDoubleHit()
    {
        Debug.Log("Double hit started");
        swordSlashAudio.Play();

        animator.SetBool("doubleHit", true);
        if (spriteRenderer.flipX == true)
        {
            swordAttack.AttackLeft();
        }
        else
        {
            swordAttack.AttackRight();
        }

        // Prevent further double hits and trigger cooldown
        canDoubleHit = false;
        isOnCooldown = true;
        doubleHitCooldownTimer = doubleHitCooldown;

        LockMovement();
    }


    public void EndSwordsAttack()
    {
        swordAttack.StopAttack();
        animator.SetBool("swordAttack", false);
        animator.SetBool("doubleHit", false);
        UnlockMovement();
    }

    public void LockMovement()
    {
        canMove = false;
    }
    public void UnlockMovement()
    {
        canMove = true;
    }

void OnTriggerEnter2D(Collider2D other)
{
    // Check if the player triggered the enemy's collider
    if (other.CompareTag("Enemy"))
    {
        StartInvincibility();
    }
}



void StartInvincibility()
{
    isInvincible = true;
    StartCoroutine(FlashDuringInvincibility());
}

// End Invincibility
void EndInvincibility()
{
    isInvincible = false;
    Debug.Log("Player is no longer invincible.");
    spriteRenderer.color = Color.white;  
    StopCoroutine(FlashDuringInvincibility());
}

// Coroutine for Flashing Effect
IEnumerator FlashDuringInvincibility()
{
    while (isInvincible)
    {
        // Flash white
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);  // Stay white for a short time

        // Return to the original color
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);  // Stay original color for a short time
    }
}




}
