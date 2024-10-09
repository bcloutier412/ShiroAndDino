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

    // New variables for double hit functionality
    public bool doubleHit = false;
    private bool canDoubleHit = false;
    public float doubleHitWindow = 2.0f; // Time allowed for double hit
    private float doubleHitTimer = 0f;

    // Cooldown variables for the double hit
    public float doubleHitCooldown = 2.0f; // Cooldown duration
    private float doubleHitCooldownTimer = 0f; // Tracks cooldown time
    private bool isOnCooldown = false; // To track if cooldown is active


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            if (movementInput != Vector2.zero)
            {
                bool success = TryMove(movementInput);
                if (!success) success = TryMove(new Vector2(movementInput.x, 0));
                if (!success) success = TryMove(new Vector2(0, movementInput.y));

                // Update animator parameters
                animator.SetFloat("X", movementInput.x);
                animator.SetFloat("Y", movementInput.y);
                animator.SetBool("isWalking", true);

                // Flip sprite based on direction
                if (movementInput.x < 0) transform.localScale = new Vector3(-1, 1, 1);
                else if (movementInput.x > 0) transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }
        }

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
    if (!isOnCooldown) // Only allow attack if not on cooldown
    {
        if (canDoubleHit)
        {
            // Trigger double attack animation
            StartDoubleHit();
        }
        else
        {
            // Normal sword attack
            StartSwordAttack();
        }
    }
    else
    {
        Debug.Log("Cannot attack, double hit is on cooldown");
    }
}


    public void StartSwordAttack()
{
    Debug.Log("Sword attack started");

    // Handle the regular attack
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
    doubleHitTimer = doubleHitWindow; // Set the timer to the window duration
}


    public void StartDoubleHit()
    {
        animator.SetBool("doubleHit", true); // Assume doubleHit is an animation parameter
        if (spriteRenderer.flipX == true) swordAttack.AttackLeft();
        else swordAttack.AttackRight();
        canDoubleHit = false;
        LockMovement();
        isOnCooldown = true;
        doubleHitCooldownTimer = doubleHitCooldown; 

        // Reset double hit after performing the action
        
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

}
