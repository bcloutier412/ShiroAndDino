using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;

    Vector2 movementInput;
    Rigidbody2D rb;
    private Animator animator;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (movementInput != Vector2.zero)
        {
            bool success = TryMove(movementInput);

            if (!success)
            {
                success = TryMove(new Vector2(movementInput.x, 0));
            }

             if (!success)
                {
                    success = TryMove(new Vector2(0, movementInput.y));
                }

            // Update animator parameters
            animator.SetFloat("X", movementInput.x);
            animator.SetFloat("Y", movementInput.y); 
            animator.SetBool("isWalking", true);  // Ensure this matches the Animator parameter

            // Flip sprite based on the direction of X movement
            if (movementInput.x < 0) // Moving left
            {
                transform.localScale = new Vector3(-1, 1, 1); // Flip on the X-axis
            }
            else if (movementInput.x > 0) // Moving right
            {
                transform.localScale = new Vector3(1, 1, 1); // Reset to original scale
            }
        }
        else
        {
            // Stop walking animation
            animator.SetBool("isWalking", false);
        }
    }

    private bool TryMove(Vector2 direction)
    {

        if(direction != Vector2.zero)
        {
        // Casting for collisions
        int count = rb.Cast
        (
            direction,  // Correctly use direction here
            movementFilter,
            castCollisions,
            moveSpeed * Time.fixedDeltaTime + collisionOffset
        );

        // If no collisions, move the player
        if (count == 0)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            return true;
        }
        else
        {
            return false;
        }
        }
        else
        {
            return false;
        }
    }

    // Input System callback for movement
    void OnMove(InputValue movementValue)
    {
        // Just update movement input here, let FixedUpdate handle animation
        movementInput = movementValue.Get<Vector2>();
    }

    void OnFire()
    {
        animator.SetTrigger("slashAttack");
    }
}
