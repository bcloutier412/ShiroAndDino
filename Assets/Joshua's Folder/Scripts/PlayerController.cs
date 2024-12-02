using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public PlayerData playerData; // Reference to the ScriptableObject


    private float moveSpeed;
    private float collisionOffset;

    public SwordAttack swordAttack;

    Vector2 movementInput;
    Rigidbody2D rb;
    private Animator animator;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    private bool canMove = true;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    // New variables for double hit functionality
    private bool canDoubleHit = false;
    private float doubleHitTimer = 0f;
    private bool isOnCooldown = false; // To track if cooldown is active
    public float doubleHitWindow = 2.0f;
    public float doubleHitCooldown = 5.0f;
    private float doubleHitCooldownTimer; // Timer for double hit cooldown

    public AudioSource swordSlashAudio;
    public AudioClip sfx1, sfx2, sfx3;

    private float invincibilityTimer;
    public bool isInvincible = false; // Invincibility state

    public int sceneIndex;


    public ContactFilter2D movementFilter;

    private GameManager gameManager;

    Vector3 startPosition;

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        gameManager = FindObjectOfType<GameManager>();

        moveSpeed = playerData.moveSpeed;
        collisionOffset = playerData.collisionOffset;
        startPosition = playerData.LoadPlayerPosition();
        transform.position = startPosition;
        //Vector3 startPosition = playerData.LoadPlayerPosition();

        if (playerData.finishedLoadingData == true)
            return;
        else
        {
            LoadPlayerData();
            playerData.finishedLoadingData = true;
        }
    }

    void LoadPlayerData()
    {
        collisionOffset = playerData.collisionOffset;

        // Load the saved scene index
        int sceneIndex = playerData.LoadSceneIndex();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // Only load a new scene if the saved index is different from the current scene
        if (sceneIndex != currentSceneIndex)
        {
            Debug.Log($"Loading scene index: {sceneIndex} (current: {currentSceneIndex})");
            SceneManager.LoadScene(sceneIndex);
            // playerData.SaveSceneIndex(currentSceneIndex);
        }
        Vector3 startPosition = playerData.LoadPlayerPosition();
    }



    void FixedUpdate()
    {
        // playerData.SavePlayerPosition(transform.position);
        if (canMove)
        {
            HandleMovement();
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
    }

    public void StartSwordAttack()
    {
        swordSlashAudio.Play();

        if (spriteRenderer.flipX)
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
        doubleHitTimer = playerData.doubleHitWindow; // Use value from ScriptableObject
    }

    public void StartDoubleHit()
    {
        animator.SetBool("doubleHit", true);
        if (spriteRenderer.flipX)
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
        doubleHitCooldownTimer = playerData.doubleHitCooldown; // Use value from ScriptableObject

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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Apply damage to the player if not invincible
            if (!isInvincible)
            {
                TakeDamage(1); // Ensure the correct amount of damage is applied
            

            }
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Player took damage: " + damage);
        playerData.currentHealth -= damage; // Directly update PlayerData's current health
        playerData.currentHealth = Mathf.Clamp(playerData.currentHealth, 0, playerData.maxHealth); // Clamp health

        Debug.Log("Current Health after damage: " + playerData.currentHealth);
          if (sfx1 != null)
    {
        swordSlashAudio.PlayOneShot(sfx1);
    }
        if (playerData.currentHealth <= 0)
        {
            Die();
        }



    }



    void Die()
    {
        animator.SetTrigger("death"); // Trigger death animation
        float animationLength = 1f; // Adjust as necessary
        gameManager.ShowGameOverScreenAfterDelay(animationLength); // Call the coroutine to show game over after the animation
    }

    public void StartInvincibility()
    {
        isInvincible = true;
        invincibilityTimer = playerData.invincibilityDuration; // Use invincibility duration from ScriptableObject
        StartCoroutine(HandleInvincibilityFrames());
    }

    private IEnumerator HandleInvincibilityFrames()
    {
        // Flash the player's color or make them transparent during invincibility
        float elapsedTime = 0f;

        while (elapsedTime < invincibilityTimer)
        {
            // Toggle the visibility of the player sprite or play an invincibility animation
            spriteRenderer.color = Color.red; // Example of making the player invisible
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(0.1f);

            elapsedTime += 0.2f; // Update elapsed time
        }
        // End invincibility
        isInvincible = false;
    }

    private void OnApplicationQuit()
    {
        // Save the player's current position
        playerData.SavePlayerPosition(transform.position);

        // Save the current scene build index before quitting
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        playerData.SaveSceneIndex(sceneIndex);
        playerData.finishedLoadingData = false;
    }

    internal void TakeDamage(object damageAmount)
    {
        throw new NotImplementedException();
    }
}