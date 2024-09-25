using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Enemy : MonoBehaviour, Idamageable
{
     private Animator animator;
     Rigidbody2D rb;

     Collider2D physicsCollider;

   
    public float Health
    {
        set
        {
            _health = value;
            if(_health <= 0)
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
            //rb.simulated = value;
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
    }
    public void Defeated()
    {
        Destroy(gameObject);
    }

    public void OnHit(float damage, Vector2 knockback)
    {
         Health -= damage;
        print("Hit");
        //apply knockback
        rb.AddForce(knockback);
    }

    public void OnHit(float damage)
    {
         Health -= damage;
    }

    public void DestroySelf()
    {
        Defeated();
    }
}
